using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public float sensX;
    public float sensY;
    public Transform pivot;
    public Transform playerPrefab;
    private float yRotation;
    private float xRotation;
    
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    } 


    void Update()
    {
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensX;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensY;

        yRotation += mouseX;
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        pivot.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        playerPrefab.rotation = Quaternion.Euler(0, yRotation, 0);
        // orientation.Rotate();
        // transform.LookAt(orientation);
        // transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);

    }
    // Update is called once per frame
}
