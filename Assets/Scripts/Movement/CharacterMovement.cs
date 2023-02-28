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
    
    private BoxCollider _collider;
    private Rigidbody _rigidbody;

    private GameObject _lwheel;
    private Transform _ltransform;
    private Rigidbody _lrigidbody;
    private BoxCollider _lcollider;
	private GameObject _rwheel;
    private Transform _rtransform;
    private Rigidbody _rrigidbody;
    private BoxCollider _rcollider;


    private float _radiusPi;
    private float _radius;
    private float _width;

    void Start()
    {
        // get parent components
        _collider = GetComponent<BoxCollider>();
        _rigidbody = GetComponent<Rigidbody>();
        
        // get left wheel & components
        _lwheel = this.gameObject.transform.GetChild(0).gameObject;
        _ltransform = _lwheel.transform;
        _lrigidbody = _lwheel.GetComponent<Rigidbody>();
        // _lcollider = _rwheel.GetComponent<BoxCollider>();
        Debug.Log($"left wheel: {_lwheel} {_ltransform} {_lrigidbody}");
        
        // get right wheel & components
        _rwheel = this.gameObject.transform.GetChild(1).gameObject;
        _rtransform = _rwheel.transform;
        _rrigidbody = _rwheel.GetComponent<Rigidbody>();
        // _rcollider = _rwheel.GetComponent<BoxCollider>();
        Debug.Log($"right wheel: {_rwheel} {_rtransform} {_rrigidbody}");


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
        float verticalInput = Input.GetAxis("Vertical");
        // float horizontalInput = Input.GetAxis("Horizontal");
        // float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        // float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;
        
        // get time change
        float dt = Time.deltaTime;
        
        // get input values for frame
        float leftInput = Input.GetAxis("Left");
        float rightInput = Input.GetAxis("Right");
        
        Debug.Log($"vertical: {verticalInput}");

        float s = 20f;

        // just pressing right - rotate around left
        if (leftInput == 0f)
        {
            if (rightInput > 0)
            {
                _rrigidbody.AddForce(rightInput * s * _rtransform.forward, ForceMode.Force);
            }

            if (rightInput < 0)
            {
                _rrigidbody.AddForce(rightInput * s * _rtransform.forward, ForceMode.Force);
            }
            // if (rightInput > 0) moveOneWheel(_rWheel, _lWheel);
            // if (rightInput < 0) moveOneWheel(_rWheel, _lWheel, -1);
        }
        // just pressing left - rotate around right
        if (rightInput == 0)
        {
       
            if (leftInput > 0)
            {
                _lrigidbody.AddForce(leftInput * s * _ltransform.forward, ForceMode.Force);
            }
            if (leftInput < 0)
            {
                _lrigidbody.AddForce(leftInput * s * _ltransform.forward, ForceMode.Force);
            }
            // if (leftInput > 0) moveOneWheel(_lWheel, _rWheel);
            // if (leftInput < 0) moveOneWheel(_lWheel, _rWheel, -1);
        }
        //
        // // rotate camera according to master local rotation
        cameraPivot.transform.localRotation = Quaternion.Euler(transform.localRotation.y, 0, 0);

        if (verticalInput > 0)
        {
            // _lrigidbody.AddForce(10f * _ltransform.forward, ForceMode.Force);
            // SpeedControl();
            // Vector3 localVelocity = transform.InverseTransformDirection(_lRigidbody.velocity);
            // float rotational = (localVelocity.z * dt * 180) / _radiusPi;
            // _lTransform.Rotate(rotational, 0, 0);
            // _rTransform.Rotate(rotational, 0, 0);
        }      
        if (verticalInput < 0)
        {
            // _lrigidbody.AddForce(-10f * _ltransform.forward, ForceMode.Force);
            // SpeedControl();
            // Vector3 localVelocity = transform.InverseTransformDirection(_lRigidbody.velocity);
            // float rotational = (localVelocity.z * dt * 180) / _radiusPi;
            // _lTransform.Rotate(rotational, 0, 0);
            // _rTransform.Rotate(rotational, 0, 0);
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
        // if (Input.GetButton("Jump") && isGrounded)
        // {
        //     _rigidbody.velocity = new Vector3(0, jumpHeight, 0);
        // }
        // if (!isGrounded)
        // {
        //     // animator.SetBool("inAir", true);
        // }
    }

    private void moveOneWheel(GameObject movingWheel, GameObject axisWheel, float coef = 1)
    {
        
        // float scale = (movingWheel == _rWheel) ? -90 : 90;
        // float dt = Time.deltaTime;
        // Vector3 pivot = axisWheel.transform.position;
        // float pivotRadius = _width / 2;
        // float bodyRotation = coef * scale * dt;
        // transform.RotateAround(pivot, Vector3.up, bodyRotation);
        // float distance = pivotRadius * (Mathf.PI / 2);
        // float wheelRotation = (coef * distance * dt * 180) / _radiusPi;
        // movingWheel.transform.Rotate(wheelRotation, 0, 0);
    }


    private void SpeedControl()
    {
        // Vector3 flatVelocity = new Vector3(_rigidbody.velocity.x, 0f, _rigidbody.velocity.z);
        //
        // if (flatVelocity.magnitude > maxSpeed)
        // {
        //     Vector3 limitedVelocity = flatVelocity.normalized * maxSpeed;
        //     _rigidbody.velocity = new Vector3(limitedVelocity.x, _rigidbody.velocity.y, limitedVelocity.z);
        //     
        // }
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
