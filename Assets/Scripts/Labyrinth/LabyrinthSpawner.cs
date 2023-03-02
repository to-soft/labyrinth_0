using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;
using Random = UnityEngine.Random;

public enum Orientation 
{
    Front,
    Back,
    Left,
    Right
}

public class LabyrinthSpawner : MonoBehaviour
{
    public enum LabyrinthGenerationAlgorithm
    {
        PureRecursive
    }

    public LabyrinthGenerationAlgorithm algorithm = LabyrinthGenerationAlgorithm.PureRecursive;
    public bool fullRandom = false;
    public int randomSeed = 12345;
    public Wall wall = null;
    public Ramp ramp = null;
    public GameObject floor = null;
    public GameObject ceiling = null;
    
    public GameObject rampRight = null;
    public GameObject rampLeft = null;
    public GameObject rampFront = null;
    public GameObject rampBack = null;
    public GameObject door = null;
    public int rows = 1;
    public int columns = 1;
    public int stories = 1;
    public float cellWidth;
    public float cellHeight;
    public float cellDepth;
    public int entranceColumn;
    public GameObject goalPrefab = null;
    
    private Quaternion _qCenter;
    private Vector3 _wallPosition;
    private Vector3 _rampPosition;

    private bool GameplayMode = false;
    private LabyrinthContainer mLabyrinthContainer = null;

    // called before first frame update
    private void Start()
    {
        if (GameplayMode)
        {
            if (LabyrinthState.rows == 0 || LabyrinthState.columns == 0 || LabyrinthState.stories == 0)
            {
                LabyrinthState.rows = 1;
                LabyrinthState.columns = 1;
                LabyrinthState.stories = 1;
            }

            rows = LabyrinthState.rows;
            columns = LabyrinthState.columns;
            stories = LabyrinthState.stories;

            if (rows < 4 || columns < 4) { stories = 1; }
        }
        if (!fullRandom)
        {
            Random.InitState(randomSeed);
        }

        switch (algorithm)
        {
            case LabyrinthGenerationAlgorithm.PureRecursive:
                print($"rows: {rows} columns: {columns} stories: {stories}");
                mLabyrinthContainer = new LabyrinthGenerator(rows, columns, stories);
                break;
        }

        mLabyrinthContainer.GenerateLabyrinth();
        _wallPosition = wall.transform.position;
        _rampPosition = ramp.transform.position;
        _qCenter = Quaternion.Euler(0, 0, 0);
        
        for (int story = 0; story < stories; story++)
        {
            for (int row = 0; row < rows; row++)
            {
                for (int column = 0; column < columns; column++)
                {
                    float x = column * (cellWidth);
                    float y = story * (cellHeight);
                    float z = row * (cellDepth);
                    LabyrinthCell cell = mLabyrinthContainer.GetLabyrinthCell(row, column, story);
                    // Debug.Log("cell location: x" + column + ", y" + story + ", z" + row);
                    // Debug.Log("Cell: \nr:" + cell.WallRight + " \nl:" + cell.WallLeft + 
                    //           " \nf:" + cell.WallFront + " \nb:" + cell.WallBack + " \nc:" + cell.Ceiling + 
                    //           " \nvisited: " + cell.IsVisited);
                    bool noTorches = false;
                    Wall w;
                    Ramp r;
                    GameObject tmp;
                    Vector3 v = new Vector3(x, y, z);
                    Vector3 rampVector = v + _rampPosition;
                    Vector3 wallVector = v + _wallPosition;

                    if (cell.Ceiling)
                    {
                        tmp = Instantiate(ceiling, v + ceiling.transform.position, _qCenter) as GameObject;
                        tmp.transform.parent = transform;
                    }
                    if (cell.Floor)
                    {
                        tmp = Instantiate(floor, v + floor.transform.position, _qCenter) as GameObject;
                        tmp.transform.parent = transform;
                    }
                    if (cell.RampFront)
                    {
                        // tmp = Instantiate(rampFront, v + rampFront.transform.position, 
                        // Quaternion.Euler(315, 0, 0)) as GameObject;
                        // tmp.transform.parent = transform;
                        // noTorches = true;

                        r = Instantiate(ramp, rampVector, _qCenter);
                        r.InitializeRamp(Orientation.Front);
                        r.transform.parent = transform;
                    }
                    if (cell.RampBack)
                    {
                        // tmp = Instantiate(rampBack, v + rampBack.transform.position, 
                            // Quaternion.Euler(45, 0, 0)) as GameObject;
                        // tmp.transform.parent = transform;
                        // noTorches = true;
                        r = Instantiate(ramp, rampVector, _qCenter) as Ramp;
                        r.InitializeRamp(Orientation.Back);
                        r.transform.parent = transform;
                    }
                    if (cell.RampRight)
                    {
                        // tmp = Instantiate(rampRight, v + rampRight.transform.position, 
                            // Quaternion.Euler(0, 0, 45)) as GameObject;
                        // tmp.transform.parent = transform;
                        // noTorches = true;
                        r = Instantiate(ramp, rampVector, _qCenter) as Ramp;
                        r.InitializeRamp(Orientation.Right);
                        r.transform.parent = transform;
                    }
                    if (cell.RampLeft)
                    {
                        // tmp = Instantiate(rampLeft, v + rampLeft.transform.position, 
                            // Quaternion.Euler(0, 0, 315)) as GameObject;
                        // tmp.transform.parent = transform;
                        // noTorches = true;
                        r = Instantiate(ramp, rampVector, _qCenter) as Ramp;
                        r.InitializeRamp(Orientation.Left);
                        r.transform.parent = transform;
                    }
                    if (cell.WallRight)
                    {
                        w = Instantiate(wall, wallVector, _qCenter);
                        w.InitializeWall(Orientation.Right, noTorches);
                        w.transform.parent = transform;
                    }
                    if (cell.WallFront)
                    {
                        w = Instantiate(wall, wallVector, _qCenter);
                        w.InitializeWall(Orientation.Front, noTorches);
                        w.transform.parent = transform;
                    }
                    if (cell.WallLeft)
                    {
                        w = Instantiate(wall, wallVector, _qCenter);
                        w.InitializeWall(Orientation.Left, noTorches);
                        w.transform.parent = transform;
                    }
                    if (cell.WallBack)
                    {
                        w = Instantiate(wall, wallVector, _qCenter);
                        w.InitializeWall(Orientation.Back, noTorches);
                        w.transform.parent = transform;
                    }

                    if (cell.Door)
                    {
                        tmp = Instantiate(door, v + door.transform.position,
                            Quaternion.Euler(0, 0, 0));
                        tmp.transform.parent = transform;
                    }

                    if (cell.IsGoal)
                    {
                        tmp = Instantiate(goalPrefab, v + goalPrefab.transform.position, 
                            Quaternion.Euler(0, 0, 0)) as GameObject;
                        tmp.transform.parent = transform;
                    }
                }
            }
        }
    }
}
