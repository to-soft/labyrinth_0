using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class VictoryMenuView : View
{
    [SerializeField] private Button _startOverButton;
    public override void Initialize()
    {
        _startOverButton.onClick.AddListener(() =>
        {
            LabyrinthState.rows++;
            LabyrinthState.columns++;
            LabyrinthState.stories++;
            Scene scene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(scene.name);
        });
    }
}