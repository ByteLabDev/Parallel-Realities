using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class MovePlayer : NetworkBehaviour
{
    public SettingsManager settingsManager;
    public float playerSpeed = 2.0f;
    public float shiftMultiplier = 1.5f;
    public MultiAimConstraint headAim;
    public MultiAimConstraint bodyAim;
    public GameObject camObj;
    public GameObject playerOnly;
    private Animator animator;
    private Vector3 velocity;
    private bool crouching;

    public float sensitivity = 50f;
    public Transform body;
    float xRotation = 0f;
    public Camera cam;

    public bool testMode = false;

    private void Start()
    {
        animator = GetComponent<Animator>();
        if (isLocalPlayer)
        {
            headAim.weight = 0;
            bodyAim.weight = 0;
        }
        
        if (!isLocalPlayer) return;

        settingsManager.LoadSettings();

        if (!testMode) Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        if (!isLocalPlayer) {
            cam.enabled = false;
            playerOnly.SetActive(false);
        }


        velocity = Vector3.forward * Input.GetAxis("Vertical");

        if (Input.GetKey(KeyCode.LeftControl)) crouching = true;
        else crouching = false;

        if (Input.GetKey(KeyCode.LeftShift) && !crouching) velocity = new Vector3(velocity.x + Input.GetAxis("Horizontal") / 1.25f, velocity.y, velocity.z) * playerSpeed * shiftMultiplier;
        else if(crouching && velocity.magnitude != 0) velocity = new Vector3(Input.GetAxis("Horizontal") / 1.25f, velocity.y, velocity.z) * playerSpeed / 2;
        else velocity = new Vector3(Input.GetAxis("Horizontal") / 1.25f, velocity.y, velocity.z) * playerSpeed;

        if (crouching && velocity.magnitude != 0) headAim.data.offset = new Vector3(-60, headAim.data.offset.y, headAim.data.offset.z);
        else headAim.data.offset = new Vector3(-80, headAim.data.offset.y, headAim.data.offset.z);

        if (isLocalPlayer) transform.Translate(velocity * Time.deltaTime);


        if (crouching && velocity.magnitude == 0)
        {
            camObj.transform.localPosition = new Vector3(0, 4.25f, 1);
            cam.transform.localPosition = new Vector3(0, 4.25f, 3f);
        }
        else if (crouching && velocity.magnitude >= 0.1)
        {
            camObj.transform.localPosition = new Vector3(0, 6f, 1);
            cam.transform.localPosition = new Vector3(0, 6f, 2.25f);
        }
        else
        {
            camObj.transform.localPosition = new Vector3(0, 6.5f, 0);
            cam.transform.localPosition = new Vector3(0, 6.5f, 0.75f);
        }


        if (!isLocalPlayer) return;

        if (crouching) animator.SetFloat("crouched", 1f, 0.1f, Time.deltaTime);
        else animator.SetFloat("crouched", 0f, 0.1f, Time.deltaTime);

        if (velocity.magnitude <= 0.1) animator.SetFloat("speed", 0f, 0.1f, Time.deltaTime);
        if (velocity.magnitude > 2 && velocity.magnitude <= 6.5) animator.SetFloat("speed", 0.5f, 0.1f, Time.deltaTime);
        if (velocity.magnitude > 6) animator.SetFloat("speed", 1f, 0.1f, Time.deltaTime);

        float mouseX = Input.GetAxis("Mouse X") * settingsManager.sensitivity * 10 * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * settingsManager.sensitivity * 10 * Time.deltaTime * 2;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        camObj.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        cam.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }
}