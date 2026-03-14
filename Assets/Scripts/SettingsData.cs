using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SettingsData
{

    // Display
    
    public string resolution;

    // Online
    
    public int defaultPort;

    // Audio

    public bool voiceChat;
    public bool audioAssist;
    public string microphone;
    public int masterVolume;
    public int musicVolume;
    public int sfxVolume;
    public int voiceVolume;
    public int sensitivity;
    

    public SettingsData(SettingsManager settingsManager)
    {
        resolution = settingsManager.resolution;
        defaultPort = settingsManager.defaultPort;
        voiceChat = settingsManager.voiceChat;
        audioAssist = settingsManager.audioAssist;
        microphone = settingsManager.microphone;
        masterVolume = settingsManager.masterVolume;
        musicVolume = settingsManager.musicVolume;
        sfxVolume = settingsManager.sfxVolume;
        voiceVolume = settingsManager.voiceVolume;
        sensitivity = settingsManager.sensitivity;
        
    }
}
