using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WheelDebuggerView : View
{
    public GameObject WheelPlayer;
    private WheelCollider _lWheel;
    private WheelCollider _rWheel;
    [SerializeField] private TextMeshProUGUI lText;
    [SerializeField] private TextMeshProUGUI rText;

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        lText.text = "Left RPM:\nLeft Pose:\nLeft Motor:\nLeft Brake:\nForward Slip:\nSide Slip:";
        rText.text = "Right RPM:\nRight Pose:\nRight Motor:\nRight Brake:\nForward Slip:\nSide Slip:";
        
        Transform wheelObj = WheelPlayer.transform.GetChild(0);
        GameObject lWheelObj = wheelObj.GetChild(0).gameObject;
        GameObject rWheelObj = wheelObj.GetChild(1).gameObject;

        _lWheel = lWheelObj.GetComponent<WheelCollider>();
        _rWheel = rWheelObj.GetComponent<WheelCollider>();
    }

    public override void Initialize() { Show(); }

    private void FixedUpdate() { RenderText(); }
    
    public override void Hide() { }

    public override void Show() { gameObject.SetActive(true); }
    
    public void RenderText()
    {
        float lRpm = _lWheel.rpm;
        float lMotor = _lWheel.motorTorque;
        float lBrake = _lWheel.brakeTorque;
        Vector3 lPos;
        Quaternion lRot;
        _lWheel.GetWorldPose(out lPos, out lRot);
        WheelHit lHit;
        _rWheel.GetGroundHit(out lHit);
        float lFSlip = lHit.forwardSlip;
        float lSSlip = lHit.sidewaysSlip;
        
        float rRpm = _rWheel.rpm;
        float rMotor = _rWheel.motorTorque;
        float rBrake = _rWheel.brakeTorque;
        Vector3 rPos;
        Quaternion rRot;
        _rWheel.GetWorldPose(out rPos, out rRot);
        WheelHit rHit;
        _rWheel.GetGroundHit(out rHit);
        float rFSlip = rHit.forwardSlip;
        float rSSlip = rHit.sidewaysSlip;

        lText.text = $"Left RPM: {lRpm}\nLeft Pose: {lPos.ToString()} {lRot.ToString()}\nLeft Motor: {lMotor}\nLeft Brake: {lBrake}\nForward Slip: {lFSlip}\nSide Slip: {lSSlip}";
        rText.text = $"Right RPM: {rRpm}\nRight Pose: {rPos.ToString()} {rRot.ToString()}\nRight Motor: {rMotor}\nRight Brake: {rBrake}\nForward Slip: {rFSlip}\nSide Slip: {rSSlip}";
    }
}