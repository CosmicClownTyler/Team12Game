using System.Collections;
using UnityEngine;

public class TutorialGame : MonoBehaviour
{
    [Header("Pin Group Prefabs")]
    public GameObject[] pinGroupPrefabArray;

    [Header("Locations")]
    public Transform pinGroupSpawn;
    public Transform firstThrowingArea;
    public Transform secondThrowingArea;

    [Header("Tutorial Text Panels")]
    public GameObject[] tutorialPanels;
    private int tutorialPanelsIndex;
    private bool isTutorialFinished = false;

    private GameType selectedGameType = GameType.Full;

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
    // All of the area pin checkers for whether or not the game area contains pins
    private AreaPinChecker[] areaPinCheckers;
    // Whether or not the pins have moved since the last check
    private bool havePinsMovedSinceLastCheck = false;

    // If the player is at the first area or the second area
    private bool atSecondThrowingArea;

    private bool readyToStart = false;
    private bool hasStarted = false;

    // Tutorial continue input flags
    private bool shouldMovePanel = false;
    private bool canMovePanel = true;

    private void Start()
    {
        // Log a warning if there are no pin group prefabs assigned in the inspector
        if (pinGroupPrefabArray.Length == 0)
        {
            Debug.LogWarning("No pin group prefabs have been assigned in the inspector. ");
        }

        // Get the area pin checkers for the city and suburb areas
        areaPinCheckers = GetComponentsInChildren<AreaPinChecker>();

        // Get the list of pin group indices for the selected game type
        pinGroupIndices = GameTypeManager.GetPinGroupList(selectedGameType);
        currentPinGroupIndex = 0;

        // Turn off all tutorial panels but the first
        for (int i = 1; i < tutorialPanels.Length; i++)
        {
            tutorialPanels[i].SetActive(false);
        }

        readyToStart = true;
    }

    private void Update()
    {
        // Do nothing if the game has not started yet
        if (!hasStarted)
        {
            return;
        }

        // If the tutorial isn't finished, don't run any game logic and check for Continue presses
        if (!isTutorialFinished)
        {
            shouldMovePanel = false;

            if (InputManager.Instance.ContinueWasPressed)
            {
                shouldMovePanel = true;
            }
            else if (InputManager.Instance.ContinueWasReleased)
            {
                canMovePanel = true;
            }

            if (shouldMovePanel && canMovePanel)
            {
                ContinueTutorial();
                canMovePanel = false;
            }

            return;
        }

        // cache the active player for this frame
        Player activePlayer = PlayerManager.Instance.GetActivePlayer();

        // If the pins have stopped after being moved (the player's throw has finished)
        if (havePinsMovedSinceLastCheck && currentPinGroupStopChecker.IsStopped())
        {
            havePinsMovedSinceLastCheck = false;

            if (activePlayer.GetCurrentThrowCount() >= 1)
            {
                AfterThrow();
            }
        }

        // If the pins have moved after being stopped (the player has thrown and hit something)
        if (!currentPinGroupStopChecker.IsStopped() && !havePinsMovedSinceLastCheck)
        {
            havePinsMovedSinceLastCheck = true;
        }

        // If the pins are stopped and the player thrower is waiting for the pins, set can throw
        if (currentPinGroupStopChecker.IsStopped() && activePlayer.ThrowerWaitingOnPins() && !activePlayer.ThrowerWaitingOnBat())
        {
            activePlayer.SetCanThrow();
        }
    }

    public void StartGame()
    {
        StartCoroutine(TryToStart());
    }

    private IEnumerator TryToStart()
    {
        if (readyToStart)
        {
            // Get the pin group prefab and spawn it
            GetCurrentPinGroupPrefab();
            SpawnCurrentPinGroup();
            PlayerManager.Instance.GetActivePlayer().SetCanThrow(false);
            PlayerManager.Instance.GetActivePlayer().playerUI.waitingToThrowText.enabled = false;
            hasStarted = true;
            yield break;
        }
        else
        {
            yield return new WaitForEndOfFrame();
            yield return TryToStart();
        }
    }

