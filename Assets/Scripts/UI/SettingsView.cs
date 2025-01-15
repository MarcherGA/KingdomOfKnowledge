using System;
using UnityEngine;
using UnityEngine.UI;

public class SettingsView : MonoBehaviour
{
    enum SettingsViewType {General, Account, About}
    [SerializeField] private GameObject _generalSettingsPanel;

    [SerializeField] private GameObject _accountSettingsPanel;
    [SerializeField] private GameObject _aboutPanel;

    [SerializeField] private Button _aboutButton;
    [SerializeField] private Button _generalSettingsButton;
    [SerializeField] private Button _accountSettingsButton;

    private SettingsViewType _activeView = SettingsViewType.General;

    private bool _initialized = false;

    void Start()
    {
        _aboutButton.onClick.AddListener(() => ShowView(SettingsViewType.About));
        _generalSettingsButton.onClick.AddListener(() => ShowView(SettingsViewType.General));
        _accountSettingsButton.onClick.AddListener(() => ShowView(SettingsViewType.Account));

        ShowView(SettingsViewType.General);

        _initialized = true;
    }

    void OnDisable()
    {
        ShowView(SettingsViewType.General);
    }


    private void CloseAllViews ()
    {
        _generalSettingsPanel.SetActive(false);
        _accountSettingsPanel.SetActive(false);
        _aboutPanel.SetActive(false);
    }

    private void ActivateAllButtons ()
    {
        _aboutButton.interactable = true;
        _generalSettingsButton.interactable = true;
        _accountSettingsButton.interactable = true;
    }

    private void ShowView (SettingsViewType settingsViewTypeToShow)
    {
        if(_activeView == settingsViewTypeToShow && _initialized) return;
        _activeView = settingsViewTypeToShow;

        CloseAllViews();
        ActivateAllButtons();

        switch (settingsViewTypeToShow)
        {
            case SettingsViewType.General:
                _generalSettingsPanel.SetActive(true);
                _generalSettingsButton.interactable = false;
                break;
            case SettingsViewType.Account:
                _accountSettingsPanel.SetActive(true);
                _accountSettingsButton.interactable = false;
                break;
            case SettingsViewType.About:
                _aboutPanel.SetActive(true);
                _aboutButton.interactable = false;
                break;
        }
    }


    
}