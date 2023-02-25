using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    public float maxSpeed = 10f;
    public float mouseSensitivity = 2f;
    public float jumpHeight = 5;
    public GameObject cameraPivot;

    private float playerSpeed;
    private float cameraRotateX = 0f;
    private bool isGrounded = false;
    private Rigidbody _rb;
    // private Animator animator;
    private CapsuleCollider _cc;
    private float ccHalfHeight;

	private Transform WheelA;
	private Transform WheelB;

    void Start()
    {
        //--get components--
        _rb = GetComponent<Rigidbody>();
        _cc = GetComponent<CapsuleCollider>();
        ccHalfHeight = _cc.height / 2;

		WheelA = this.gameObject.transform.GetChild(0);
		WheelB = this.gameObject.transform.GetChild(1);
		
        //--hide the mosue cursor. Press Esc during play to show the cursor. --
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }


    void Update()
    {
        //--get values used for character and camera movement--
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        float mouseX = Input.GetAxis("Mouse X")*mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y")*mouseSensitivity;
        //normalize horizontal and vertical input (I am not sure about this one but it seems to work :P)
        float normalizedSpeed = Vector3.Dot(new Vector3(horizontalInput, 0f, verticalInput).normalized, new Vector3(horizontalInput, 0f, verticalInput).normalized);

        //--camera movement and character sideways rotation--
        transform.Rotate(0, mouseX, 0);
        
        cameraRotateX += mouseY;
        cameraRotateX = Mathf.Clamp(cameraRotateX, -15, 60); //limites the up/down rotation of the camera 
        cameraPivot.transform.localRotation = Quaternion.Euler(cameraRotateX, 0, 0);

        //--check if character is on the ground
        CheckGround();

        //--sets Speed, "inAir" and "isCrouched" parameters in the Animator--
        // animator.SetFloat("Speed", playerSpeed);
        // animator.SetBool("inAir", false);
        // animator.SetBool("isCrouched", false);
        
        _rb.AddForce(verticalInput * 10f * this.transform.forward, ForceMode.Force);
        SpeedControl();
        Vector3 localVelocity = transform.InverseTransformDirection(_rb.velocity);
        float rotational = (localVelocity.z / ccHalfHeight) * (180/Mathf.PI) * Time.deltaTime;
        WheelA.Rotate(rotational, 0, 0);
        WheelB.Rotate(rotational, 0, 0);
        Debug.Log($"deltaT {Time.deltaTime}");

        //--Jump behaviour--
        if (Input.GetButton("Jump") && isGrounded)
        {
            _rb.velocity = new Vector3(0, jumpHeight, 0);
        }
        if (!isGrounded)
        {
            // animator.SetBool("inAir", true);
        }
    }
    
    private void SpeedControl()
    {
        Vector3 flatVelocity = new Vector3(_rb.velocity.x, 0f, _rb.velocity.z);

        if (flatVelocity.magnitude > maxSpeed)
        {
            Vector3 limitedVelocity = flatVelocity.normalized * maxSpeed;
            _rb.velocity = new Vector3(limitedVelocity.x, _rb.velocity.y, limitedVelocity.z);
            
        }
    }

    void CheckGround()
    {
        //--send a ray from the center of the collider to the ground. The player is "grounded" if the ray distance(length) is equal to half of the capsule height--
        Physics.Raycast(_cc.bounds.center, Vector3.down, out var hit);
        if (hit.distance < (ccHalfHeight + 0.1f))
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
    }

}
