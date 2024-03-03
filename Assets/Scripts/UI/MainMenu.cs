using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public GameObject exitButton;
    public GameObject exitPopup;

    private void Start()
    {
#if UNITY_EDITOR
        // Do nothing (this directive avoids disabling the exit button in edit mode)
#elif (UNITY_WEBGL)
        exitButton.SetActive(false);
#endif
    }

    public void StartTutorial()
    {
        GameManager.Instance.StartTutorial();
    }

    public void StartSingleplayerGame()
    {
        GameManager.Instance.StartGame(1);
    }

    public void ShowExitModal()
    {
        exitPopup.SetActive(true);
    }
    public void HideExitModal()
    {
        exitPopup.SetActive(false);
    }

    public void ExitGame()
    {
        GameManager.Instance.ExitGame();
    }
} 