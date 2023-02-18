using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class NewDoor : MonoBehaviour
{
    public bool isOpen = false;
    [SerializeField]
    private bool isRotatingDoor = true;
    [SerializeField]
    private float speed = 1f;

    [Header("Rotation Config")] 
    [SerializeField]
    private float rotationAmount = 90f;

    [SerializeField] private float forwardDirection = 0;

    private Vector3 _startRotation;
    private Vector3 _forward;

    private Coroutine _animationCoroutine;

    private void Awake()
    {
        _startRotation = transform.rotation.eulerAngles;
        _forward = transform.forward;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PlayerBody"))
        {
            Debug.Log("door touched by player...");
            Open(other.gameObject.transform.position);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("PlayerBody"))
        {
            Debug.Log("door zone left...");
            Close();
        }
    }

    // private void OnTriggerEnter(Collision other)
    // {
    //     Debug.Log(other.gameObject.tag);
    //     Debug.Log("Door touched by player");
    //     if (other.gameObject.CompareTag("PlayerBody"))
    //     {
    //         Debug.Log("Door zone triggered");
    //         Open(other.gameObject.transform.position);
    //     }
    // }

    public void Open(Vector3 userPosition)
    {
        if (!isOpen)
        {
            if (_animationCoroutine is not null)
            {
                StopCoroutine(_animationCoroutine);
            }

            if (isRotatingDoor)
            {
                // dot keeps track of where player is relative to door
                float dot = Vector3.Dot(_forward, (userPosition - transform.position).normalized);
                Debug.Log($"Dot: {dot:N3}");
                _animationCoroutine = StartCoroutine(DoRotationOpen(dot));
            }
        }
    }

    private IEnumerator DoRotationOpen(float forwardAmount)
    {
        Quaternion startRotation = transform.rotation;
        Quaternion endRotation;

        if (forwardAmount >= forwardDirection)
        {
            endRotation = Quaternion.Euler(new Vector3(0, _startRotation.y - rotationAmount, 0));
        }
        else
        {
            endRotation = Quaternion.Euler(new Vector3(0, _startRotation.y + rotationAmount, 0));
        }

        isOpen = true;
        float time = 0;
        while (time < 1)
        {
            transform.rotation = Quaternion.Slerp(startRotation, endRotation, time);
            yield return null;
            time += Time.deltaTime * speed;
        }
    }

    public void Close()
    {
        if (isOpen)
        {
            if (_animationCoroutine is not null)
            {
                StopCoroutine(_animationCoroutine);
            }

            if (isRotatingDoor)
            {
                _animationCoroutine = StartCoroutine(DoRotationClose());
            }
        }
    }

    private IEnumerator DoRotationClose()
    {
        Quaternion startRotation = transform.rotation;
        Quaternion endRotation = Quaternion.Euler(_startRotation);

        isOpen = false;
        float time = 0;
        while (time < 1)
        {
            transform.rotation = Quaternion.Slerp(startRotation, endRotation, time);
            yield return null;
            time += Time.deltaTime * speed;
        }

    }
}
