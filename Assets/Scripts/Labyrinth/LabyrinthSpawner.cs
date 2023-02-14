using System;
using System.Collections;
using System.Collections.Generic;
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
    public GameObject wall = null;
    public GameObject ceiling = null;
    public GameObject rampRight = null;
    public GameObject rampLeft = null;
    public GameObject rampFront = null;
    public GameObject rampBack = null;
    public int rows = 5;
    public int columns = 5;
    public int stories = 5;
    public float cellWidth = 3;
    public float cellHeight = 3;
    public float cellDepth = 3;
    public bool entrance = true;
    public GameObject goalPrefab = null;

    private LabyrinthContainer mLabyrinthContainer = null;

    // called before first frame update
    private void Start()
    {
        if (!fullRandom)
        {
            Random.InitState(randomSeed);
        }

        switch (algorithm)
        {
            case LabyrinthGenerationAlgorithm.PureRecursive:
                mLabyrinthContainer = new LabyrinthGenerator(rows, columns, stories);
                break;
        }
        
        mLabyrinthContainer.GenerateLabyrinth();
        
        if (entrance)
        {
            mLabyrinthContainer.GetLabyrinthCell(0, 0, 0).WallBack = false;    
        }
        

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
                    Debug.Log("cell location: x" + column + ", y" + story + ", z" + row);
                    Debug.Log("Cell: \nr:" + cell.WallRight + " \nl:" + cell.WallLeft + 
                              " \nf:" + cell.WallFront + " \nb:" + cell.WallBack + " \nc:" + cell.Ceiling + 
                              " \nvisited: " + cell.IsVisited);
                    GameObject tmp;

                    if (cell.WallRight)
                    {
                        tmp = Instantiate(wall, new Vector3(x + cellWidth / 2, y, z) + wall.transform.position, 
                            Quaternion.Euler(0, 90, 0)) as GameObject;
                        tmp.transform.parent = transform;
                    }
                    if (cell.WallFront)
                    {
                        tmp = Instantiate(wall, new Vector3(x, y, z + cellDepth / 2) + wall.transform.position, 
                            Quaternion.Euler(0, 0, 0)) as GameObject;
                        tmp.transform.parent = transform;
                    }
                    if (cell.WallLeft)
                    {
                        tmp = Instantiate(wall, new Vector3(x - cellWidth / 2, y, z) + wall.transform.position, 
                            Quaternion.Euler(0, 270, 0)) as GameObject;
                        tmp.transform.parent = transform;
                    }
                    if (cell.WallBack)
                    {
                        tmp = Instantiate(wall, new Vector3(x, y, z - cellDepth / 2) + wall.transform.position, 
                            Quaternion.Euler(0, 180, 0)) as GameObject;
                        tmp.transform.parent = transform;
                    }
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
                    }
                    if (cell.RampBack)
                    {
                        tmp = Instantiate(rampBack, new Vector3(x, y, z) + rampBack.transform.position, 
                            Quaternion.Euler(45, 0, 0)) as GameObject;
                        tmp.transform.parent = transform;
                    }
                    if (cell.RampRight)
                    {
                        tmp = Instantiate(rampRight, new Vector3(x, y, z) + rampRight.transform.position, 
                            Quaternion.Euler(0, 0, 45)) as GameObject;
                        tmp.transform.parent = transform;
                    }
                    if (cell.RampLeft)
                    {
                        tmp = Instantiate(rampLeft, new Vector3(x, y, z) + rampLeft.transform.position, 
                            Quaternion.Euler(0, 0, 315)) as GameObject;
                        tmp.transform.parent = transform;
                    }
                    
//                    if (cell.IsGoal && goalPrefab != null)
//                    {
//                        tmp = Instantiate(goalPrefab, new Vector3(x, 1, z), Quaternion.Euler(0,0,0)) as GameObject;
//                        tmp.transform.parent = transform;
//                    }
                }
            }
        }
    }
}
