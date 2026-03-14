using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    // Display

    public string resolution = "1920 X 1080";

    // Online

    public int defaultPort = 7777;

    // Audio

    public bool voiceChat = true;
    public bool audioAssist = false;
    public string microphone = null;
    public int masterVolume = 50;
    public int musicVolume = 50;
    public int sfxVolume = 50;
    public int voiceVolume = 50;
    public int sensitivity = 50;

    public AudioSource audioSource;
    public Dropdown micDropdown;
    private int lastSample;
    private bool toggleState = false;
    public Slider sensitivitySlider;
    private Text sensitivityText;

    void Start()
    {
        // Find in children
        //micDropdown = transform.Find("MicrophoneSelection").GetComponent<Dropdown>();

        // Iterate

        if (!sensitivitySlider) return;
        foreach(var sensText in sensitivitySlider.GetComponentsInChildren<Text>())
        {
            if (sensText.name == "Value") sensitivityText = sensText;
        }
    }

    public void SaveSettings()
    {
        SaveSystem.SaveSettings(this);
    }

    public void LoadSettings()

    {
        SettingsData data = SaveSystem.LoadSettings();
        if (data != null)
        {
            resolution = data.resolution;
            defaultPort = data.defaultPort;
            voiceChat = data.voiceChat;
            audioAssist = data.audioAssist;
            microphone = data.microphone;
            masterVolume = data.masterVolume;
            musicVolume = data.musicVolume;
            sfxVolume = data.sfxVolume;
            voiceVolume = data.voiceVolume;
            sensitivity = data.sensitivity;
        }
    }
    
    public void updateSensitivity()
    {
        sensitivity = (int)sensitivitySlider.value;
        sensitivityText.text = sensitivity.ToString();
        
    }

    public void updateMicrophone()
    {
        microphone = micDropdown.options[micDropdown.value].text;
    }

    public void ToggleMicrophone()
    {
        toggleState = !toggleState;
        if (toggleState)
        {
            Debug.Log("Start recording");
            int minFreq;
            int maxFreq;
            Microphone.GetDeviceCaps("Maono Microphone (Realtek Audio USB)", out minFreq, out maxFreq);
            int frequency = maxFreq > 44100 ? 44100 : maxFreq;
            int bufferSize = 1024; // Increase this value to reduce latency, but it may affect performance
            audioSource.clip = Microphone.Start("Maono Microphone (Realtek Audio USB)", true, 1, bufferSize);
            audioSource.loop = true;

            float timer = 0;

            while (!(Microphone.GetPosition(null) > 0) && timer < 1000)
            {
                timer += Time.deltaTime;
            }

            if (timer >= 1000)
            {
                Debug.LogError("Failed to play from microphone.");
            }

            audioSource.Play();
        }
        else
        {
            Debug.Log("Disabling Mic.");
            audioSource.Stop();
            audioSource.loop = false;
            while (Microphone.IsRecording(null))
            {
                Microphone.End(null);
            }
            audioSource.clip = null;
        }
    }




}
