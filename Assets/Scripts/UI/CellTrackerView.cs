using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CellTrackerView : View
{
    private LabyrinthCell _currentCell;
    [SerializeField] private TextMeshProUGUI text;

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        text.text = "You're not in the Labyrinth...";

    }

    public override void Hide() { }
    
    public override void Show() { gameObject.SetActive(true); }
    
    public override void Initialize()
    {
        Show();
    }

    public void RenderText()
    {
        _currentCell = Player.currentCell;
        if (_currentCell is null)
        {
            text.text = "You're not in the labyrinth...";
            return;
        }

        string warmer = Player.Warmer ? "Warmer..." : "Colder...";
        text.text = $"Row: {_currentCell.Row}\nColumn: {_currentCell.Column}\nStory: {_currentCell.Story}" +
                    $"\n{warmer}\nDistance from goal: {Player.DistanceFromGoal}";
    }

    private void Update()
    {
        RenderText();
    }
}
