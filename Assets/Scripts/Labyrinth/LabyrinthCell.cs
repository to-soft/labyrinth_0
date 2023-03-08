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
    public int Row;
    public int Column;
    public int Story;
    
    public bool Door = false;
    public bool Floor = false;
    public bool IsGoal = false;
    public bool Ceiling = false;
    public bool RampBack = false;
    public bool RampLeft = false;
    public bool WallLeft = false;
    public bool WallBack = false;
    public bool RampRight = false;
    public bool RampFront = false;
    public bool WallRight = false;
    public bool WallFront = false;
    public bool IsVisited = false;
    public int DistanceFromGoal;

    public LabyrinthCell(int row, int column, int story)
    {
        Row = row;
        Column = column;
        Story = story;
    }
}