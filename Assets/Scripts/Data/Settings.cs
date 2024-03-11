using UnityEngine;

public class Settings
{
    public static bool ChangeGraphicOnly = false;

    // Gameplay

    // Controls
    private static bool _holdToThrow;
    private const bool HoldToThrowDefault = true;
    private static float _horizontalSensitivity;
    private const float HorizontalSensitivityDefault = 1.0f;
    private static float _verticalSensitivity;
    private const float VerticalSensitivityDefault = 1.0f;
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
    public static float HorizontalSensitivity
    {
        get
        {
            return _horizontalSensitivity;
        }
        set
        {
            if (ChangeGraphicOnly) return;

            if (value <= 0 || value > 2)
            {
                Debug.LogWarning("The horizontal sensitivity cannot be less than or equal to 0 or greater than 2. " + value);
                return;
            }

            _horizontalSensitivity = value;
            InputManager.Instance.SetSensitivity(_horizontalSensitivity, _verticalSensitivity);
            SettingsChanged();
        }
    }
    public static float VerticalSensitivity
    {
        get
        {
            return _verticalSensitivity;
        }
        set
        {
            if (ChangeGraphicOnly) return;

            if (value < 0.009 || value > 2)
            {
                Debug.LogWarning("The vertical sensitivity cannot be less than or equal to 0 or greater than 2. " + value);
                return;
            }

            _verticalSensitivity = value;
            InputManager.Instance.SetSensitivity(_horizontalSensitivity, _verticalSensitivity);
            SettingsChanged();
        }
    }

    // Audio
    private static float _masterVolume;
    private const float MasterVolumeDefault = 1.0f;
    private static float _musicVolume;
    private const float MusicVolumeDefault = 1.0f;
    private static float _effectsVolume;
    private const float EffectsVolumeDefault = 1.0f;
    private static float _uiVolume;
    private const float UIVolumeDefault = 1.0f;
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
    private const string horizontalSensitivityName = "X Sensitivity";
    private const string verticalSensitivityName = "Y Sensitivity";
    private const string masterVolumeKeyName = "Master Volume";
    private const string musicVolumeKeyName = "Music Volume";
    private const string effectsVolumeKeyName = "Effects Volume";
    private const string uiVolumeKeyName = "UI Volume";

    public static void SaveSettings()
    {
        PlayerPrefs.SetInt(holdToThrowKeyName, HoldToThrow ? 1 : 0);
        PlayerPrefs.SetFloat(horizontalSensitivityName, HorizontalSensitivity);
        PlayerPrefs.SetFloat(verticalSensitivityName, VerticalSensitivity);
        PlayerPrefs.SetFloat(masterVolumeKeyName, MasterVolume);
        PlayerPrefs.SetFloat(musicVolumeKeyName, MusicVolume);
        PlayerPrefs.SetFloat(effectsVolumeKeyName, EffectsVolume);
        PlayerPrefs.SetFloat(uiVolumeKeyName, UIVolume);
    }

    public static void LoadSettings()
    {
        int holdToThrowDefaultInt = BoolToInt(HoldToThrowDefault);
        _holdToThrow = IntToBool(PlayerPrefs.GetInt(holdToThrowKeyName, holdToThrowDefaultInt));
        _horizontalSensitivity = PlayerPrefs.GetFloat(horizontalSensitivityName, HorizontalSensitivityDefault);
        _verticalSensitivity = PlayerPrefs.GetFloat(verticalSensitivityName, VerticalSensitivityDefault);
        _masterVolume = PlayerPrefs.GetFloat(masterVolumeKeyName, MasterVolumeDefault);
        _musicVolume = PlayerPrefs.GetFloat(musicVolumeKeyName, MusicVolumeDefault);
        _effectsVolume = PlayerPrefs.GetFloat(effectsVolumeKeyName, EffectsVolumeDefault);
        _uiVolume = PlayerPrefs.GetFloat(uiVolumeKeyName, UIVolumeDefault);

        // todo set input manager for hold to throw
        InputManager.Instance.SetSensitivity(_horizontalSensitivity, _verticalSensitivity);
        AudioManager.Instance.SetMasterVolume(MasterVolume);
        AudioManager.Instance.SetMusicVolume(MusicVolume);
        AudioManager.Instance.SetEffectsVolume(EffectsVolume);
        AudioManager.Instance.SetUIVolume(UIVolume);
    }

    // Reset all settings
    public static void ResetAllSettings()
    {
        ResetAllGameplaySettings();
        ResetAllControlsSettings();
        ResetAllAudioSettings();
        ResetAllGraphicsSettings();
    }
    // Reset all settings by category
    public static void ResetAllGameplaySettings()
    {
        
    }
    public static void ResetAllControlsSettings()
    {
        ResetHoldToThrow();
        ResetHorizontalSensitivity();
        ResetVerticalSensitivity();
    }
    public static void ResetAllAudioSettings()
    {
        MasterVolume = MasterVolumeDefault;
        MusicVolume = MusicVolumeDefault;
        EffectsVolume = EffectsVolumeDefault;
        UIVolume = UIVolumeDefault;
    }
    public static void ResetAllGraphicsSettings()
    {

    }
    // Reset individual settings
    // Gameplay
    // Controls
    public static void ResetHoldToThrow()
    {
        HoldToThrow = HoldToThrowDefault;
    }
    public static void ResetHorizontalSensitivity()
    {
        HorizontalSensitivity = HorizontalSensitivityDefault;
    }
    public static void ResetVerticalSensitivity()
    {
        VerticalSensitivity = VerticalSensitivityDefault;
    }
    // Audio
    public static void ResetMasterVolume()
    {
        MasterVolume = MasterVolumeDefault;
    }
    public static void ResetMusicVolume()
    {
        MusicVolume = MusicVolumeDefault;
    }
    public static void ResetEffectsVolume()
    {
        EffectsVolume = EffectsVolumeDefault;
    }
    public static void ResetUIVolume()
    {
        UIVolume = UIVolumeDefault;
    }
    // Graphics

    // Miscellaneous
    private static int BoolToInt(bool b)
    {
        return b ? 1 : 0;
    }
    private static bool IntToBool(int i)
    {
        return i == 1;
    }
}