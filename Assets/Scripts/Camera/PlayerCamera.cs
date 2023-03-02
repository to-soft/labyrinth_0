using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public float sensitivity;
    public Transform pivot;
    public Transform playerPrefab;
    private float yRotation;
    private float xRotation;
    private float sensitivityFactor = 100;
    
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    
    void Update()
    {
        if (ViewManager.isOpen) { return; }
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensitivity;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensitivity;

        yRotation += mouseX; 
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        pivot.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        playerPrefab.rotation = Quaternion.Euler(0, yRotation, 0);
    }
}
