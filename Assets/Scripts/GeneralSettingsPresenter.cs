using UnityEngine;

public class GeneralSettingsPresenter : MonoBehaviour
{
    [SerializeField] private GeneralSettingsView view;  // Reference to the View
    [SerializeField] private SettingsModel model;  // Reference to the Model

    private void Start()
    {
        // Subscribe to the View's events
        view.MusicVolumeChanged += OnMusicVolumeChanged;
        view.SoundVolumeChanged += OnSoundVolumeChanged;
        view.NotificationsEnabledChanged += OnNotificationsEnabledChanged;
        view.JoystickOrientationChanged += OnJoystickOrientationChanged;

        // Load settings when the scene starts
        model.LoadSettings();

        // Update the View with the current settings from the Model
        UpdateView();
    }

    // Event handler for music volume change
    private void OnMusicVolumeChanged(float volume)
    {
        model.MusicVolume = volume;
    }

    // Event handler for sound volume change
    private void OnSoundVolumeChanged(float volume)
    {
        model.SoundVolume = volume;
    }

    // Event handler for notifications toggle
    private void OnNotificationsEnabledChanged(bool isEnabled)
    {
        model.AreNotificationsEnabled = isEnabled;
    }

    // Event handler for joystick orientation change
    private void OnJoystickOrientationChanged(JoystickOrientation orientation)
    {
        model.JoystickOrientation = orientation;
    }

    // Update the View based on the settings in the Model
    private void UpdateView()
    {
        view.SetMusicVolume(model.MusicVolume);
        view.SetSoundVolume(model.SoundVolume);
        view.SetNotificationsEnabled(model.AreNotificationsEnabled);
        view.SetJoystickOrientation(model.JoystickOrientation);
    }

    private void OnDestroy()
    {
        // Unsubscribe from events to avoid memory leaks
        view.MusicVolumeChanged -= OnMusicVolumeChanged;
        view.SoundVolumeChanged -= OnSoundVolumeChanged;
        view.NotificationsEnabledChanged -= OnNotificationsEnabledChanged;
        view.JoystickOrientationChanged -= OnJoystickOrientationChanged;
    }
}
