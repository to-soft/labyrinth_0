using System;
using Unity.VisualScripting;
using UnityEngine;

public class Goal : MonoBehaviour
{
    [SerializeField] private View _victoryMenuView;

    private void Start()
    {
        _victoryMenuView = ViewManager.GetView<VictoryMenuView>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PlayerBody"))
        {
            Debug.Log("Player reached goal prefab...");
            ViewManager.Show(_victoryMenuView, false);
        }
    }
}