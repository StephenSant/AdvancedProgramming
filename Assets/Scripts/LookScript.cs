using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class LookScript : NetworkBehaviour
{

    public float mouseSensitivity = 2.0f;

    public float minY = -90f;
    public float maxY = 90f;

    float yaw = 0;
    float pitch = 0;

    GameObject camera;

    // Use this for initialization
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Camera cam = GetComponentInChildren<Camera>();
        if (cam != null)
        {
            camera = cam.gameObject;
        }
    }

    private void OnDestroy()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void HandleInput()
    {
        yaw += Input.GetAxis("Mouse X") * mouseSensitivity;
        pitch += Input.GetAxis("Mouse Y") * mouseSensitivity;
        pitch = Mathf.Clamp(pitch, minY, maxY);
    }

    // Update is called once per frame
    void Update()
    {
        if (isLocalPlayer)
        {
            HandleInput();
            transform.eulerAngles = new Vector3(0, yaw, 0);
        }
    }
    private void LateUpdate()
    {
        if (isLocalPlayer)
        {
            camera.transform.localEulerAngles = new Vector3(-pitch, 0, 0);
        }
    }
}
