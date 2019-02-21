using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ShootScript : NetworkBehaviour
{

    public float
        fireRate = 1,
        range = 1000;
    public LayerMask mask;

    private float fireFactor = 0;
    private GameObject mainCam;

    // Use this for initialization
    void Start()
    {
        mainCam = GetComponentInChildren<Camera>().gameObject;
    }



    [Client]void Shoot()
    {
        RaycastHit hit;
        Ray ray = new Ray(mainCam.transform.position, transform.forward);
        if (Physics.Raycast(ray, out hit, range,mask))
        {
            CmdPlayerShot(hit.collider.name);
        }
    }
    [Command]void CmdPlayerShot(string id)
    {
        Debug.Log("Player: " + id + " has been shot!");
    }
    void HandleInput()
    {
        fireFactor += Time.deltaTime;
        float fireInterval = 1 / fireRate;
        if (fireFactor >= fireInterval)
        {
            if (Input.GetMouseButton(0))
            {
                Shoot();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isLocalPlayer)
        {
            HandleInput();
        }
    }

}
