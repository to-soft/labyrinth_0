using System;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    public static int DistanceFromGoal;
    
    private void Awake()
    {
        nearestRespawn = GameObject.FindGameObjectWithTag("RespawnPoint");
        _x = _y = _z = _row = _story = _column = -1;
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void Update()
    {
        CheckLife();
        CalculateCell();
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        _x = _y = _z = _story = _row = _column = 0;
        currentCell = null;
        DistanceFromGoal = Int32.MaxValue;
    }

    public void CalculateCell()
    {
        var p = playerObject.transform.position;
        _x = Math.Floor(p.x);
        _y = Math.Floor(p.y);
        _z = Math.Floor(p.z);
        int c = (int)Math.Floor(_x / 6);
        int s = (int)Math.Floor(_y / 6);
        int r = (int)Math.Floor(_z / 6);
        
        // Debug.Log($"col {c} row {r} story {s}");
        if (c != _column || s != _story || r != _row)
        {
            _column = c;
            _row = r;
            _story = s;
            int previousDistanceFromGoal;
            if (_z < 0 || _row + 1 > LabyrinthState.rows
                         || _x < 0 || _column + 1 > LabyrinthState.columns
                         || _y < 0 || _story + 1 > LabyrinthState.stories)
            {
                currentCell = null;
                return;
            }
            if (currentCell is not null)
            {
                previousDistanceFromGoal = currentCell.DistanceFromGoal;
            }
            else
            {
                DistanceFromGoal = previousDistanceFromGoal = Int32.MaxValue;
                
            }
            currentCell = LabyrinthState.GetLabyrinthCellMap(_row, _column, _story);
            DistanceFromGoal = currentCell.DistanceFromGoal;
            if (DistanceFromGoal < previousDistanceFromGoal)
            {
                Warmer = true;
            }
            else
            {
                Warmer = false;
            }
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
