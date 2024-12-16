using System.Security.Cryptography;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


public class RegisterUiFlow : MonoBehaviour
{
    public UnityAction<string, string, UserData> onRegisterDataReady;

    [SerializeField] private InputFieldWithError _usernameInput;
    [SerializeField] private InputFieldWithError _passwordInput;
    [SerializeField] private InputFieldWithError _emailInput;
    [SerializeField] private TMP_InputField _nameInput;
    [SerializeField] private BirthdaySelector _birthdaySelector;
    [SerializeField] private TMP_Dropdown _genderDropdown;
    [SerializeField] private TMP_Dropdown _classDropdown;

    [SerializeField] private Button _registerButton;

    private void Start()
    {
        ResetState();
        _registerButton.onClick.AddListener(TryRegister);
    }

    private void OnDestroy()
    {
        _registerButton?.onClick.RemoveListener(TryRegister);
    }


    public void Close()
    {
        ResetState();
        gameObject.SetActive(false);
    }
    
    public void ShowUsernameOccupiedError()
    {
        _usernameInput.SetState(true, "שם משתמש תפוס");
    }

    private void ResetState()
    {
        _usernameInput.SetState(false, "שם משתמש קצר");
        _passwordInput.SetState(false);
        _emailInput.SetState(false);

        _usernameInput.InputField.text = "";
        _passwordInput.InputField.text = "";
        _emailInput.InputField.text = "";
        _nameInput.text = "";
        _genderDropdown.value = -1;
        _classDropdown.value = -1;


    }

    private void TryRegister()
    {
        if (ValidateInputs() && ValidateDropdowns())
        {
            string email = _emailInput.InputField.text.Trim();
            string password = _passwordInput.InputField.text.Trim();
            string username = _usernameInput.InputField.text.Trim();
            string fullName = _nameInput.text.Trim();
            string birthday = _birthdaySelector.GetSelectedBirthday();
            string gender = _genderDropdown.options[_genderDropdown.value].text;
            string userClass = _classDropdown.options[_classDropdown.value].text;

            // Create a UserData object
            UserData userData = new UserData(fullName, birthday, gender, userClass, username, email);

            onRegisterDataReady?.Invoke(email, password, userData);
        }
    }

    private bool ValidateDropdowns()
    {
        return _genderDropdown.value != -1 && _classDropdown.value != -1 && _birthdaySelector.IsBirthdaySelected;
    }

    private bool ValidateInputs()
    {
        string username = _usernameInput.InputField.text.Trim();
        string password = _passwordInput.InputField.text.Trim();
        string email = _emailInput.InputField.text.Trim();

        bool areValid = true;

        if (!UserDataValidation.IsValidUsername(ref username))
        {
            _usernameInput.SetState(true);
            areValid = false;
        }
        else
        {
            _usernameInput.SetState(false);
        }

        if (!UserDataValidation.IsValidPassword(ref password))
        {
            _passwordInput.SetState(true);
            areValid = false;
        }
        else
        {
            _passwordInput.SetState(false);
        }

        if (!UserDataValidation.IsValidEmail(ref email))
        {
            _emailInput.SetState(true);
            areValid = false;
        }
        else
        {
            _emailInput.SetState(false);
        }

        return areValid;
    }



}
