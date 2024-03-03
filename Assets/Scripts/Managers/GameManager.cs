using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Singleton
    public static GameManager Instance;

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

    private void Start()
    {
        Settings.LoadSettings();
    }

    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_STANDALONE
        Application.Quit();
#elif UNITY_WEBGL
        // Do nothing (can't reliably force close a webgl build)
#endif
    }

    public void StartTutorial()
    {
        // todo
    }

    public void StartGame(int numOfPlayers)
    {
        LoadScene("prefab creation scene");
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);

        // UPDATE SETTINGS WHEN SWITCHING SCENES AND PAUSING/UNPAUSING SO THAT THEY ARE ALWAYS CONSISTENT

        // If loading a scene that isn't the menu
        if (sceneName != "Menu")
        {
            // todo: update pause menu settings
            InputManager.Instance.ResumeGameInput();
            LockCursor();
        }
        
        // If loading the menu scene
        if (sceneName == "Menu")
        {
            // todo: update main menu settings
            InputManager.Instance.PauseGameInput();
            UnlockCursor();
        }
    }

    private IEnumerator LoadSceneAsync(string sceneName)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            Debug.Log("waiting for scene to load... " + asyncLoad.progress);
            yield return null;
        }

        Debug.Log("scene finished loading");
    }

    private void EnablePlayer()
    {
        PlayerManager.Instance.EnablePlayer();
    }

    private void DisablePlayer()
    {
        PlayerManager.Instance.DisablePlayer();
    }

    public void LockCursor()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void UnlockCursor()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

}