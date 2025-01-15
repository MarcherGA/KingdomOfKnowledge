using UnityEngine;
using TMPro; // If you're using TextMeshPro InputField; otherwise, use UnityEngine.UI for standard InputField

[RequireComponent(typeof(TMP_InputField))]
public class InputFieldRtlChecker : MonoBehaviour
{
    private TMP_InputField _inputField; // Replace with InputField if not using TMP
    [SerializeField] private bool _changeAligment;
    [SerializeField] private TextAlignmentOptions _rtlAlignment = TextAlignmentOptions.Right;
    [SerializeField] private TextAlignmentOptions _ltrAlignment = TextAlignmentOptions.Left;

    private void Start()
    {
        _inputField = GetComponent<TMP_InputField>();
        if (_inputField != null)
        {
            _inputField.onValueChanged.AddListener(CheckAndApplyRtl);
        }
    }

    private void OnDestroy()
    {
        if (_inputField != null)
        {
            _inputField.onValueChanged.RemoveListener(CheckAndApplyRtl);
        }
    }

    private void CheckAndApplyRtl(string inputText)
    {
        if (ContainsRtlCharacters(inputText))
        {
            _inputField.textComponent.isRightToLeftText = true;

            if (_changeAligment)
            {
                _inputField.textComponent.alignment = _rtlAlignment;
            }
        }
        else
        {
            _inputField.textComponent.isRightToLeftText = false;

            if (_changeAligment)
            {
                _inputField.textComponent.alignment = _ltrAlignment;
            }
        }
    }

    private bool ContainsRtlCharacters(string inputText)
    {
        foreach (char c in inputText)
        {
            if (IsRtlCharacter(c))
            {
                return true;
            }
        }
        return false;
    }

    private bool IsRtlCharacter(char c)
    {
        // Unicode ranges for RTL characters (e.g., Arabic, Hebrew)
        return (c >= '\u0590' && c <= '\u05FF') || // Hebrew
               (c >= '\u0600' && c <= '\u06FF') || // Arabic
               (c >= '\u0750' && c <= '\u077F') || // Arabic Supplement
               (c >= '\u08A0' && c <= '\u08FF') || // Arabic Extended-A
               (c >= '\uFB50' && c <= '\uFDFF') || // Arabic Presentation Forms-A
               (c >= '\uFE70' && c <= '\uFEFF');   // Arabic Presentation Forms-B
    }
}
