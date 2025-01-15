using UnityEngine;
using UnityEngine.UI;

public class MainMenuView : MonoBehaviour
{
    enum MainMenuViewState
    {
        Main,
        Settings
    }

    [SerializeField] private GameObject _mainMenuPanel;
    [SerializeField] private GameObject _settingsPanel;

    [SerializeField] private Button _playButton;
    [SerializeField] private Button _settingsButton;
    [SerializeField] private Button _backButton;

    [SerializeField] private GameObject _playerView;

    private MainMenuViewState _mainMenuViewState = MainMenuViewState.Main;

    private bool _initialized = false;
    
    private void Start()
    {
        _backButton.onClick.AddListener(() => ShowView(MainMenuViewState.Main));
        _settingsButton.onClick.AddListener(() => ShowView(MainMenuViewState.Settings));
        _playButton.onClick.AddListener(() => SceneController.Instance.LoadScene("ForestWorld_Level1"));  //TODO implement play for current level

        ShowView(MainMenuViewState.Main);
        _initialized = true;
    }

    private void CloseAllViews()
    {
        _mainMenuPanel.SetActive(false);
        _playerView.SetActive(false);

        _settingsPanel.SetActive(false);
    }

    private void ShowView(MainMenuViewState mainMenuViewStateToShow)
    {
        if (_mainMenuViewState == mainMenuViewStateToShow && _initialized) return;
        _mainMenuViewState = mainMenuViewStateToShow;
        
        CloseAllViews();

        switch (mainMenuViewStateToShow)
        {
            case MainMenuViewState.Main:
                _mainMenuPanel.SetActive(true);
                _playerView.SetActive(true);
                _backButton.gameObject.SetActive(false);
                break;
            case MainMenuViewState.Settings:
                _settingsPanel.SetActive(true);
                _backButton.gameObject.SetActive(true);
                break;
        }
    }
}
