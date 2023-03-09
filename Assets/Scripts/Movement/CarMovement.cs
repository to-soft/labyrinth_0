using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarMovement : MonoBehaviour
{
    public Axle axle;
    public float maxMotorTorque;
    public float maxSteeringAngle;


    // Start is called before the first frame update
    void Start()
    {

    }

    public void applyPositionToWheel(WheelCollider collider)
    {
        Transform wheel = collider.transform;

        Vector3 position;
        Quaternion rotation;
        collider.GetWorldPose(out position, out rotation);
        // Debug.Log(position);
        wheel.position = position;
        wheel.rotation = rotation;
    }
    
    // Update is called once per frame
    void FixedUpdate()
    {
        float motor = maxMotorTorque * Input.GetAxis("Vertical");
        float steering = maxSteeringAngle * Input.GetAxis("Horizontal");
        
        axle.lwheel.steerAngle = steering;
        axle.rwheel.steerAngle = steering;
        axle.lwheel.motorTorque = motor;
        axle.rwheel.motorTorque = motor;
        
        applyPositionToWheel(axle.lwheel);
        applyPositionToWheel(axle.rwheel);
        
    }
}


[System.Serializable]
public class Axle
{
    public WheelCollider lwheel;
    public WheelCollider rwheel;
}
