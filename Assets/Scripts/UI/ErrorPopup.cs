using UnityEngine;
using TMPro;
using UnityEngine.UI;

[RequireComponent(typeof(Canvas))]
public class ErrorPopup : MonoBehaviour
{
    public static ErrorPopup Instance { get; private set; }

    [SerializeField] private GameObject _errorPopup;
    [SerializeField] private TMP_Text _errorMessageText;
    [SerializeField] private Button _closeButton;

    private void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject); // Optional if you want to persist across scenes
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        _closeButton.onClick.AddListener(HideError);
        _errorPopup.SetActive(false);
    }

    private void OnDestroy()
    {
        _closeButton?.onClick.RemoveListener(HideError);
    }

    public void ShowError(ref string message, float duration = 0)
    {
        _errorMessageText.text = message;
        _errorPopup.SetActive(true);

        if (duration > 0)
        {
            Invoke(nameof(HideError), duration);
        }
    }

    public void HideError()
    {
        _errorPopup.SetActive(false);
    }
}
