using Unity.Mathematics;
using UnityEngine;
using UnityEngine.ProBuilder;
using Cursor = UnityEngine.Cursor;

public class CharacterMovement : MonoBehaviour
{
    public float strength = 1f;
    public float drag = 0.5f;
    public float jumpHeight = 5;
    public GameObject cameraPivot;

    private float maxRotationalAcc = 2f;
    private float maxRotationalVel = 4f;
    private float cameraRotateX = 0f;
    private bool isGrounded = false;
    private Rigidbody _rigidbody;
    // private Animator animator;
    private BoxCollider _collider;
    private float _width;
    private float _radius;
    private float _radiusPi;

	private Transform _lwheel;
    private float _lvel;
    private float _lacc;
    private Transform _rwheel;
    private float _rvel;
    private float _racc;

    void Start()
    {
        // get components
        _lwheel = this.gameObject.transform.GetChild(1);
        _rwheel = this.gameObject.transform.GetChild(0);

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

        _lacc = Mathf.Clamp(strength * leftInput, -maxRotationalAcc, +maxRotationalAcc);
        _racc = Mathf.Clamp(strength * rightInput, -maxRotationalAcc, +maxRotationalAcc);

        _lvel += _lacc * dt;
        if (_lvel > 0)
        {
            _lvel -= drag * dt;
            _lvel = Mathf.Clamp(_lvel, 0, +maxRotationalVel);
        }
        if (_lvel < 0)
        {
            _lvel += drag * dt;
            _lvel = Mathf.Clamp(_lvel,  -maxRotationalVel, 0);
        }
        
        // Debug.Log($"left acc: {_lacc} vel: {_lvel}");

        _rvel += _racc * dt;
        if (_rvel > 0)
        {
            _rvel -= drag * dt;
            _rvel = Mathf.Clamp(_rvel, 0, +maxRotationalVel);
        }
        if (_rvel < 0)
        {
            _rvel += drag * dt;
            _rvel = Mathf.Clamp(_rvel,  -maxRotationalVel, 0);
        }
        
        // Debug.Log($"right acc: {_racc} vel: {_rvel}");
        
        moveOneWheel(_lvel * dt, _lwheel, _rwheel);
        moveOneWheel(_rvel * dt, _rwheel, _lwheel);
        
        
        // Debug.Log($"rotation: {transform.rotation} lw: {_lwheel.localRotation} rw {_rwheel.rotation}");

        
        // just pressing right - rotate around left
        // if (leftInput == 0f)
        // {
        //     if (rightInput > 0)
        //     {
        //         _racc = strength * rightInput;
        //     }
        //     if (rightInput < 0) moveOneWheel(_rightWheel, _leftWheel, rightInput);
        // }
        // // just pressing left - rotate around right
        // if (rightInput == 0)
        // {
        //     if (leftInput > 0) moveOneWheel(_leftWheel, _rightWheel, leftInput);
        //     if (leftInput < 0) moveOneWheel(_leftWheel, _rightWheel, leftInput);
        // }
        //
        // rotate camera according to master local rotation
        cameraPivot.transform.localRotation = Quaternion.Euler(transform.localRotation.y, 0, 0);

        //
        // if (rightInput > 0 && leftInput > 0)
        // {
        //     _rigidbody.AddForce(10f * this.transform.forward, ForceMode.Force);
        //     SpeedControl();
        //     Vector3 localVelocity = transform.InverseTransformDirection(_rigidbody.velocity);
        //     float rotational = (localVelocity.z * dt * 180) / _radiusPi;
        //     _leftWheel.Rotate(rotational, 0, 0);
        //     _rightWheel.Rotate(rotational, 0, 0);
        // }
        //
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

    private void moveOneWheel(float distance, Transform movingWheel, Transform axisWheel)
    {
        float bodyDir = (movingWheel == _rwheel) ? -1 : 1;
        float bodyRotation = (bodyDir * 180 * distance) / (Mathf.PI * _width);

        Vector3 pivot = axisWheel.position + (axisWheel.position - movingWheel.position);
        transform.RotateAround(pivot, Vector3.up, bodyRotation);
        
        float wheelRotation = (120 * distance) / _radiusPi;
        movingWheel.Rotate(wheelRotation, 0, 0);
    }


    private void SpeedControl()
    {
        // Vector3 flatVelocity = new Vector3(_rigidbody.velocity.x, 0f, _rigidbody.velocity.z);
        //
        // if (flatVelocity.magnitude > maxSpeed)
        // {
        //     Vector3 limitedVelocity = flatVelocity.normalized * maxSpeed;
        //     _rigidbody.velocity = new Vector3(limitedVelocity.x, _rigidbody.velocity.y, limitedVelocity.z)
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
