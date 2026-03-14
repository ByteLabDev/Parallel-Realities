using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class ShowStartButton : NetworkBehaviour
{
    public GameObject startButton;
    public NewNetworkRoomManager roomManager;
    // Start is called before the first frame update
    void Start()
    {
        roomManager = FindObjectOfType<NewNetworkRoomManager>().gameObject.GetComponent<NewNetworkRoomManager>();
        if (!isServer) startButton.SetActive(false);
    }

    public void forceReady()
    {
        Debug.Log("Hi!");

        NetworkManager.singleton.ServerChangeScene("Game");
        // Iterate clients
        foreach (var conn in NetworkServer.connections.Values)
        {
            if (conn != null)
            {
                // Get the player object for this connection
            }
        }
    }
}
