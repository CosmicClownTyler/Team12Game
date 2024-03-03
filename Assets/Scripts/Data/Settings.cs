using UnityEngine;

public class Settings
{
    public static bool ChangeGraphicOnly = false;

    // Gameplay


    // Controls
    private static bool _holdToThrow;
    public static bool HoldToThrow
    {
        get
        {
            return _holdToThrow;
        }
        set
        {
            if (ChangeGraphicOnly) return;

            _holdToThrow = value;
            // todo: change input manager accordingly
            SettingsChanged();
        }
    }

    // Audio
    private static float _masterVolume;
    private static float _musicVolume;
    private static float _effectsVolume;
    private static float _uiVolume;
    public static float MasterVolume
    {
        get
        {
            return _masterVolume;
        }
        set
        {
            if (ChangeGraphicOnly) return;

            _masterVolume = value;
            AudioManager.Instance.SetMasterVolume(value);
            SettingsChanged();
        }
    }
    public static float MusicVolume
    {
        get
        {
            return _musicVolume;
        }
        set
        {
            if (ChangeGraphicOnly) return;

            _musicVolume  = value;
            AudioManager.Instance.SetMusicVolume(value);
            SettingsChanged();
        }
    }
    public static float EffectsVolume
    {
        get
        {
            return _effectsVolume;
        }
        set
        {
            if (ChangeGraphicOnly) return;

            _effectsVolume = value;
            AudioManager.Instance.SetEffectsVolume(value);
            SettingsChanged();
        }
    }
    public static float UIVolume
    {
        get
        {
            return _uiVolume;
        }
        set
        {
            if (ChangeGraphicOnly) return;

            _uiVolume = value;
            AudioManager.Instance.SetUIVolume(value);
            SettingsChanged();
        }
    }

    // Graphics


    // Global
    private static void SettingsChanged()
    {
        SaveSettings();
    }

    // Key name constants
    private const string holdToThrowKeyName = "Hold To Throw";
    private const string masterVolumeKeyName = "Master Volume";
    private const string musicVolumeKeyName = "Music Volume";
    private const string effectsVolumeKeyName = "Effects Volume";
    private const string uiVolumeKeyName = "UI Volume";

    public static void SaveSettings()
    {
        PlayerPrefs.SetInt(holdToThrowKeyName, HoldToThrow ? 1 : 0);
        PlayerPrefs.SetFloat(masterVolumeKeyName, MasterVolume);
        PlayerPrefs.SetFloat(musicVolumeKeyName, MusicVolume);
        PlayerPrefs.SetFloat(effectsVolumeKeyName, EffectsVolume);
        PlayerPrefs.SetFloat(uiVolumeKeyName, UIVolume);
    }

    public static void LoadSettings()
    {
        _holdToThrow = PlayerPrefs.GetInt(holdToThrowKeyName, 1) == 1;
        _masterVolume = PlayerPrefs.GetFloat(masterVolumeKeyName, 1);
        _musicVolume = PlayerPrefs.GetFloat(musicVolumeKeyName, 1);
        _effectsVolume = PlayerPrefs.GetFloat(effectsVolumeKeyName, 1);
        _uiVolume = PlayerPrefs.GetFloat(uiVolumeKeyName, 1);

        // todo set input manager
        AudioManager.Instance.SetMasterVolume(MasterVolume);
        AudioManager.Instance.SetMusicVolume(MusicVolume);
        AudioManager.Instance.SetEffectsVolume(EffectsVolume);
        AudioManager.Instance.SetUIVolume(UIVolume);
    }
}