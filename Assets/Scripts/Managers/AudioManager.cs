using UnityEngine;

public class AudioManager : MonoBehaviour
{
    // Singleton
    public static AudioManager Instance;

    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource effectsSource;
    [SerializeField] private AudioSource uiSource;
    [SerializeField] private AudioClip defaultMusicClip;

    private void Awake()
    {
        // Manage singleton
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

    // Play the audio clip
    public void PlayUISound(AudioClip clip)
    {
        uiSource.PlayOneShot(clip);
    }

    private void Start()
    {
        PlayBackgroundMusic(defaultMusicClip, true); // Automatically start playing the default music clip
    }

    public void PlayBackgroundMusic(AudioClip clip, bool loop = true)
    {
        if (musicSource.isPlaying)
        {
            musicSource.Stop(); // Stop current music if any is playing
        }
        musicSource.clip = clip; // Set the new clip
        musicSource.loop = loop; // Set looping based on parameter
        musicSource.Play(); // Start playing the music
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