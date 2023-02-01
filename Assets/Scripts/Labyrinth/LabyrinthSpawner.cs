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

    public LabyrinthGenerationAlgorithm Algorithm = LabyrinthGenerationAlgorithm.PureRecursive;
    public bool FullRandom = false;
    public int RandomSeed = 12345;
    public GameObject Floor = null;
    public GameObject Wall = null;
    public GameObject Pillar = null;
    public int Rows = 5;
    public int Columns = 5;
    public float CellWidth = 4;
    public float CellHeight = 4;
    public bool AddGaps = false;
    public GameObject GoalPrefab = null;

    private LabyrinthContainer mLabyrinthContainer = null;

    // called before first frame update
    private void Start()
    {
        if (!FullRandom)
        {
            Random.InitState(RandomSeed);
        }

        switch (Algorithm)
        {
            case LabyrinthGenerationAlgorithm.PureRecursive:
                mLabyrinthContainer = new LabyrinthGenerator(Rows, Columns);
                break;
        }
        
        mLabyrinthContainer.GenerateLabyrinth();

        for (int row = 0; row < Rows; row++)
        {
            for (int column = 0; column < Columns; column++)
            {
                float x = column * (CellWidth + (AddGaps ? .2f : 0));
                float z = row * (CellHeight + (AddGaps ? .2f : 0));
                LabyrinthCell cell = mLabyrinthContainer.GetLabyrinthCell(row, column);
                GameObject tmp;
                // tmp = Instantiate(Floor, new Vector3(x, 0, z), Quaternion.Euler(0,0,0)) as GameObject;
                // tmp.transform.parent = transform;

                if (cell.WallRight)
                {
                    tmp = Instantiate(Wall, new Vector3(x + CellWidth / 2, 0, z) + Wall.transform.position, Quaternion.Euler(0, 90, 0)) as GameObject;
                    tmp.transform.parent = transform;
                }
                if (cell.WallFront)
                {
                    tmp = Instantiate(Wall, new Vector3(x, 0, z + CellHeight / 2) + Wall.transform.position, Quaternion.Euler(0, 0, 0)) as GameObject;
                    tmp.transform.parent = transform;
                }
                if (cell.WallLeft)
                {
                    tmp = Instantiate(Wall, new Vector3(x - CellWidth / 2, 0, z) + Wall.transform.position, Quaternion.Euler(0, 270, 0)) as GameObject;
                    tmp.transform.parent = transform;
                }
                if (cell.WallBack)
                {
                    tmp = Instantiate(Wall, new Vector3(x, 0, z - CellHeight / 2) + Wall.transform.position, Quaternion.Euler(0, 180, 0)) as GameObject;
                    tmp.transform.parent = transform;
                }

                if (cell.IsGoal && GoalPrefab != null)
                {
                    tmp = Instantiate(GoalPrefab, new Vector3(x, 1, z), Quaternion.Euler(0,0,0)) as GameObject;
                    tmp.transform.parent = transform;
                }
            }
        }

        if (Pillar != null)
        {
            for (int row = 0; row < Rows + 1; row++)
            {
                for (int column = 0; column < Columns + 1; column++)
                {
                    float x = column * (CellWidth + (AddGaps ? .2f : 0));
                    float z = row * (CellHeight + (AddGaps ? .2f : 0));
                    GameObject tmp = Instantiate(Pillar, new Vector3(x - CellWidth / 2, 0, z - CellHeight / 2), Pillar.transform.rotation) as GameObject;
                    tmp.transform.parent = transform;
                }
            }
        }
        
    }
}
