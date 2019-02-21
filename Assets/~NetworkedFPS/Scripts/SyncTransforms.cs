using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SyncTransforms : NetworkBehaviour
{
    public float lerpRate = 15;

    public float positionThreshold = 0.5f;
    public float rotationThreshold = 5.0f;

    private Vector3 lastPosition;
    private Quaternion lastRotation;

    [SyncVar] private Vector3 syncPosition;
    [SyncVar] private Quaternion syncRotation;

    private Rigidbody rigidbody;

    // Use this for initialization
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
    }
    void LerpPosition()
    {
        if (!isLocalPlayer)
        {
            rigidbody.position = Vector3.Lerp(rigidbody.position, syncPosition, Time.deltaTime * lerpRate);
        }
    }
    void LerpRotation()
    {
        if (!isLocalPlayer)
        {
            rigidbody.rotation = Quaternion.Lerp(rigidbody.rotation, syncRotation, Time.deltaTime * lerpRate);
        }
    }

    [Command] void CmdSendPositionToServer(Vector3 _position)
    {
        syncPosition = _position;
    }
    [Command] void CmdSendRotationToServer(Quaternion _rotation)
    {
        syncRotation = _rotation;
    }

    [ClientCallback] void TransmitPosition()
    {
        if (isLocalPlayer && Vector3.Distance(rigidbody.position,lastPosition)>positionThreshold)
        {
            CmdSendPositionToServer(rigidbody.position);
            lastPosition = rigidbody.position;
        }
    }
    [ClientCallback] void TransmitRotation()
    {
        if (isLocalPlayer&& Quaternion.Angle(rigidbody.rotation,lastRotation)>rotationThreshold)
        {
            CmdSendRotationToServer(rigidbody.rotation);
            lastRotation = rigidbody.rotation;
        }
    }


    void FixedUpdate()
    {
        TransmitPosition();
        LerpPosition();

        TransmitRotation();
        LerpRotation();
    }
}
