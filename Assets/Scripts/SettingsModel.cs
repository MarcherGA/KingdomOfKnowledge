using System;
using System.IO;
using UnityEngine;

public class SettingsModel : MonoBehaviour
{
    [SerializeField] private GeneralSettingsData settingsData;  // Reference to the ScriptableObject for storing settings
    public event Action SettingsChanged;  // Event to notify when settings change

    private string settingsFilePath; // Path for saving settings

    void Awake()
    {
        settingsFilePath = Application.persistentDataPath + "/settings.json";
    }

    // Save the settings to a JSON file
    public void SaveSettings()
    {
        string json = JsonUtility.ToJson(settingsData, true);
        File.WriteAllText(settingsFilePath, json);  // Write to file in persistent data path
        SettingsChanged?.Invoke();
    }

    // Load the settings from a JSON file
    public void LoadSettings()
    {
        if (File.Exists(settingsFilePath))
        {
            string json = File.ReadAllText(settingsFilePath);
            JsonUtility.FromJsonOverwrite(json, settingsData);  // Overwrite with saved data
            SettingsChanged?.Invoke();
        }
        else
        {
            Debug.LogWarning("Settings file not found, using default settings.");
        }
    }
    // Getter and setter for settings data
    public float MusicVolume
    {
        get => settingsData.musicVolume;
        set
        {
            settingsData.musicVolume = value;
            SaveSettings();  // Save settings after change
        }
    }

    public float SoundVolume
    {
        get => settingsData.soundVolume;
        set
        {
            settingsData.soundVolume = value;
            SaveSettings();
        }
    }

    public bool AreNotificationsEnabled
    {
        get => settingsData.areNotificationsEnabled;
        set
        {
            settingsData.areNotificationsEnabled = value;
            SaveSettings();
        }
    }

    public JoystickOrientation JoystickOrientation
    {
        get => settingsData.joystickOrientation;
        set
        {
            settingsData.joystickOrientation = value;
            SaveSettings();
        }
    }
}
