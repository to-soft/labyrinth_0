using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int health = 10;
    public GameObject playerObject;

    public GameObject nearestRespawn;


    private void Awake()
    {
        nearestRespawn = GameObject.FindGameObjectWithTag("RespawnPoint");
    }

    private void Update()
    {
        CheckLife();
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
