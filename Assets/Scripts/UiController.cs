using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiController : MonoBehaviour
{
    public Dropdown microphoneDropdown;
    public GameObject[] menus;
    public GameObject settingsMenu;
    private List<string> mic_DropOptions = new List<string>();

    void Start()
    {
        
        // Update Microphone List

        microphoneDropdown.ClearOptions();
        string[] devices = Microphone.devices;
        for (int i = 0; i < devices.Length; i++)
        {
            mic_DropOptions.Add(devices[i]);
        }

        microphoneDropdown.AddOptions(mic_DropOptions);

    }

    public void changeMenu(string menu)
    {
        foreach(GameObject menuObject in menus)
        {
            if (menuObject.name == menu+"Menu") menuObject.SetActive(true);
            else menuObject.SetActive(false);
        }
    }

    public void toggleSettings()
    {
        if (settingsMenu.activeSelf) settingsMenu.SetActive(false);
        else settingsMenu.SetActive(true);
    }
}
