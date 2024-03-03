using UnityEngine;

public class PauseManager : MonoBehaviour
{
    // Singleton
    public static PauseManager Instance;

    public bool IsPaused = false;

    public GameObject pauseMenu;
    public MenuPageGroup pauseMenuPageGroup;
    public MenuPage mainPage;
    public MenuPage settingsPage;

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
        pauseMenu.SetActive(true);
        Time.timeScale = 0;
        InputManager.Instance.PauseGameInput();
        GameManager.Instance.UnlockCursor();
        PlayerManager.Instance.DisablePlayerUI();
    }

    public void Resume()
    {
        IsPaused = false;
        pauseMenu.SetActive(false);
        Time.timeScale = 1;
        InputManager.Instance.ResumeGameInput();
        GameManager.Instance.LockCursor();
        PlayerManager.Instance.EnablePlayerUI();
        pauseMenuPageGroup.SelectPage(mainPage);
    }

}