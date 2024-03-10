using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Singleton
    public static GameManager Instance;

    private GameType gameType;
    private GamePlayers gamePlayers;

    private GameObject gameAreaObject;
    private GameArea gameArea;

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

        SceneManager.activeSceneChanged += SceneChanged;

        OnSceneLoad(SceneManager.GetActiveScene());
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

    public void SetGameType(GameType gameType)
    {
        this.gameType = gameType;
    }
    public void SetGamePlayers(GamePlayers gamePlayers)
    {
        this.gamePlayers = gamePlayers;
    }

    public void StartTutorial()
    {
        LoadScene("Tutorial");
    }

    
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
    private void SceneChanged(Scene oldScene, Scene newScene)
    {
        OnSceneLoad(newScene);
    }

    private void OnSceneLoad(Scene scene)
    {
        if (scene.name == "Tutorial")
        {

            return;
        }

        // If the new scene isn't the menu
        if (scene.name != "Menu")
        {
            // todo: update pause menu settings
            PlayerManager.Instance.SetActivePlayers();
            InputManager.Instance.ResumeGameInput();
            PauseManager.Instance.Resume();
            gameAreaObject = GameObject.FindWithTag("GameArea");
            gameArea = gameAreaObject.GetComponent<GameArea>();
            gameArea.selectedGameType = gameType;
            gameArea.selectedGamePlayers = gamePlayers;
            LockCursor();
            gameArea.StartGame();
        }

        // If the new scene is the menu
        if (scene.name == "Menu")
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