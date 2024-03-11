using UnityEngine;

public class Player : MonoBehaviour
{
    public GameObject playerObject;
    [HideInInspector] public PlayerMovement playerMovement;
    [HideInInspector] public PlayerThrower playerThrower;
    public PlayerUI playerUI;

    private void Start()
    {
        playerMovement = playerObject.GetComponent<PlayerMovement>();
        playerThrower = playerObject.GetComponent<PlayerThrower>();
        
        // Disable the mesh renderer to avoid the capsule being seen in first person
        playerObject.GetComponent<MeshRenderer>().enabled = false;
    }

    private void Update()
    {
        // Update the throw count text
        playerUI.totalThrowCountText.text = "Total Throws: " + GetTotalThrowCount();
        playerUI.currentThrowCountText.text = "Current Throws: " + GetCurrentThrowCount();

        // Set the force text
        if (playerThrower.IsThrowing())
        {
            // Format and update the force text
            string formattedThrowForce = string.Format("{0:0}", playerThrower.GetThrowForce());
            playerUI.throwForceText.text = "Throw force: " + formattedThrowForce;
        }
        else
        {
            playerUI.throwForceText.text = "Throw force: N/A";
        }

        // Tells the UI whether the player can throw or not
        playerUI.IsWaitingAfterThrow = !playerThrower.CanThrow();

        // Update the aiming direction
        playerThrower.SetAimingDirection(playerMovement.playerCamera.transform);
    }

    public int GetTotalThrowCount()
    {
        return playerThrower.GetTotalThrowCount();
    }
    public int GetCurrentThrowCount()
    {
        return playerThrower.GetCurrentThrowCount();
    }
    public void ResetCurrentThrowCount()
    {
        playerThrower.ResetCurrentThrowCount();
    }

    public void SetCanThrow(bool canThrow = true)
    {
        playerThrower.SetCanThrow(canThrow);
    }
    public bool GetCanThrow()
    {
        return playerThrower.CanThrow();
    }
    public bool ThrowerWaitingOnBat()
    {
        return playerThrower.WaitingOnBat();
    }
    public bool ThrowerWaitingOnPins()
    {
        return playerThrower.WaitingOnPins();
    }

}