using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameLogic : MonoBehaviour
{
    [Header("Pin Group Prefabs")]
    public GameObject[] pinGroupPrefabArray;
    
    [Header("Game Settings")]
    public GameType selectedGameType = GameType.One;

    [Header("Locations")]
    public Transform pinGroupSpawn;
    public Transform firstThrowingArea;
    public Transform secondThrowingArea;

    // Data:
    [Header("Player References")]
    public TextMeshProUGUI currentPinGroupText;
    public Image notificationImage;
    private TextMeshProUGUI notificationText;

    // The current pin group prefab to be instantiated
    private GameObject currentPinGroupPrefab;
    // The list of indices to use from the array of pin group prefabs for this game type
    private int[] pinGroupIndices;
    // The current pin group index
    private int currentPinGroupIndex = 0;
    // A reference to the current pin group game object
    private GameObject currentPinGroup;
    // A reference to the stop checker for the current pin group game object
    private PinGroupStopChecker currentPinGroupStopChecker;
    // Whether or not the pins have moved since the last check
    private bool havePinsMovedSinceLastCheck = false;

    // Throw statistics
    private int totalThrows = 0;
    private int throwsPerPinGroup = 0;

    private GameObject player;
    private BatThrower playerThrower;

    private AreaPinChecker[] areaPinCheckers;

    private void Start()
    {
        // Log a warning if there are no pin group prefabs assigned in the inspector
        if (pinGroupPrefabArray.Length == 0)
        {
            Debug.LogWarning("No pin group prefabs have been assigned in the inspector. ");
        }

        // Disable the notification image if it is assigned, log a warning if it is not
        if (notificationImage != null)
        {
            notificationText = notificationImage.GetComponentInChildren<TextMeshProUGUI>();
            notificationImage.enabled = false;
        }
        else
        {
            Debug.LogError("The notification image is not assigned in the inspector. ");
        }

        // Get the player object
        player = GameObject.FindWithTag("Player");
        playerThrower = player.GetComponentInChildren<BatThrower>();

        // Get the area pin checkers for the city and suburb areas
        areaPinCheckers = GetComponentsInChildren<AreaPinChecker>();

        // Get the list of pin group indices for the selected game type
        pinGroupIndices = GameTypeManager.GetPinGroupList(selectedGameType);
        currentPinGroupIndex = 0;

        // Get the pin group prefab and spawn it
        GetCurrentPinGroupPrefab();
        SpawnCurrentPinGroup();
    }

    private void Update()
    {
        // If the pins are stopped after being moved
        if (havePinsMovedSinceLastCheck && currentPinGroupStopChecker.IsStopped())
        {
            havePinsMovedSinceLastCheck = false;

            if (throwsPerPinGroup >= 1)
            {
                AfterThrow();
            }
        }

        // If the pins are moved after being stopped
        if (!havePinsMovedSinceLastCheck && !currentPinGroupStopChecker.IsStopped())
        {
            havePinsMovedSinceLastCheck = true;
        }
    }

    // Return true if any of the areas contain pins
    private bool ContainsPinsInAnyAreas()
    {
        foreach (AreaPinChecker areaPinChecker in areaPinCheckers)
        {
            if (areaPinChecker.ContainsPins())
            {
                return true;
            }
        }

        return false;
    }

    private void GetCurrentPinGroupPrefab()
    {
        currentPinGroupPrefab = pinGroupPrefabArray[currentPinGroupIndex];
    }
    private void MoveToNextPinGroup()
    {
        if (currentPinGroup != null)
        {
            Destroy(currentPinGroup);
        }

        // Reset the throws per pin group when the new one is spawned
        throwsPerPinGroup = 0;

        currentPinGroupIndex++;
        GetCurrentPinGroupPrefab();
        SpawnCurrentPinGroup();

    }

    private void SpawnCurrentPinGroup()
    {
        currentPinGroup = Instantiate(currentPinGroupPrefab, pinGroupSpawn.position, pinGroupSpawn.rotation);
        currentPinGroupStopChecker = currentPinGroup.GetComponent<PinGroupStopChecker>();

        currentPinGroupText.text = "Current Figure: #" + (currentPinGroupIndex + 1);
    }

    private void AfterThrow()
    {
        // If there are no pins left, move to the next pin group (or end the game if there are none left)
        if (!ContainsPinsInAnyAreas())
        {
            if (throwsPerPinGroup > 1)
            {
                string text = string.Format("Congratulations! You knocked the whole figure out of the gorod in {0} throws. Moving you back to the start", GetTotalPerObjectThrows());
                ShowTextOnScreen(text, 7);
            }
            else
            {
                ShowTextOnScreen("Congratulations! You knocked the whole figure out of the gorod in one shot!", 7);
            }

            // Move to the next pin group if it exists
            if (GameTypeManager.HasNextPinGroup(selectedGameType, currentPinGroupIndex))
            {
                ShowTextOnScreen("You knocked some gorodki from the gorod. Moving you closer!", 7);
                MovePlayerToFirstThrowingArea();
                SpawnCurrentPinGroup();
            }
            else
            {
                GameCompleted();
            }
        }
        else
        {
            // Move the player to the second throwing area if they've just completed their first throw
            if (throwsPerPinGroup == 1)
            {
                MovePlayerToSecondThrowingArea();
            }
        }
    }

    private void MovePlayerToFirstThrowingArea()
    {
        playerThrower.transform.position = firstThrowingArea.position;
    }
    private void MovePlayerToSecondThrowingArea()
    {
        playerThrower.transform.position = secondThrowingArea.position;
    }

    public void IncreaseThrows()
    {
        totalThrows++;
        throwsPerPinGroup++;
    }

    public int GetTotalThrows()
    {
        return totalThrows;
    }

    public int GetTotalPerObjectThrows()
    {
        return throwsPerPinGroup;
    }

    private IEnumerator ReturnToMenu(int timeout)
    {
        yield return new WaitForSeconds(timeout);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        SceneManager.LoadScene("Menu", LoadSceneMode.Single);
    }

    private void GameCompleted()
    {
        string gameCompletexText = string.Format("Congratulations! You finished the game with a total of {0} throws. Well done!", GetTotalPerObjectThrows());
        ShowTextOnScreen(gameCompletexText, 7);

        StartCoroutine(ReturnToMenu(3));
        // TODO: Game over! Show score!
        return;
    }

    private IEnumerator SendNotification(string text, int timeout)
    {
        Debug.Log(text);
        Debug.Log(timeout);
        notificationImage.enabled = true;
        notificationText.text = text;
        yield return new WaitForSeconds(timeout);
        notificationText.text = "";
        notificationImage.enabled = false;
    }
    private void ShowTextOnScreen(string text, int time)
    {
        StartCoroutine(SendNotification(text, time));
    }
}