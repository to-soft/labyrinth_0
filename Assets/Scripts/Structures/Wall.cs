using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Orientation 
{
    Front,
    Back,
    Left,
    Right
}

public class Wall : MonoBehaviour
{
    public GameObject torch;
    public GameObject wall;
    public bool hasTorch = false;
    public int torchDensity = 1;

    public void InitializeWall(Orientation o, float x, float y, float z, bool noTorches)
    {
        System.Random gen = new System.Random();
        if (gen.Next(100) < torchDensity)
        {
            hasTorch = true;
        }
        torch.SetActive(!noTorches && hasTorch);

        switch (o)
        {
            case Orientation.Front:
                transform.position += new Vector3(x, y, z + 1.5f);
                transform.rotation = Quaternion.Euler(0, 0, 0);
                break;
            case Orientation.Back:
                transform.position += new Vector3(x, y, z - 1.5f);
                transform.rotation = Quaternion.Euler(0, 180, 0);
                break;
            case Orientation.Right:
                transform.position += new Vector3(x + 1.5f, y, z);
                transform.rotation = Quaternion.Euler(0, 90, 0);
                break;
            case Orientation.Left:
                transform.position += new Vector3(x - 1.5f, y, z);
                transform.rotation = Quaternion.Euler(0, 270, 0);
                break;
        }
    }
}
