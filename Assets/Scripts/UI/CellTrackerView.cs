using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;

public class CellTrackerView : View
{
    // private int row;
    // private int column;
    // private int story;
    private double _x;
    private double _y;
    private double _z;
    private Transform _playerTransform;
    [SerializeField] private TextMeshProUGUI text;
    
    public override void Hide() { }
    
    public override void Show()
    {
        gameObject.SetActive(true);
    }
    
    public override void Initialize()
    {
        Debug.Log("Cell Tracker View searching for player object...");
        var player = GameObject.FindGameObjectWithTag("PlayerBody");
        if (!player)
        {
            Debug.Log("failed to find player");
            return;
        }

        _playerTransform = player.transform;
        Show();
    }

    public void CalculateCell()
    {
        var p = _playerTransform.position;
        _x = Math.Round(p.x, 2);
        _y = Math.Round(p.y, 2);
        _z = Math.Round(p.z, 2);
    }

    public void RenderText()
    {
        text.text = $"x: {_x}\ny: {_y}\nz: {_z}";
    }

    private void Update()
    {
        CalculateCell();
        RenderText();
    }
}
