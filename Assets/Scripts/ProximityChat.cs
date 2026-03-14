using UnityEngine;
using Mirror;

[RequireComponent(typeof(AudioSource))]
public class ProximityChat : NetworkBehaviour
{
    public AudioSource audioSource;

    void Start()
    {
        audioSource.clip = Microphone.Start("NVIDIA Broadcast (NVIDIA Broadcast)", true, 10, 44100);
        audioSource.loop = true;
        while (!(Microphone.GetPosition(null) > 0)) { }
        audioSource.Play();
    }

    private void Update()
    {
        
    }
}