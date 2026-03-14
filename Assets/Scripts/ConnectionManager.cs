using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ConnectionManager : MonoBehaviour
{
    private string username = "Player";
    private string ip = "";
    public GameObject usernameInputField;
    public GameObject ipInputField;
    public GameObject hostButton;
    public GameObject startButton;
    public ErrorUIManager errorManager;
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {

    }

    public void updateUsername()
    {
        username = usernameInputField.GetComponent<Text>().text;
    }

    public void updateIp()
    {
        ip = ipInputField.GetComponent<Text>().text;
        if (ip.Length == 0) Debug.LogError("Invalid Ip Address");
    }

    public void hostGame()
    {
        NetworkManager.singleton.StartHost();
    }

    void connectToServer()
    {
        NetworkManager.singleton.networkAddress = ip;
        NetworkManager.singleton.StartClient();
    }
}