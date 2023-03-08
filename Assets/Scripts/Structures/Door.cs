using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Door : MonoBehaviour
{
    [Header("Door")] 
    public GameObject upperDoorObject;
    public GameObject lowerDoorObject;
    public bool isOpen = false;
    
    private Vector3 lowerDoorOpenPosition;
    private Quaternion lowerDoorOpenRotation;
    private Vector3 lowerDoorClosedPosition;
    private Quaternion lowerDoorClosedRotation;
    private Quaternion upperDoorOpenRotation;
    private Quaternion upperDoorClosedRotation;

    private Rigidbody upperDoor;
    private Rigidbody lowerDoor;
    
    private void Start()
    {
        lowerDoor = lowerDoorObject.gameObject.GetComponent<Rigidbody>();
        upperDoor = upperDoorObject.gameObject.GetComponent<Rigidbody>();
        
        upperDoorClosedRotation = upperDoor.transform.rotation;
        upperDoorOpenRotation = upperDoorClosedRotation * Quaternion.Euler(0, -90, 0);
        lowerDoorClosedPosition = lowerDoor.transform.position;
        lowerDoorClosedRotation = lowerDoor.transform.rotation;
        lowerDoorOpenRotation = lowerDoorClosedRotation * Quaternion.Euler(0, -70, 0);
        lowerDoorOpenPosition = new Vector3(lowerDoor.position.x, 0, lowerDoor.position.z);
        
        // Debug.Log($"upper door closed rotation: {upperDoorClosedRotation}");
        // Debug.Log($"upper door open rotation: {upperDoorOpenRotation}");
    }

    private void FixedUpdate()
    {
        Transform lt = lowerDoor.transform;
        Transform ut = upperDoor.transform;
        if (isOpen)
        {
            if (ut.rotation != upperDoorOpenRotation)
            {
                upperDoor.MoveRotation(ut.rotation * Quaternion.Euler(new Vector3(0, -90, 0) * Time.fixedDeltaTime));
            }
            if (lt.position.y > lowerDoorOpenPosition.y)
            {
                lowerDoor.MovePosition(lt.position + new Vector3(0, -2.9f, 0) * Time.fixedDeltaTime);
            }
            if (lt.rotation != lowerDoorOpenRotation)
            {
                lowerDoor.MoveRotation(lt.rotation * Quaternion.Euler(new Vector3(0, -70, 0) * Time.fixedDeltaTime));
            }
        }
        else
        {
            if (ut.rotation != upperDoorClosedRotation)
            {
                upperDoor.MoveRotation(ut.rotation * Quaternion.Euler(new Vector3(0, 90, 0) * Time.fixedDeltaTime));
            }
            if (lt.position.y < lowerDoorClosedPosition.y)
            {
                lowerDoor.MovePosition(lt.position + new Vector3(0, 1.45f, 0) * Time.fixedDeltaTime);
            }
            if (lt.rotation != lowerDoorClosedRotation)
            {
                lowerDoor.MoveRotation(lt.rotation * Quaternion.Euler(new Vector3(0, 70, 0) * Time.fixedDeltaTime));
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PlayerBody"))
        {
            Debug.Log("Door zone touched by player");
            isOpen = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("PlayerBody"))
        {
            Debug.Log("Door zone left by player");
            isOpen = false;
        }
    }
}
