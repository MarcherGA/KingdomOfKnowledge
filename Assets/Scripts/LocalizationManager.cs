using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class LocalizationManager : MonoBehaviour
{
    private static Dictionary<string, string> _localizedStrings;
    private static string _currentLanguage = "en"; // Default to English

    // Singleton pattern to access LocalizationManager
    public static LocalizationManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Load the localization file based on the language
    public void LoadLanguage(string languageCode)
    {
        _currentLanguage = languageCode;
        string filePath = Path.Combine(Application.streamingAssetsPath, $"{languageCode}.json");

        if (File.Exists(filePath))
        {
            string jsonContent = File.ReadAllText(filePath);
            _localizedStrings = JsonUtility.FromJson<LocalizationData>(jsonContent).strings;
        }
        else
        {
            Debug.LogError("Localization file not found: " + filePath);
        }
    }

    // Get error message based on the FieldError enum
    public string GetStringByName(string stringName)
    {
        return _localizedStrings.ContainsKey(stringName) ? _localizedStrings[stringName] : "Unknown error";
    }
}

// Data class to parse the JSON
[System.Serializable]
public class LocalizationData
{
    public Dictionary<string, string> strings;
}
