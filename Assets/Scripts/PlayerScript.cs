using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerScript : NetworkBehaviour
{
    public float movementSpeed = 10;
    public float rotationSpeed = 10;
    public float jumpHeight = 2;
    public bool isGrounded = false;
    public Rigidbody rigidbody;
    public string remotelayerName = "RemotePlayer";

    // Use this for initialization
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        AudioListener audioListener = GetComponentInChildren<AudioListener>();
        Camera camera = GetComponentInChildren<Camera>();
        if (isLocalPlayer)
        {
            camera.enabled = true;
            audioListener.enabled = true;
        }
        else
        {
            camera.enabled = false;
            audioListener.enabled = false;
            AssignRemoteLayer();
        }
        RegisterPlayer();
    }

    void Move(KeyCode _key)
    {
        Vector3 position = rigidbody.position;
        Quaternion rotation = rigidbody.rotation;
        switch (_key)
        {
            case KeyCode.W:
                position += transform.forward * movementSpeed * Time.deltaTime;
                break;
            case KeyCode.S:
                position += -transform.forward * movementSpeed * Time.deltaTime;
                break;
            case KeyCode.D:
                position += transform.right * movementSpeed * Time.deltaTime;
                break;
            case KeyCode.A:
                position += -transform.right * movementSpeed * Time.deltaTime;
                break;
        }
        rigidbody.MovePosition(position);


        
    }

    void HandleInput()
    {
        KeyCode[] keys =
        {
            KeyCode.W,
            KeyCode.S,
            KeyCode.A,
           KeyCode.D,
            KeyCode.Space
        };
        foreach (var key in keys)
        {
            if (Input.GetKey(key))
            {
                Move(key);
            }
        }
    }

    private void OnCollisionEnter(Collision _col)
    {
        isGrounded = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (isLocalPlayer)
        {
            HandleInput();
        }
    }
    void RegisterPlayer()
    {
        string ID = "Player" + GetComponent<NetworkIdentity>().netId;
        name = ID;
    }
    void AssignRemoteLayer()
    {
        gameObject.layer = LayerMask.NameToLayer(remotelayerName);
    }
}
