using UnityEngine;
using UnityEngine.UI;

public class MainMenuView : View
{

    [SerializeField] private Button _settingsButton;
    [SerializeField] private Button _startButton;
    public override void Initialize()
    {
        _settingsButton.onClick.AddListener(() => ViewManager.Show<SettingsMenuView>());
        _startButton.onClick.AddListener(() =>
        {
            View view = ViewManager.GetView<SettingsMenuView>();
            ViewManager.Hide(view);
        });
    }
}
