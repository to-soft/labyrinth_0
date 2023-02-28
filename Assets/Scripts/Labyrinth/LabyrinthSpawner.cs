using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class LabyrinthSpawner : MonoBehaviour
{
    public enum LabyrinthGenerationAlgorithm
    {
        PureRecursive
    }

    public LabyrinthGenerationAlgorithm algorithm = LabyrinthGenerationAlgorithm.PureRecursive;
    public bool fullRandom = false;
    public int randomSeed = 12345;
    public GameObject floor = null;
    public Wall wall = null;
    public GameObject ceiling = null;
    public GameObject rampRight = null;
    public GameObject rampLeft = null;
    public GameObject rampFront = null;
    public GameObject rampBack = null;
    public GameObject Door = null;
    public int rows = 4;
    public int columns = 4;
    public int stories = 3;
    public float cellWidth;
    public float cellHeight;
    public float cellDepth;
    public int entranceColumn;
    public GameObject goalPrefab = null;
    private bool GameplayMode = true;

    private LabyrinthContainer mLabyrinthContainer = null;

    // called before first frame update
    private void Start()
    {
        if (GameplayMode)
        {
            if (LabyrinthState.rows == 0 || LabyrinthState.columns == 0 || LabyrinthState.stories == 0)
            {
                LabyrinthState.rows = 4;
                LabyrinthState.columns = 4;
                LabyrinthState.stories = 3;
            }

            rows = LabyrinthState.rows;
            columns = LabyrinthState.columns;
            stories = LabyrinthState.stories;
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
                    GameObject tmp;
                    Vector3 _v = new Vector3();
                    Quaternion _q = Quaternion.Euler(0, 0, 0);

                    if (cell.Ceiling)
                    {
                        tmp = Instantiate(ceiling, new Vector3(x, y, z) + ceiling.transform.position, 
                            Quaternion.Euler(0, 0, 0)) as GameObject;
                        tmp.transform.parent = transform;
                    }
                    if (cell.Floor)
                    {
                        tmp = Instantiate(floor, new Vector3(x, y, z) + floor.transform.position, 
                            Quaternion.Euler(0, 0, 0)) as GameObject;
                        tmp.transform.parent = transform;
                    }
                    if (cell.RampFront)
                    {
                        tmp = Instantiate(rampFront, new Vector3(x, y, z) + rampFront.transform.position, 
                            Quaternion.Euler(315, 0, 0)) as GameObject;
                        tmp.transform.parent = transform;
                        noTorches = true;
                    }
                    if (cell.RampBack)
                    {
                        tmp = Instantiate(rampBack, new Vector3(x, y, z) + rampBack.transform.position, 
                            Quaternion.Euler(45, 0, 0)) as GameObject;
                        tmp.transform.parent = transform;
                        noTorches = true;
                    }
                    if (cell.RampRight)
                    {
                        tmp = Instantiate(rampRight, new Vector3(x, y, z) + rampRight.transform.position, 
                            Quaternion.Euler(0, 0, 45)) as GameObject;
                        tmp.transform.parent = transform;
                        noTorches = true;
                    }
                    if (cell.RampLeft)
                    {
                        tmp = Instantiate(rampLeft, new Vector3(x, y, z) + rampLeft.transform.position, 
                            Quaternion.Euler(0, 0, 315)) as GameObject;
                        tmp.transform.parent = transform;
                        noTorches = true;
                    }
                    if (cell.WallRight)
                    {
                        w = Instantiate(wall, _v, _q) as Wall;
                        w.InitializeWall(Orientation.Right, x, y, z, noTorches);
                        w.transform.parent = transform;
                    }
                    if (cell.WallFront)
                    {
                        w = Instantiate(wall, _v, _q) as Wall;
                        w.InitializeWall(Orientation.Front, x, y, z, noTorches);
                        w.transform.parent = transform;
                    }
                    if (cell.WallLeft)
                    {
                        w = Instantiate(wall, _v, _q) as Wall;
                        w.InitializeWall(Orientation.Left, x, y, z, noTorches);
                        w.transform.parent = transform;
                    }
                    if (cell.WallBack)
                    {
                        w = Instantiate(wall, _v, _q) as Wall;
                        w.InitializeWall(Orientation.Back, x, y, z, noTorches);
                        w.transform.parent = transform;
                    }

                    if (cell.Door)
                    {
                        tmp = Instantiate(Door, new Vector3(x, y, z) + Door.transform.position,
                            Quaternion.Euler(0, 0, 0));
                        tmp.transform.parent = transform;
                    }

                    if (cell.IsGoal)
                    {
                        tmp = Instantiate(goalPrefab, new Vector3(x, y, z) + goalPrefab.transform.position, 
                            Quaternion.Euler(0, 0, 0)) as GameObject;
                        tmp.transform.parent = transform;
                    }
                }
            }
        }
    }
}
