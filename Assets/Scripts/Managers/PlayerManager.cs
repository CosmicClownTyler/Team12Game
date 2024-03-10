using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    // Singleton
    public static PlayerManager Instance;

    // Player object references
    [HideInInspector] public GameObject playerOneObject;
    [HideInInspector] public Player playerOne;
    [HideInInspector] public GameObject playerTwoObject;
    [HideInInspector] public Player playerTwo;

    private Player activePlayerReference;

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

    public void SetActivePlayers()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        if (players.Length > 2 || players.Length <= 0)
        {
            Debug.LogWarning("Invalid number of player objects in scene. ");
        }
        else if (players.Length == 2)
        {
            playerTwoObject = players[1];
            playerTwo = playerTwoObject.GetComponent<Player>();
            DisablePlayer(playerTwo);
            DisablePlayerUI(playerTwo);
        }

        playerOneObject = players[0];
        playerOne = playerOneObject.GetComponent<Player>();
        EnablePlayer(playerOne);
        EnablePlayerUI(playerOne);
        activePlayerReference = playerOne;
    }
    public Player GetActivePlayer()
    {
        return activePlayerReference;
    }

    public void SwitchActivePlayer()
    {
        if (activePlayerReference == playerOne)
        {
            activePlayerReference = playerTwo;
        }
        else
        {
            activePlayerReference = playerOne;
        }
    }

    public void EnableActivePlayerUI()
    {
        EnablePlayerUI(GetActivePlayer());
    }
    public void DisableActivePlayerUI()
    {
        DisablePlayerUI(GetActivePlayer());
    }

    public void EnablePlayer(Player player)
    {
        foreach (Transform t in player.transform)
        {
            t.gameObject.SetActive(true);
        }
    }
    public void DisablePlayer(Player player)
    {
        foreach (Transform t in player.transform)
        {
            t.gameObject.SetActive(false);
        }
    }

    public void EnablePlayerUI(Player player)
    {
        player.playerUI.EnableUI();
    }
    public void DisablePlayerUI(Player player)
    {
        player.playerUI.DisableUI();
    }
}