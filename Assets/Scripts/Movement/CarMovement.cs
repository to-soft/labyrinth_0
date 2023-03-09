using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarMovement : MonoBehaviour
{
    public WheelCollider lwheel;
    public WheelCollider rwheel;
    public float maxMotorTorque;
    public float maxSteeringAngle;
    public WheelSettings wheelSettings;


    // Start is called before the first frame update
    void Start()
    {
        wheelSettings.applyToWheel(lwheel);
        wheelSettings.applyToWheel(rwheel);
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
        
        lwheel.steerAngle = steering;
        rwheel.steerAngle = steering;
        lwheel.motorTorque = motor;
        rwheel.motorTorque = motor;
        
        applyPositionToWheel(lwheel);
        applyPositionToWheel(rwheel);
        
    }
}


[System.Serializable]
public class WheelSettings
{
    public float mass;
    public float dampingRate;
    public float suspensionDistance;
    // public JointSpring suspensionSpring;
    public Friction forwardFriction;
    public Friction sidewaysFriction;

    public void applyToWheel(WheelCollider wheel)
    {
        wheel.mass = mass / 2;
        wheel.wheelDampingRate = dampingRate;
        wheel.suspensionDistance = suspensionDistance;
        // wheel.suspensionSpring = suspensionSpring;
        WheelFrictionCurve forward = wheel.forwardFriction;
        forward.extremumSlip = forwardFriction.extrenumSlip;
        forward.extremumValue = forwardFriction.extrenumForce;
        forward.asymptoteSlip = forwardFriction.asymptoteSlip;
        forward.asymptoteValue = forwardFriction.asymptoteForce;
        wheel.forwardFriction = forward;
        WheelFrictionCurve sideways = wheel.sidewaysFriction;
        forward.extremumSlip = sidewaysFriction.extrenumSlip;
        forward.extremumValue = sidewaysFriction.extrenumForce;
        forward.asymptoteSlip = sidewaysFriction.asymptoteSlip;
        forward.asymptoteValue = sidewaysFriction.asymptoteForce;
        wheel.sidewaysFriction = sideways;
    }
}

[System.Serializable]
public class Friction
{
    public float extrenumSlip;
    public float extrenumForce;
    public float asymptoteSlip;
    public float asymptoteForce;
    public float stiffness;
}