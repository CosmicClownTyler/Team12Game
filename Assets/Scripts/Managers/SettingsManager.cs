using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    [Header("Graphic Components")]
    public Toggle holdToThrowToggle;
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


    // Gameplay 

    // Controls
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