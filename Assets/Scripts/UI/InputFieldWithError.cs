using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InputFieldWithError: MonoBehaviour
{
    [SerializeField] public TMP_InputField InputField;
    public bool IsInErrorState {  get; private set; }

    [SerializeField] private Image _backgroundImage;

    [SerializeField] public Sprite _normalBackgroundImage;
    [SerializeField] private Sprite _errorBackgroundImage;

    [SerializeField] private TMP_Text _errorText;

    private void Start()
    {
        NormalState();
        IsInErrorState = false;
    }
    public void SetState(bool isError)
    {
        if (isError)
        {
            if (IsInErrorState) return;
            ErrorState();
        }
        else if (IsInErrorState)
        {
            NormalState();
        }
    }

    public void SetState(bool isError, string errorMessage)
    {
        if (isError)
        {
            if (IsInErrorState) return;
            ErrorState(errorMessage);
        }
        else if(IsInErrorState)
        {
            NormalState();
        }
    }

    private void ErrorState()
    {
        _backgroundImage.sprite = _errorBackgroundImage;
        if (_errorText != null)
        {
            _errorText.enabled = true;
        }
        IsInErrorState = true;
    }
    private void ErrorState(string errorMessage)
    {
        ErrorState();
        if (errorMessage != null && _errorText.text != errorMessage)
        {
            _errorText.text = errorMessage;
        }
    }

    private void NormalState()
    {
        _backgroundImage.sprite = _normalBackgroundImage;
        if (_errorText != false)
        {
            _errorText.enabled = false;
        }
        IsInErrorState = false;
    }
}
