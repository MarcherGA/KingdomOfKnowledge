using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UserSignUiFlow : MonoBehaviour
{
    public UnityEvent<string, string, UserData> onRegisterTry;
    public UnityEvent<string, string> onLoginWithEmailTry;
    public UnityEvent<string, string> onLoginWithUsernameTry;


    [SerializeField] private LandingUi _landingUi;
    [SerializeField] private LoginUiFlow _loginUi;
    [SerializeField] private RegisterUiFlow _registerUi;

    [SerializeField] private Button _backButton;

    private void Start()
    {
        _landingUi.onOpenLoginClicked += OpenLoginPanel;
        _landingUi.onOpenRegisterClicked += OpenRegisterPanel;

        _backButton.onClick.AddListener(OpenLandingPanel);

        _loginUi.onLoginWithEmailDataReady += onLoginWithEmailDataReady;
        _loginUi.onLoginWithUsernameDataReady += onLoginWithUsernameDataReady;
        _registerUi.onRegisterDataReady += onRegisterDataReady;

        _loginUi.Close();
        _registerUi.Close();

        _landingUi.gameObject.SetActive(true);

        _backButton.gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        _landingUi.onOpenLoginClicked -= OpenLoginPanel;
        _landingUi.onOpenRegisterClicked -= OpenRegisterPanel;

        _loginUi.onLoginWithEmailDataReady -= onLoginWithEmailDataReady;
        _registerUi.onRegisterDataReady -= onRegisterDataReady;

        _backButton?.onClick?.RemoveListener(OpenLandingPanel);
    }

    public void UserSignedIn()
    {
        _landingUi.Close();
        _registerUi.Close();
        _loginUi.Close();

        _backButton.gameObject.SetActive(false);

        SceneController.Instance.LoadScene("MainMenuScene");
    }

    public void GameLoaded()
    {
        //TODO add check if already sign in

        _landingUi.EnableButtons();
    }

    public void LoginFailed()
    {
        string errorMessage = "������/�� ����� �/�� �����\n�� ������";
        ErrorPopup.Instance.ShowError(ref errorMessage);
    }

    public void UsernameCheckCompleted(bool isAvailable)
    {
        if (!isAvailable)
        {
            _registerUi.ShowUsernameOccupiedError();
        }

    }

    private void onLoginWithEmailDataReady(string arg0, string arg1)
    {
        onLoginWithEmailTry?.Invoke(arg0, arg1);
    }


    private void onLoginWithUsernameDataReady(string arg0, string arg1)
    {
        onLoginWithUsernameTry?.Invoke(arg0, arg1);
    }

    private void onRegisterDataReady(string arg0, string arg1, UserData arg2)
    {
        onRegisterTry?.Invoke(arg0, arg1, arg2);
    }


    private void OpenLoginPanel()
    {
        _loginUi.gameObject.SetActive(true);
        _landingUi.Close();
        _backButton.gameObject.SetActive(true);
    }

    private void OpenRegisterPanel()
    {
        _registerUi.gameObject.SetActive(true);
        _landingUi.Close();
        _backButton.gameObject.SetActive(true);
    }

    private void OpenLandingPanel()
    {
        _landingUi.gameObject.SetActive(true);
        _registerUi.Close();
        _loginUi.Close();
        _backButton.gameObject.SetActive(false);
    }

}
