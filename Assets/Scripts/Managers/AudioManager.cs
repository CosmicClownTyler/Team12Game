using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource effectsSource;
    [SerializeField] private AudioSource uiSource;

    private void Awake()
    {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Play the audio clip
    public void PlayUISound(AudioClip clip)
    {
        uiSource.PlayOneShot(clip);
    }

    // Set volume
    public void SetMasterVolume(float volume)
    {
        AudioListener.volume = volume;
    }
    public void SetMusicVolume(float volume)
    {
        musicSource.volume = volume;
    }
    public void SetEffectsVolume(float volume)
    {
        effectsSource.volume = volume;
    }
    public void SetUIVolume(float volume)
    {
        uiSource.volume = volume;
    }

    public void PlaySoundEffect(AudioClip clip)
    {
        if (clip != null)
        {
            effectsSource.PlayOneShot(clip);
        }
        else
        {
            Debug.LogWarning("Attempted to play a null sound effect.");
        }
    }
}