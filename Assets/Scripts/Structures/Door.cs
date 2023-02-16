using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [Header("Door")] 
    public GameObject upperDoor; // (pos y to 2.94; rot x to 90)
    public GameObject lowerDoor; // (pos y to -0.05; rot x to 60)
    public bool isOpen = false;
    public float duration = 1;
    // private Transform upperDoorClosed;
    private Vector3 lowerDoorClosedPosition;
    private Vector3 upperDoorClosedPosition;
    private Quaternion lowerDoorClosedRotation;
    private Quaternion upperDoorClosedRotation;

    private void Start()
    {
        lowerDoorClosedPosition = lowerDoor.transform.position;
        lowerDoorClosedRotation = lowerDoor.transform.rotation;
        upperDoorClosedRotation = upperDoor.transform.rotation;
        upperDoorClosedPosition = upperDoor.transform.position;
    }


    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.tag);
        Debug.Log("Door touched by player");
        if (other.gameObject.CompareTag("PlayerBody"))
        {
            Debug.Log("Door touched by player");
            if (!isOpen)
            {
                Open();
            }
            // GameObject playerBody = other.gameObject;
            // GameObject parent = playerBody.transform.parent.gameObject;
            // Player playerComponent = playerBody.gameObject.GetComponent<Player>();
            // playerComponent.Damage(5);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log(other.gameObject.tag);
        Debug.Log("Door touched by player");
        if (other.gameObject.CompareTag("PlayerBody"))
        {
            Debug.Log("Door left by player");
            if (isOpen)
            {
                Close();
            }
            // GameObject playerBody = other.gameObject;
            // GameObject parent = playerBody.transform.parent.gameObject;
            // Player playerComponent = playerBody.gameObject.GetComponent<Player>();
            // playerComponent.Damage(5);
        }
    }

    private void Open()
    {
        isOpen = true;
        Debug.Log("door opening...");
        lowerDoor.transform.position = new Vector3(lowerDoorClosedPosition.x, -0.05f, lowerDoorClosedPosition.z);
        lowerDoor.transform.rotation *= Quaternion.Euler(0, 300, 0);
        upperDoor.transform.rotation *= Quaternion.Euler(0, 270, 0);
    }

    private void Close()
    {
        isOpen = false;
        Debug.Log("door closing...");
        lowerDoor.transform.position = lowerDoorClosedPosition;
        lowerDoor.transform.rotation = lowerDoorClosedRotation;
        upperDoor.transform.rotation = upperDoorClosedRotation;
    }
    
    
    
}
