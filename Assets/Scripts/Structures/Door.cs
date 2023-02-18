using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Door : MonoBehaviour
{
    [Header("Door")] 
    public GameObject upperDoorObject; // (pos y to 2.94; rot x to 90)
    public GameObject lowerDoorObject; // (pos y to -0.05; rot x to 60)
    public bool isOpen = false;
    public float m_speed = 3f;
    // public float duration = 1;
    private Vector3 lowerDoorClosedPosition;
    private Vector3 upperDoorClosedPosition;
    private Quaternion lowerDoorClosedRotation;
    private Quaternion upperDoorClosedRotation;

    private Vector3 lowerDoorOpenPosition;
    private Vector3 upperDoorOpenPosition;
    private Quaternion upperDoorOpenRotation;
    private Quaternion lowerDoorOpenRotation;

    private Rigidbody lowerDoor;
    private Rigidbody upperDoor;

    private Vector3 m_lowerRotationSpeed;
    private Vector3 m_upperRotationSpeed;

    private Vector3 moveSpeed;
    private void Start()
    {
        lowerDoor = lowerDoorObject.gameObject.GetComponent<Rigidbody>();
        upperDoor = upperDoorObject.gameObject.GetComponent<Rigidbody>();
        
        lowerDoorClosedPosition = lowerDoor.transform.position;
        lowerDoorClosedRotation = lowerDoor.transform.rotation;
        upperDoorClosedPosition = upperDoor.transform.position;
        upperDoorClosedRotation = upperDoor.transform.rotation;

        // Vector3 x = new Vector3(0, -0.5f, 0);
        // lowerDoorOpenPosition = lowerDoor.position + new Vector3(0, -0.05f, 0);
        lowerDoorOpenPosition = new Vector3(lowerDoor.position.x, -0.05f, lowerDoor.position.y);
        lowerDoorOpenRotation = lowerDoorClosedRotation * Quaternion.Euler(0, -60, 0);
        upperDoorOpenRotation = upperDoorClosedRotation * Quaternion.Euler(0, -90, 0);

        // m_lowerRotationSpeed = new Vector3(0, 100, 0);
        m_upperRotationSpeed = new Vector3(0, -10, 0);
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
                lowerDoor.MovePosition(lt.position + new Vector3(0, -1.45f, 0) * Time.fixedDeltaTime);
                // lowerDoor.MovePosition(lowerDoorOpenPosition);
            }

            if (lt.rotation != lowerDoorOpenRotation)
            {
                lowerDoor.MoveRotation(lt.rotation * Quaternion.Euler(new Vector3(0, -60, 0) * Time.fixedDeltaTime));
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
                lowerDoor.MoveRotation(lt.rotation * Quaternion.Euler(new Vector3(0, 60, 0) * Time.fixedDeltaTime));
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
    //
    // IEnumerator MoveDoor()
    // {
    //     float timeElapsed = 0;
    //     
    //     while (timeElapsed < duration)
    //     {
    //         float t = timeElapsed / duration;
    //         if (isOpen)
    //         {
    //             m_lowerRigidBody.Move(
    //                 Vector3.Lerp(lowerDoorOpenPosition, lowerDoorClosedPosition, t), 
    //                 Quaternion.Lerp(lowerDoorOpenRotation, lowerDoorClosedRotation, t) 
    //                 );
    //             m_upperRigidBody.MoveRotation(Quaternion.Lerp(upperDoorOpenRotation, upperDoorClosedRotation, t));
    //         }
    //         else
    //         {
    //             m_lowerRigidBody.Move(
    //                 Vector3.Lerp(lowerDoorClosedPosition, lowerDoorOpenPosition, t),
    //                 Quaternion.Lerp(lowerDoorClosedRotation, lowerDoorOpenRotation, t)
    //                 );
    //             m_upperRigidBody.MoveRotation(Quaternion.Lerp(upperDoorClosedRotation, upperDoorOpenRotation, t));
    //         }
    //
    //     }
    //
    //     isOpen = !isOpen;
    //     yield return null;
    // }
    //
    // private void Open()
    // {
    //     Debug.Log("door opening...");
    //     m_lowerRigidBody.MovePosition(lowerDoorOpenPosition * Time.deltaTime * m_speed);
    //     // m_lowerRigidBody.MoveRotation(Quaternion.Lerp(lowerDoorClosedRotation, lowerDoorOpenRotation, m_speed));
    // }
    //
    // private void Close()
    // {
    //     isOpen = false;
    //     Debug.Log("door closing...");
    //     lowerDoor.transform.position = lowerDoorClosedPosition;
    //     lowerDoor.transform.rotation = lowerDoorClosedRotation;
    //     upperDoor.transform.rotation = upperDoorClosedRotation;
    // }
    //
    //
    
}
