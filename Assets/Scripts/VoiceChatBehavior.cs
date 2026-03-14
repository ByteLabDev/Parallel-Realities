using System;
using System.IO;
using System.IO.Compression;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class VoiceChatBehavior : NetworkBehaviour
{
    AudioSource audioSource;
    public AudioSource audioSourceEar;
    byte[] audioData;
    public int sampleRate = 20000;
    public int micSamplePacketSize = 8350;
    public int lengthSec = 120;
    int lastSample;
    public SettingsManager settingsManager;
    float delay = 0;

    public AudioSource commsOpen;
    public AudioClip commsOpenClip;
    public AudioClip commsNoise;

    void Start()
    {
        if (!isLocalPlayer) return;
        settingsManager.LoadSettings();
        audioSource = GetComponent<AudioSource>();
        if (!settingsManager.voiceChat) return;
        // Don't start the microphone here
    }

    bool micToggle = false;

    void ToggleMicrophone()
    {
        micToggle = !micToggle;
        if (micToggle)
        {
            Debug.Log("Toggle ON");
            if (!Microphone.IsRecording(null))
            {
                startTransmission();
                float timer = 0;
                audioSource.clip = Microphone.Start(settingsManager.microphone, true, lengthSec, sampleRate);

                float startTime = Time.realtimeSinceStartup;
                
                while (!(Microphone.GetPosition(null) > 0) && timer < 1000)
                {
                    timer += Time.deltaTime;
                }

                if (timer >= 1000)
                {
                    Debug.LogError("Failed to play from microphone.");
                }

                float timeToStart = Time.realtimeSinceStartup - startTime;

                Debug.Log($"Microphone started in {timer/1000} (Time.DeltaTime)");
                Debug.Log($"Microphone started in {timeToStart} (Time.realtimeSinceStartup)");

                //delay = timer / 1000;
                delay = timeToStart;

                lastSample = Microphone.GetPosition(null);
            }
        }
        else
        {
            

            // Delay 5 seconds
            StartCoroutine(DelayedStop());
        }
    }

    IEnumerator DelayedStop()
    {
        commsOpen.Stop();
        yield return new WaitForSeconds(delay * 10 + 0.1f);
        Debug.Log("Toggle OFF");
        if (Microphone.IsRecording(null))
        {
            Microphone.End(null);
            Debug.Log("Microphone stopped");
            endTransmission();
        }
    }


    void Update()
    {
        if (!isLocalPlayer) return;
        if (!settingsManager.voiceChat) return;

        if(Input.GetButtonDown("PushToTalk")) ToggleMicrophone();

        int pos = Microphone.GetPosition(null);
        int diff = pos - lastSample;

        if (diff >= micSamplePacketSize)
        {
            {
                float[] floatData = new float[diff];
                audioSource.clip.GetData(floatData, lastSample);

                // convert to byte array
                byte[] byteData = new byte[floatData.Length * 4];
                Buffer.BlockCopy(floatData, 0, byteData, 0, byteData.Length);
                CmdSendAudio(byteData);
            }
            lastSample = pos;
        }
    }

    [Command]
    private void startTransmission()
    {
        commsOpen.loop = true;
        commsOpen.clip = commsNoise;
        commsOpen.Play();
    }

    [Command]
    private void endTransmission()
    {
        commsOpen.loop = false;
        commsOpen.clip = commsOpenClip;
        commsOpen.Play();
    }

    [Command(channel = Channels.Unreliable)]
    private void CmdSendAudio(byte[] _data)
    {
        //Debug.Log("CmdSend:src " + _data.Length);
        byte[] compressedData = Compress(_data);
        //Debug.Log("CmdSend:dst " + compressedData.Length);
        RpcSendAudio(compressedData);
    }

    // [ClientRpc(channel = Channels.Unreliable)]
    [ClientRpc(channel = Channels.Unreliable, includeOwner = false)]
    private void RpcSendAudio(byte[] _data)
    {
        Debug.Log("RpcSend " + audioSourceEar.isPlaying);
        float[] floatData = ToFloatArray(Decompress(_data));
        AudioClip ac = AudioClip.Create("ear", floatData.Length, 1, sampleRate, false);
        ac.SetData(floatData, 0);
        audioSourceEar.clip = ac;
        if (!audioSourceEar.isPlaying)
        {
            audioSourceEar.Play();
        }
    }

    public static byte[] Compress(byte[] src)
    {
        using (var ms = new MemoryStream())
        {
            using (var ds = new DeflateStream(ms, System.IO.Compression.CompressionLevel.Fastest, true))
            {
                ds.Write(src, 0, src.Length);
            }

            ms.Position = 0;
            byte[] comp = new byte[ms.Length];
            ms.Read(comp, 0, comp.Length);
            return comp;
        }
    }

    public static byte[] Decompress(byte[] src)
    {
        using (var ms = new MemoryStream(src))
        using (var ds = new DeflateStream(ms, CompressionMode.Decompress))
        {
            using (var dest = new MemoryStream())
            {
                ds.CopyTo(dest);

                dest.Position = 0;
                byte[] decomp = new byte[dest.Length];
                dest.Read(decomp, 0, decomp.Length);
                return decomp;
            }
        }
    }

    public float[] ToFloatArray(byte[] byteArray)
    {
        int len = byteArray.Length / 4;
        float[] floatArray = new float[len];
        for (int i = 0; i < byteArray.Length; i += 4)
        {
            floatArray[i / 4] = System.BitConverter.ToSingle(byteArray, i);
        }
        return floatArray;
    }
}