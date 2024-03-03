using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    // Singleton
    public static PlayerManager Instance;
    
    // Player object references
    [HideInInspector] public GameObject player;
    //[HideInInspector] public GameObject playerTwo;
    //[HideInInspector] public GameObject playerThree;
    //[HideInInspector] public GameObject playerFour;

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

    public void EnablePlayer()
    {
        foreach (Transform t in player.transform)
        {
            t.gameObject.SetActive(true);
        }
    }
    public void DisablePlayer()
    {
        foreach (Transform t in player.transform)
        {
            t.gameObject.SetActive(false);
        }
    }

    public void EnablePlayerUI()
    {
        GameObject activePlayer = GameObject.FindWithTag("Player");
        Canvas canvas = activePlayer.GetComponentInChildren<Canvas>();
        canvas.enabled = true;
    }

    public void DisablePlayerUI()
    {
        GameObject activePlayer = GameObject.FindWithTag("Player");
        Canvas canvas = activePlayer.GetComponentInChildren<Canvas>();
        canvas.enabled = false;
    }
}