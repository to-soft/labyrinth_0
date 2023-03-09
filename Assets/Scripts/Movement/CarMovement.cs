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
    public float Mass;
    public float DampingRate;
    public float SuspensionDistance;
    public JointSpring SuspensionSpring;
    public WheelFrictionCurve ForwardFriction;
    public WheelFrictionCurve SidewaysFriction;

    public void applyToWheel(WheelCollider wheel)
    {
        wheel.mass = Mass / 2;
        wheel.wheelDampingRate = DampingRate;
        wheel.suspensionDistance = SuspensionDistance;
        wheel.suspensionSpring = SuspensionSpring;
        wheel.forwardFriction = ForwardFriction;
        wheel.sidewaysFriction = SidewaysFriction;
    }
}
