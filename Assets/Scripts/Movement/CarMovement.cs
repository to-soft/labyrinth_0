using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarMovement : MonoBehaviour
{
    public WheelCollider lwheel;
    public WheelCollider rwheel;
    public float maxMotorTorque;
    public float maxBrakeTorque;
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
        // Debug.Log(position);
        collider.GetWorldPose(out position, out rotation);
        wheel.position = position;
        wheel.rotation = rotation;
    }
    
    // Update is called once per frame
    void FixedUpdate()
    {
        float brake = maxBrakeTorque * Input.GetAxis("Brake");
        float lmotor = maxMotorTorque * Input.GetAxis("Left");
        float rmotor = maxMotorTorque * Input.GetAxis("Right");
        float lbrake = maxBrakeTorque * Input.GetAxis("Left Brake");
        float rbrake = maxBrakeTorque * Input.GetAxis("Right Brake");
        
        // float steering = maxSteeringAngle * Input.GetAxis("Horizontal");
        //
        // lwheel.steerAngle = steering;
        // rwheel.steerAngle = steering;
        lwheel.motorTorque = lmotor;
        rwheel.motorTorque = rmotor;

        lwheel.brakeTorque = brake;
        rwheel.brakeTorque = brake;

        if (lbrake > 0)
            lwheel.brakeTorque = lbrake;
        if (rbrake > 0)
            rwheel.brakeTorque = rbrake;
        
        applyPositionToWheel(lwheel);
        applyPositionToWheel(rwheel);
        
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

    public void applyToWheel(WheelCollider wheel)
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