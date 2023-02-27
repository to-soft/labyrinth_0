using UnityEngine;
using Cursor = UnityEngine.Cursor;

public class CharacterMovement : MonoBehaviour
{
    public float maxSpeed = 10f;
    public float mouseSensitivity = 2f;
    public float jumpHeight = 5;
    public GameObject cameraPivot;

    private float playerSpeed;
    private float cameraRotateX = 0f;
    private bool isGrounded = false;
    private Rigidbody _rigidbody;
    // private Animator animator;
    private BoxCollider _collider;
    private float _width;
    private float _radius;
    private float _radiusPi;

	private Transform _rightWheel;
	private Transform _leftWheel;

    void Start()
    {
        // get components
        _rightWheel = this.gameObject.transform.GetChild(0);
        _leftWheel = this.gameObject.transform.GetChild(1);
        
        _rigidbody = GetComponent<Rigidbody>();
        _collider = GetComponent<BoxCollider>();
        
        // get dimensions
        var dimensions = _collider.size;
        _radius = dimensions.y / 2;
        _width = dimensions.x;
        _radiusPi = _radius * Mathf.PI;

        // hide the mouse cursor
        // Press Esc during play to show the cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // get input values for frame
        // float verticalInput = Input.GetAxis("Vertical");
        // float horizontalInput = Input.GetAxis("Horizontal");
        // float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        // float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;
        
        // get time change
        float dt = Time.deltaTime;
        
        // get input values for frame
        float leftInput = Input.GetAxis("Left");
        float rightInput = Input.GetAxis("Right");
        
        Debug.Log($"left: {leftInput} right: {rightInput}");

        // just pressing right - rotate around left
        if (leftInput == 0f)
        {
            if (rightInput > 0) moveOneWheel(_rightWheel, _leftWheel);
            if (rightInput < 0) moveOneWheel(_rightWheel, _leftWheel, -1);
        }
        // just pressing left - rotate around right
        if (rightInput == 0)
        {
            if (leftInput > 0) moveOneWheel(_leftWheel, _rightWheel);
            if (leftInput < 0) moveOneWheel(_leftWheel, _rightWheel, -1);
        }
        
        // rotate camera according to master local rotation
        cameraPivot.transform.localRotation = Quaternion.Euler(transform.localRotation.y, 0, 0);


        if (rightInput > 0 && leftInput > 0)
        {
            _rigidbody.AddForce(10f * this.transform.forward, ForceMode.Force);
            SpeedControl();
            Vector3 localVelocity = transform.InverseTransformDirection(_rigidbody.velocity);
            float rotational = (localVelocity.z * dt * 180) / _radiusPi;
            _leftWheel.Rotate(rotational, 0, 0);
            _rightWheel.Rotate(rotational, 0, 0);
        }
        
        // check if we're on the ground
        CheckGround();
        
        
        
        // rotate player according to horizontal mouse pos
        // transform.Rotate(0, mouseX, 0);
        
        // rotate camera according to vertical mouse pos
        // cameraRotateX += mouseY;
        // cameraRotateX = Mathf.Clamp(cameraRotateX, -15, 60); // set limits
        

        //--sets Speed, "inAir" and "isCrouched" parameters in the Animator--
        // animator.SetFloat("Speed", playerSpeed);
        // animator.SetBool("inAir", false);
        // animator.SetBool("isCrouched", false);
        

        //--Jump behaviour--
        if (Input.GetButton("Jump") && isGrounded)
        {
            _rigidbody.velocity = new Vector3(0, jumpHeight, 0);
        }
        // if (!isGrounded)
        // {
        //     // animator.SetBool("inAir", true);
        // }
    }

    private void moveOneWheel(Transform movingWheel, Transform axisWheel, float coef = 1)
    {
        float scale = (movingWheel == _rightWheel) ? -90 : 90;
        float dt = Time.deltaTime;
        Vector3 pivot = axisWheel.position;
        float pivotRadius = _width / 2;
        float bodyRotation = coef * scale * dt;
        transform.RotateAround(pivot, Vector3.up, bodyRotation);
        float distance = pivotRadius * (Mathf.PI / 2);
        float wheelRotation = (coef * distance * dt * 180) / _radiusPi;
        movingWheel.Rotate(wheelRotation, 0, 0);
    }


    private void SpeedControl()
    {
        Vector3 flatVelocity = new Vector3(_rigidbody.velocity.x, 0f, _rigidbody.velocity.z);

        if (flatVelocity.magnitude > maxSpeed)
        {
            Vector3 limitedVelocity = flatVelocity.normalized * maxSpeed;
            _rigidbody.velocity = new Vector3(limitedVelocity.x, _rigidbody.velocity.y, limitedVelocity.z);
            
        }
    }

    void CheckGround()
    {
        //--send a ray from the center of the collider to the ground. The player is "grounded" if the ray distance(length) is equal to half of the capsule height--
        Physics.Raycast(_collider.bounds.center, Vector3.down, out var hit);
        if (hit.distance < (_radius + 0.1f))
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
    }

}
