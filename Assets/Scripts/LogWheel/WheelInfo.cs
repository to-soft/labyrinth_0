using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelInfo : MonoBehaviour
{
    public WheelName name;
    private WheelCollider _wheel;
    private Rigidbody _wheelBody;
    private float _rPi;
    private float _r;

    void Start()
    {
        // get components
        _wheel = gameObject.GetComponent<WheelCollider>();
        _wheelBody = gameObject.GetComponent<Rigidbody>();
        
        // get dimensions
        _r = _wheel.radius;
        _rPi = _r * Mathf.PI;
    }
}
