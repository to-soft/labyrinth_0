using Unity.Mathematics;
using UnityEngine;
using UnityEngine.ProBuilder;
using Cursor = UnityEngine.Cursor;



public enum WheelName { left, right }
public class KineticWheelMovement : MonoBehaviour
{
    public float strength = 0.5f;
    public float friction = 0.1f;
    public float jumpHeight = 5;
    public GameObject cameraPivot;

    private float maxRotationalAcc = 2f;
    private float maxRotationalVel = 4f;
    private float cameraRotateX = 0f;
    private bool isGrounded = false;
    private Rigidbody _rigidbody;
    // private WheelCollider _collider;
    private WheelCollider _lCollider;
    private WheelCollider _rCollider;
    // private BoxCollider _collider;
    private float _width;
    private float _radius;
    private float _radiusPi;

    public string Fart = "Farty";

    private GameObject _wheel;
	private Transform _lwheel;
    private float _lvel;
    private float _lacc;
    private Transform _rwheel;
    private float _rvel;
    private float _racc;

    void Start()
    {
        // get components
        _wheel = gameObject.transform.GetChild(0).gameObject;
        _lwheel = _wheel.transform.GetChild(0);
        _rwheel = _wheel.transform.GetChild(1);

        _rigidbody = GetComponent<Rigidbody>();
        _lCollider = _lwheel.gameObject.GetComponent<WheelCollider>();
        _rCollider = _rwheel.gameObject.GetComponent<WheelCollider>();
        // _collider = GetComponent<BoxCollider>();
        
        // get dimensions
        _radius = _lCollider.radius; // dimensions.y / 2;
        _width = _rwheel.position.x - _lwheel.position.x;
        _radiusPi = _radius * Mathf.PI;
        
        // var dimensions = _collider.size;
        // _radius = dimensions.y / 2;
        // _width = dimensions.x;
        // _radiusPi = _radius * Mathf.PI;

        // hide the mouse cursor
        // Press Esc during play to show the cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    float getFriction(WheelName wheel)
    {
        return -1 // opposite direction
               * (wheel == WheelName.left ? _lvel : _rvel) // wheel velocity
               * friction // coefficient of friction
               * 1;  // (_rigidbody.mass * Physics.gravity).magnitude; // normal of gravity
    }

    void Update()
    {
        // get time change
        float dt = Time.deltaTime;
                
        // get input values for frame
        
        float leftInput = Input.GetAxis("Left");
        float rightInput = Input.GetAxis("Right");
        
        if (leftInput != 0) applyForce(strength * leftInput, WheelName.left);
        if (rightInput != 0) applyForce(strength * rightInput, WheelName.right);
        
        if (_lvel != 0) applyForce(getFriction(WheelName.left), WheelName.left);
        if (_rvel != 0) applyForce(getFriction(WheelName.right), WheelName.right);
        
        updateVelocities();
        
        updateWheelRotation(WheelName.left, WheelName.right, dt);
        updateWheelRotation(WheelName.right, WheelName.left, dt);
        
        cameraPivot.transform.localRotation = Quaternion.Euler(transform.localRotation.y, 0, 0);

        // check if we're on the ground
        CheckGround();

        // jump behaviour
        // if (Input.GetButton("Jump") && isGrounded)
            // _rigidbody.velocity = new Vector3(0, jumpHeight, 0);
    }
    
    void applyForce(float force, WheelName wheel)
    {
        float f = force / _lCollider.mass;
        if (wheel == WheelName.right) _racc += f;
        if (wheel == WheelName.left) _lacc += f;
    }

    void updateVelocities()
    {
        // cap acceleration
        _lacc = Mathf.Clamp(_lacc, -maxRotationalAcc, +maxRotationalAcc);
        _racc = Mathf.Clamp(_racc, -maxRotationalAcc, +maxRotationalAcc);
        _lvel += _lacc;
        _rvel += _racc;
        _lacc = 0;
        _racc = 0;
    }

    private void updateWheelRotation(WheelName movingWheel, WheelName axisWheel, float dTime)
    {
        float distance = dTime * (movingWheel == WheelName.right ? _rvel : _lvel);
        Transform mover = movingWheel == WheelName.right ? _rwheel : _lwheel;
        Transform axis = axisWheel == WheelName.left ? _lwheel : _rwheel;
        float bodyDir = movingWheel == WheelName.right ? -1 : 1;
        float bodyRotation = (bodyDir * 180 * distance) / (Mathf.PI * _width);

        Vector3 pivot = axis.position + (axis.position - mover.position);
        transform.RotateAround(pivot, Vector3.up, bodyRotation);
        
        float wheelRotation = (120 * distance) / _radiusPi;
        mover.Rotate(wheelRotation, 0, 0);
    }

    void CheckGround()
    {
        Physics.Raycast(_lCollider.bounds.center, Vector3.down, out var hit);
        isGrounded = hit.distance < (_radius + 0.1f);
    }
}
