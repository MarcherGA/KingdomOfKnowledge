using UnityEngine;
using UnityEngine.UI;
using System;

public class GeneralSettingsView : MonoBehaviour
{
    public event Action<float> MusicVolumeChanged;
    public event Action<float> SoundVolumeChanged;
    public event Action<bool> NotificationsEnabledChanged;
    public event Action<JoystickOrientation> JoystickOrientationChanged;

    // UI elements (these should be assigned via the Unity Inspector)
    [SerializeField] private Slider musicVolumeSlider;
    [SerializeField] private Slider soundVolumeSlider;
    [SerializeField] private ToggleSwitch notificationsToggle;
    [SerializeField] private ToggleSwitch joystickOrientationToggle;

    void Start()
    {
        musicVolumeSlider.onValueChanged.AddListener(OnMusicVolumeChanged);
        soundVolumeSlider.onValueChanged.AddListener(OnSoundVolumeChanged);
        notificationsToggle.onValueChanged.AddListener(OnNotificationsEnabledChanged);
        joystickOrientationToggle.onValueChanged.AddListener(OnJoystickOrientationChanged);
    }

    // These methods are called when the user interacts with the UI
    public void OnMusicVolumeChanged(float volume)
    {
        MusicVolumeChanged?.Invoke(volume);
    }

    public void OnSoundVolumeChanged(float volume)
    {
        SoundVolumeChanged?.Invoke(volume);
    }

    public void OnNotificationsEnabledChanged(bool isEnabled)
    {
        NotificationsEnabledChanged?.Invoke(isEnabled);
    }

    public void OnJoystickOrientationChanged(bool isRight)
    {
        // Convert the dropdown index to the corresponding JoystickOrientation enum
        JoystickOrientationChanged?.Invoke(GetJoystickOrientationByBool(isRight));
    }

    // Methods to update the UI based on the model data
    public void SetMusicVolume(float volume)
    {
        musicVolumeSlider.value = volume;
    }

    public void SetSoundVolume(float volume)
    {
        soundVolumeSlider.value = volume;
    }

    public void SetNotificationsEnabled(bool isEnabled)
    {
        notificationsToggle.Toggle(isEnabled);
    }

    public void SetJoystickOrientation(JoystickOrientation joystickOrientation)
    {
        joystickOrientationToggle.Toggle(GetBoolByJoystickOrientation(joystickOrientation));
    }

    private JoystickOrientation GetJoystickOrientationByBool(bool isRight)
    {
        return isRight? JoystickOrientation.Right : JoystickOrientation.Left;
    }

    private bool GetBoolByJoystickOrientation(JoystickOrientation joystickOrientation)
    {
        return joystickOrientation == JoystickOrientation.Right;
    }
}
