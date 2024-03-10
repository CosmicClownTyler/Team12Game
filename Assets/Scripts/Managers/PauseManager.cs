using UnityEngine;

public class PauseManager : MonoBehaviour
{
    // Singleton
    public static PauseManager Instance;

    public bool IsPaused = false;

    public PauseMenu pauseMenu;

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

    private void Update()
    {
        if (InputManager.Instance.PausePressed && !IsPaused)
        {
            Pause();
        }
        
        if (InputManager.Instance.ResumePressed && IsPaused)
        {
            Resume();
        }
    }

    public void Pause()
    {
        IsPaused = true;
        pauseMenu.ShowPauseMenu();
        Time.timeScale = 0;
        InputManager.Instance.PauseGameInput();
        GameManager.Instance.UnlockCursor();
        PlayerManager.Instance.DisableActivePlayerUI();
        PlayerManager.Instance.GetActivePlayer().playerThrower.CancelThrow();
    }

    public void Resume()
    {
        IsPaused = false;
        pauseMenu.HidePauseMenu();
        Time.timeScale = 1;
        InputManager.Instance.ResumeGameInput();
        GameManager.Instance.LockCursor();
        PlayerManager.Instance.EnableActivePlayerUI();
    }

}