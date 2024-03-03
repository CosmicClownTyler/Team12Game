using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public void StartTutorial()
    {
        GameManager.Instance.StartTutorial();
    }

    public void StartSingleplayerGame()
    {
        GameManager.Instance.StartGame(1);
    }
}