    // Iterate through the tutorial panels
    private void ContinueTutorial()
    {
        if (isTutorialFinished) return;
        
        // Turn off the current panel
        tutorialPanels[tutorialPanelsIndex].SetActive(false);

        // Increment the index
        tutorialPanelsIndex++;

        // Turn on the next panel if it exists
        if (tutorialPanelsIndex < tutorialPanels.Length)
        {
            tutorialPanels[tutorialPanelsIndex].SetActive(true);
        }
        // Otherwise set tutorial is finished to true
        else
        {
            PlayerManager.Instance.GetActivePlayer().SetCanThrow();
            PlayerManager.Instance.GetActivePlayer().playerUI.waitingToThrowText.enabled = true;
            isTutorialFinished = true;
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
        currentPinGroupPrefab = pinGroupPrefabArray[pinGroupIndices[currentPinGroupIndex]];
    }
    private void MoveToNextPinGroup()
    {
        if (currentPinGroup != null)
        {
            Destroy(currentPinGroup);
        }

        // Reset the throws per pin group when the new one is spawned
        PlayerManager.Instance.GetActivePlayer().ResetCurrentThrowCount();

        currentPinGroupIndex++;
        GetCurrentPinGroupPrefab();
        SpawnCurrentPinGroup();
    }

    private void SpawnCurrentPinGroup()
    {
        currentPinGroup = Instantiate(currentPinGroupPrefab, pinGroupSpawn.position, pinGroupSpawn.rotation);
        currentPinGroupStopChecker = currentPinGroup.GetComponent<PinGroupStopChecker>();

        string currentPinGroupText = "Current Figure: " + GetPrefabName(currentPinGroupPrefab) + " (" + (currentPinGroupIndex + 1) + "/" + (pinGroupIndices.Length) + ")";
        PlayerManager.Instance.GetActivePlayer().playerUI.currentPinGroupText.text = currentPinGroupText;

        MovePlayerToFirstThrowingArea();
    }

    // Move the player transform to the appropriate throwing areas
    private void MovePlayerToFirstThrowingArea()
    {
        PlayerManager.Instance.GetActivePlayer().playerObject.transform.position = firstThrowingArea.position;
        PlayerManager.Instance.GetActivePlayer().SetCanThrow();
        atSecondThrowingArea = false;
    }
    private void MovePlayerToSecondThrowingArea()
    {
        PlayerManager.Instance.GetActivePlayer().playerObject.transform.position = secondThrowingArea.position;
        PlayerManager.Instance.GetActivePlayer().SetCanThrow();
        atSecondThrowingArea = true;
    }

    // Throwing logic and statistics
    private void AfterThrow()
    {
        // If there are no pins left, move to the next pin group (or end the game if there are none left)
        if (!ContainsPinsInAnyAreas())
        {
            if (PlayerManager.Instance.GetActivePlayer().GetCurrentThrowCount() > 1)
            {
                string text = string.Format("Congratulations! You knocked all the gorodki out in {0} throws", PlayerManager.Instance.GetActivePlayer().GetCurrentThrowCount());
                ShowPlayerNotification(text, 7);
            }
            else
            {
                ShowPlayerNotification("Congratulations! You knocked all the gorodki out in one shot!", 7);
            }

            // Move to the next pin group if it exists
            if (GameTypeManager.HasNextPinGroup(selectedGameType, currentPinGroupIndex))
            {
                MoveToNextPinGroup();
            }
            else
            {
                GameCompleted();
            }
        }
        else
        {
            // Move the player to the second throwing area if they've just completed their first throw
            if (!atSecondThrowingArea)
            {
                ShowPlayerNotification("You knocked some gorodki out. Moving you closer!", 7);
                MovePlayerToSecondThrowingArea();
            }
            else
            {
                PlayerManager.Instance.GetActivePlayer().SetCanThrow();
            }
        }
    }

    private IEnumerator ReturnToMenu(int timeout)
    {
        yield return new WaitForSeconds(timeout);
        GameManager.Instance.LoadScene("Menu");
    }

    private void GameCompleted()
    {
        string gameCompletexText = string.Format("Congratulations! You finished the game with a total of {0} throws. Well done!", PlayerManager.Instance.GetActivePlayer().GetTotalThrowCount());
        ShowPlayerNotification(gameCompletexText, 7);

        StartCoroutine(ReturnToMenu(3));
        // TODO: Game over! Show score!
        return;
    }

    private void ShowPlayerNotification(string text, int timeout)
    {
        PlayerManager.Instance.GetActivePlayer().playerUI.ShowTextOnScreen(text, timeout);
    }

    // Return the name of the prefab without the sorting number (i.e. return "Cannon" for prefab "1 Cannon")
    private string GetPrefabName(GameObject prefab)
    {
        string name = prefab.name;
        string[] tokens = name.Split(' ');
        string nameWithoutNumber = "";
        for (int i = 1; i < tokens.Length; i++)
        {
            nameWithoutNumber += tokens[i] + " ";
        }
        return nameWithoutNumber.Trim();
    }
}