using UnityEngine;

[CreateAssetMenu(fileName = "SettingsData", menuName = "Settings/SettingsData", order = 1)]
public class GeneralSettingsData : ScriptableObject
{
    public float musicVolume = 1.0f;
    public float soundVolume = 1.0f;
    public bool areNotificationsEnabled = true;
    public JoystickOrientation joystickOrientation= JoystickOrientation.Left;
}

public enum JoystickOrientation
{
    Left,
    Right
} 