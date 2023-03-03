using TMPro;
using UnityEngine;

public class CellTrackerView : View
{
    private LabyrinthCell _currentCell;
    [SerializeField] private TextMeshProUGUI text;
    
    public override void Hide() { }
    
    public override void Show()
    {
        gameObject.SetActive(true);
    }
    
    public override void Initialize()
    {
        Debug.Log("Cell Tracker View searching for player object...");
        Show();
    }

    public void RenderText()
    {
        _currentCell = Player.currentCell;
        if (_currentCell is null)
        {
            print("current cell is null");
            text.text = "";
            return;
        }

        string warmer = Player.Warmer ? "Warmer..." : "Colder...";
        text.text = $"Row: {_currentCell.Row}\nColumn: {_currentCell.Column}\nStory: {_currentCell.Story}" +
                    $"\n{warmer}";
    }

    private void Update()
    {
        RenderText();
    }
}
