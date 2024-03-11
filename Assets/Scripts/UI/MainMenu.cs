using UnityEngine;
using UnityEngine.SceneManagement;

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

    public void LoadParkScene()
    {
        GameManager.Instance.LoadScene("Park");
    }
    public void LoadCityScene()
    {
        GameManager.Instance.LoadScene("City");
    }
    public void LoadRandomScene()
    {
        float randInt = Random.Range(0, 10);
        if (randInt > 5)
        {
            LoadParkScene();
        }
        else
        {
            LoadCityScene();
        }
    }

    public void SetGameType(GameTypeComponent gameTypeComponent)
    {
        GameManager.Instance.SetGameType(gameTypeComponent.GameType);
    }
    public void SetGamePlayers(GamePlayersComponent gamePlayersComponent)
    {
        GameManager.Instance.SetGamePlayers(gamePlayersComponent.GamePlayers);
    }
    public void SetPinGroup(PinGroupComponent pinGroupComponent)
    {
        GameManager.Instance.SetPinGroup(pinGroupComponent.PinGroup);
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