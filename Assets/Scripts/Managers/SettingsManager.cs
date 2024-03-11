using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    [Header("Graphic Components")]
    public Toggle holdToThrowToggle;
    public Slider horizontalSensitivity;
    public Slider verticalSensitivity;
    public Slider masterVolumeSlider;
    public Slider musicVolumeSlider;
    public Slider effectsVolumeSlider;
    public Slider uiVolumeSlider;

    // Update the graphics based on the actual settings values. 
    public void UpdateGraphics()
    {
        // Change the graphics to reflect the settings without changing the actual settings
        Settings.ChangeGraphicOnly = true;
        holdToThrowToggle.isOn = Settings.HoldToThrow;
        horizontalSensitivity.value = Settings.HorizontalSensitivity;
        verticalSensitivity.value = Settings.VerticalSensitivity;
        masterVolumeSlider.value = Settings.MasterVolume;
        musicVolumeSlider.value = Settings.MusicVolume;
        effectsVolumeSlider.value = Settings.EffectsVolume;
        uiVolumeSlider.value = Settings.UIVolume;
        Settings.ChangeGraphicOnly = false;
    }

    private void Start()
    {
        UpdateGraphics();
    }


    // Reset all settings
    public void ResetAllSettings()
    {
        ResetAllGameplaySettings();
        ResetAllControlsSettings();
        ResetAllAudioSettings();
        ResetAllGraphicsSettings();
    }
    // Reset all settings by category
    public void ResetAllGameplaySettings()
    {
        
    }
    public void ResetAllControlsSettings()
    {
        ResetHoldToThrow();
        ResetHorizontalSensitivity();
        ResetVerticalSensitivity();
    }
    public void ResetAllAudioSettings()
    {
        ResetMasterVolume();
        ResetMusicVolume();
        ResetEffectsVolume();
        ResetUIVolume();
    }
    public void ResetAllGraphicsSettings()
    {
        
    }
    // Reset individual settings
    // Gameplay
    // Controls
    public void ResetHoldToThrow()
    {
        Settings.ResetHoldToThrow();
        UpdateGraphics();
    }
    public void ResetHorizontalSensitivity()
    {
        Settings.ResetHorizontalSensitivity();
        UpdateGraphics();
    }
    public void ResetVerticalSensitivity()
    {
        Settings.ResetVerticalSensitivity();
        UpdateGraphics();
    }
    // Audio
    public void ResetMasterVolume()
    {
        Settings.ResetMasterVolume();
        UpdateGraphics();
    }
    public void ResetMusicVolume()
    {
        Settings.ResetMusicVolume();
        UpdateGraphics();
    }
    public void ResetEffectsVolume()
    {
        Settings.ResetEffectsVolume();
        UpdateGraphics();
    }
    public void ResetUIVolume()
    {
        Settings.ResetUIVolume();
        UpdateGraphics();
    }
    // Graphics


    // Set settings using dynamic values from UI
    // Controls
    public void SetHorizontalMouseSensitivity(float sensitivity)
    {
        Settings.HorizontalSensitivity = sensitivity;
    }
    public void SetVerticalMouseSensitivity(float sensitivity)
    {
        Settings.VerticalSensitivity = sensitivity;
    }
    public void SetHoldToThrow(bool holdToThrow)
    {
        Settings.HoldToThrow = holdToThrow;
    }

    // Audio
    public void SetMasterVolume(float volume)
    {
        Settings.MasterVolume = volume;
    }
    public void SetMusicVolume(float volume)
    {
        Settings.MusicVolume = volume;
    }
    public void SetEffectsVolume(float volume)
    {
        Settings.EffectsVolume = volume;
    }
    public void SetUIVolume(float volume)
    {
        Settings.UIVolume = volume;
    }

    // Graphics

}