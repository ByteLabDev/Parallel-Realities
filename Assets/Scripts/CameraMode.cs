using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class CameraMode : NetworkBehaviour
{
    public Camera cam;
    private bool recording = false;

    public float barHeight = 50f; // Height of the black bars
    public Image topBar; // Reference to the top black bar image
    public Image bottomBar; // Reference to the bottom black bar image

    private bool isAnimating = false; // Flag to indicate if the bars are currently animating

    public RenderTexture camcorderTexture;
    public GameObject renderImage;


    private void Start()
    {
        RectTransform topBarRect = topBar.GetComponent<RectTransform>();
        topBarRect.anchorMin = new Vector2(0f, 1f);
        topBarRect.anchorMax = new Vector2(1f, 1f);
        topBarRect.pivot = new Vector2(0.5f, 1f);
        topBarRect.sizeDelta = new Vector2(0f, barHeight);

        RectTransform bottomBarRect = bottomBar.GetComponent<RectTransform>();
        bottomBarRect.anchorMin = new Vector2(0f, 0f);
        bottomBarRect.anchorMax = new Vector2(1f, 0f);
        bottomBarRect.pivot = new Vector2(0.5f, 0f);
        bottomBarRect.sizeDelta = new Vector2(0f, barHeight);

        // Set the initial position of the black bars
        topBarRect.anchoredPosition = new Vector2(0f, barHeight);
        bottomBarRect.anchoredPosition = new Vector2(0f, -barHeight);

        cam.targetTexture = null;   
        renderImage.SetActive(false);
    }
    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetButtonDown("Record"))
        {
            renderImage.SetActive(true);
            cam.targetTexture = camcorderTexture;
            StartCoroutine(ShowBars(.5f));
            recording = true;
            Debug.Log("Recording Started.");
        }
        else if (Input.GetButtonUp("Record"))
        {
            renderImage.SetActive(false);
            cam.targetTexture = null;
            StartCoroutine(HideBars(.5f));
            recording = false;
            Debug.Log("Recording Stopped.");
        }
    }

    IEnumerator ShowBars(float time)
    {
        isAnimating = true;

        Vector3 topBarPos = topBar.GetComponent<RectTransform>().anchoredPosition;
        Vector3 bottomBarPos = bottomBar.GetComponent<RectTransform>().anchoredPosition;
        Vector2 startPosTop = topBarPos;
        Vector2 startPosBottom = bottomBarPos;
        Vector2 finalPosTop = new Vector2(0, topBarPos.y - barHeight);
        Vector2 finalPosBottom = new Vector2(0, bottomBarPos.y + barHeight);

        float elapsedTime = 0;

        while (elapsedTime < time)
        {
            float t = elapsedTime / time;
            t = Mathf.SmoothStep(0, 1, t);

            topBar.GetComponent<RectTransform>().anchoredPosition = Vector3.Lerp(startPosTop, finalPosTop, t);
            bottomBar.GetComponent<RectTransform>().anchoredPosition = Vector3.Lerp(startPosBottom, finalPosBottom, t);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        topBar.GetComponent<RectTransform>().anchoredPosition = finalPosTop;
        bottomBar.GetComponent<RectTransform>().anchoredPosition = finalPosBottom;

        isAnimating = false;
    }


    IEnumerator HideBars(float time)
    {
        isAnimating = true;

        Vector3 topBarPos = topBar.GetComponent<RectTransform>().anchoredPosition;
        Vector3 bottomBarPos = bottomBar.GetComponent<RectTransform>().anchoredPosition;
        Vector2 startPosTop = topBarPos;
        Vector2 startPosBottom = bottomBarPos;
        Vector2 finalPosTop = new Vector2(0, topBarPos.y + barHeight);
        Vector2 finalPosBottom = new Vector2(0, bottomBarPos.y - barHeight);

        float elapsedTime = 0;

        while (elapsedTime < time)
        {
            float t = elapsedTime / time;
            t = Mathf.SmoothStep(0, 1, t);

            topBar.GetComponent<RectTransform>().anchoredPosition = Vector3.Lerp(startPosTop, finalPosTop, t);
            bottomBar.GetComponent<RectTransform>().anchoredPosition = Vector3.Lerp(startPosBottom, finalPosBottom, t);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        topBar.GetComponent<RectTransform>().anchoredPosition = finalPosTop;
        bottomBar.GetComponent<RectTransform>().anchoredPosition = finalPosBottom;

        isAnimating = false;
    }
}
