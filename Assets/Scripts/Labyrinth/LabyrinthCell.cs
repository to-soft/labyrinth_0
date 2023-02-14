using UnityEngine;
using System.Collections;

public enum Direction
{
    Base,
    Start,
    Right,
    Front,
    Left,
    Back,
    Down,
    Up,
}

public class LabyrinthCell
{
    public bool IsVisited = false;
    public bool IsGoal = false;
    public bool RampRight = false;
    public bool RampLeft = false;
    public bool RampFront = false;
    public bool RampBack = false;
    public bool WallRight = false;
    public bool WallFront = false;
    public bool WallLeft = false;
    public bool WallBack = false;
    public bool Ceiling = false;
    public bool Floor = false;
}