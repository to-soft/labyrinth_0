using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    public GameObject torch;
    public GameObject wall;
    public bool hasTorch = false;
    public int torchDensity = 1;

    public void InitializeWall(Orientation o, bool noTorches)
    {
        System.Random gen = new System.Random();
        if (gen.Next(100) < torchDensity)
        {
            hasTorch = true;
        }
        torch.SetActive(!noTorches && hasTorch);
     
        Debug.Log($"WALL: Orientation: {o}");
        Debug.Log($"WALL: Initial position: {transform.position}");
        
        switch (o)
        {
            case Orientation.Front:
                transform.position += new Vector3(0, 0, 3f);
                transform.rotation = Quaternion.Euler(0, 0, 0);
                break;
            case Orientation.Back:
                transform.position += new Vector3(0, 0, -3f);
                transform.rotation = Quaternion.Euler(0, 180, 0);
                break;
            case Orientation.Right:
                transform.position += new Vector3(3f, 0, 0);
                transform.rotation = Quaternion.Euler(0, 90, 0);
                break;
            case Orientation.Left:
                transform.position += new Vector3(-3f, 0, 0);
                transform.rotation = Quaternion.Euler(0, 270, 0);
                break;
        }
        Debug.Log($"WALL: Final position: {transform.position}");
    }
}
