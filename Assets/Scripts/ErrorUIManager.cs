using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ErrorUIManager : MonoBehaviour
{
    public GameObject errorScreen;
    public GameObject canvas;
    public GameObject errorDescription;
    string error;

    void OnEnable()
    {
        Application.logMessageReceived += HandleLog;
    }

    void OnDisable()
    {
        Application.logMessageReceived -= HandleLog;
    }

    void HandleLog(string logString, string stackTrace, LogType type)
    {

        if (type == LogType.Error)
        {
            //error = error + logString;
            var popup = Instantiate(errorScreen, canvas.transform);
            //popup.GetComponent<RectTransform>().position = new Vector3(0, 0, 0);
            //errorDescription.GetComponent<Text>().text = error;
        }
    }

    public void displayErrorScreen(string errorMessage)
    {
        Instantiate(errorScreen);
        errorDescription.GetComponent<Text>().text = errorMessage;
    }

}
