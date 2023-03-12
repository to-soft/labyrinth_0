using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysWheelMovement : MonoBehaviour
{
    public WheelCollider lwheel;
    public WheelCollider rwheel;
    public float maxMotorTorque;
    public float maxBrakeTorque;
    public WheelSettings wheelSettings;
    
    void Start()
    {
        wheelSettings.applyConfigToWheel(lwheel);
        wheelSettings.applyConfigToWheel(rwheel);
    }
    
    void FixedUpdate()
    {
        float left = Input.GetAxis("Left");
        float right = Input.GetAxis("Right");
        float brake = Input.GetAxis("Brake");
        float leftBrake = Input.GetAxis("Left Brake");
        float rightBrake = Input.GetAxis("Right Brake");
        // Debug.Log($"left:{left}  right:{right}  brake:{brake}  left brake:{leftBrake}  right brake:{rightBrake}");
        
        float lmotor = maxMotorTorque * left;
        float rmotor = maxMotorTorque * right;
        float ubrake = maxBrakeTorque * brake;
        float lbrake = maxBrakeTorque * leftBrake;
        float rbrake = maxBrakeTorque * rightBrake;

        lwheel.motorTorque = lmotor;
        rwheel.motorTorque = rmotor;
        
        lwheel.brakeTorque = ubrake;
        rwheel.brakeTorque = ubrake;

        if (lbrake > 0) lwheel.brakeTorque = lbrake;
        if (rbrake > 0) rwheel.brakeTorque = rbrake;

        if (lwheel.brakeTorque == 0f && lwheel.motorTorque == 0) lwheel.brakeTorque = 10;
        if (rwheel.brakeTorque == 0f && rwheel.motorTorque == 0) rwheel.brakeTorque = 10;
        
        applyPositionToWheel(lwheel);
        applyPositionToWheel(rwheel);
        
    }

    public void applyPositionToWheel(WheelCollider collider)
    {
        Transform wheel = collider.transform;
        Vector3 position;
        Quaternion rotation;
        collider.GetWorldPose(out position, out rotation);
        wheel.position = position;
        wheel.rotation = rotation;
    }
}


[System.Serializable]
public class WheelSettings
{
    public float mass = 200;
    public float radius = 0.7f;
    public float dampingRate = 0.25f;
    public float suspensionDistance = 0;
    public float forceAppPointDistance = 0;
    public Vector3 center = Vector3.zero;
    public Spring suspensionSpring;
    public ForwardFriction forwardFriction;
    public SidewaysFriction sidewaysFriction;

    public void applyConfigToWheel(WheelCollider wheel)
    {
        wheel.mass = mass / 2;
        wheel.radius = radius;
        wheel.center = center;
        wheel.wheelDampingRate = dampingRate;
        wheel.suspensionDistance = suspensionDistance;
        wheel.forceAppPointDistance = forceAppPointDistance;
        
        JointSpring spring = wheel.suspensionSpring;
        spring.spring = suspensionSpring.spring;
        spring.damper = suspensionSpring.damper;
        spring.targetPosition = suspensionSpring.targetPosition;
        wheel.suspensionSpring = spring;
        
        WheelFrictionCurve forward = wheel.forwardFriction;
        forward.extremumSlip = forwardFriction.extrenumSlip;
        forward.extremumValue = forwardFriction.extrenumForce;
        forward.asymptoteSlip = forwardFriction.asymptoteSlip;
        forward.asymptoteValue = forwardFriction.asymptoteForce;
        forward.stiffness = forwardFriction.stiffness;
        wheel.forwardFriction = forward;
        
        WheelFrictionCurve sideways = wheel.sidewaysFriction;
        sideways.extremumSlip = sidewaysFriction.extrenumSlip;
        sideways.extremumValue = sidewaysFriction.extrenumForce;
        sideways.asymptoteSlip = sidewaysFriction.asymptoteSlip;
        sideways.asymptoteValue = sidewaysFriction.asymptoteForce;
        sideways.stiffness = sidewaysFriction.stiffness;
        wheel.sidewaysFriction = sideways;
    }
}

[System.Serializable]
public class Spring
{
    public float spring = 35000;
    public float damper = 4500;
    public float targetPosition = 0.5f;
}

[System.Serializable]
public class ForwardFriction
{
    public float extrenumSlip = 1f;
    public float extrenumForce = 1f;
    public float asymptoteSlip = 2;
    public float asymptoteForce = 0.5f;
    public float stiffness = 5;
}

[System.Serializable]
public class SidewaysFriction
{
    public float extrenumSlip = 0.2f;
    public float extrenumForce = 1f;
    public float asymptoteSlip = 0.5f;
    public float asymptoteForce = 0.75f;
    public float stiffness = 1;
}