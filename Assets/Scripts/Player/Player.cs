using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int health = 10;
    public GameObject playerObject;
    public GameObject nearestRespawn;
    public static LabyrinthCell currentCell;
    private double _x;
    private double _y;
    private double _z;
    private int _row;
    private int _story;
    private int _column;
    public static bool Warmer = false;
    
    private void Awake()
    {
        nearestRespawn = GameObject.FindGameObjectWithTag("RespawnPoint");
        Debug.Log("HIIIIIII");
    }

    private void Update()
    {
        CheckLife();
        CalculateCell();
    }

    public void CalculateCell()
    {
        var p = transform.position;
        if (p.x <= 0 || p.y <= 0 || p.z <= 0)  return;
        _x = Math.Floor(p.x);
        _y = Math.Floor(p.y);
        _z = Math.Floor(p.z);
        int c = (int)Math.Floor(_x / 6);
        int s = (int)Math.Floor(_y / 6);
        int r = (int)Math.Floor(_z / 6);
        
        if (c != _column || s != _story || r != _row)
        {
            _column = c;
            _row = r;
            _story = s;
            int previousDistanceFromGoal;
            if (_row < 0 || _row + 1 > LabyrinthState.rows
                         || _column < 0 || _column + 1 > LabyrinthState.columns
                         || _story < 0 || _story + 1 > LabyrinthState.stories) return;
            if (currentCell is not null)
            {
                previousDistanceFromGoal = currentCell.DistanceFromGoal;
            }
            else
            {
                previousDistanceFromGoal = Int32.MaxValue;
            }
            currentCell = LabyrinthState.GetLabyrinthCellMap(_row, _column, _story);
            if (currentCell.DistanceFromGoal < previousDistanceFromGoal)
            {
                Warmer = true;
            }
            else
            {
                Warmer = false;
            }
            print($"current cell: {currentCell}");
        }
    }


    public void Damage(int dmg)
    {
        Debug.Log("Player took " + dmg + " damage");
        health -= 5;
    }

    public void Die()
    {
        health = 10;
        playerObject.transform.position = nearestRespawn.transform.position;
        playerObject.transform.rotation = Quaternion.Euler(0,0,0);
    }

    public void CheckLife()
    {
        if (health <= 0)
        {
            Die();
        }
    }
}
