using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class LoginUiFlow : MonoBehaviour
{
    public UnityAction<string, string> onLoginWithEmailDataReady;
    public UnityAction<string, string> onLoginWithUsernameDataReady;


    [SerializeField] private TMP_InputField _emailOrUsernameInput;
    [SerializeField] private TMP_InputField _passwordInput;
    [SerializeField] private Button _loginButton;

    private void Start()
    {
        _loginButton.onClick.AddListener(TryLogin);
    }

    private void OnDestroy()
    {
        _loginButton?.onClick.RemoveListener(TryLogin);
    }

    private void TryLogin()
    {
        string password = _passwordInput.text.Trim();
        string emailOrUsername = _emailOrUsernameInput.text.Trim();

        if (UserDataValidation.IsValidEmail(ref emailOrUsername))
        {
            onLoginWithEmailDataReady?.Invoke(emailOrUsername, password);
        }
        else if (UserDataValidation.IsValidUsername(ref emailOrUsername))
        {
            onLoginWithUsernameDataReady?.Invoke(emailOrUsername, password);
        }
    }

    public void Close()
    {
        ResetState();
        gameObject.SetActive(false);
    }

    private void ResetState()
    {
        _emailOrUsernameInput.text = string.Empty;
        _passwordInput.text = string.Empty;
    }
}
