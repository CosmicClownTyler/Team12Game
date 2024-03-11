using System.Runtime.CompilerServices;
using UnityEngine;

public class PlayerThrower : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject batPrefab;

    [Header("Audio")]
    public AudioClip throwSoundClip;

    [Header("Throwing Force")]
    private float throwForce;
    public float throwForceMinLimit = 50;
    public float throwForceMaxLimit = 100;
    [Range(5, 50)]
    public int throwForceSlideSpeed = 15;
    private Transform aimingTransform;

    private bool startedThrow = false;
    private bool throwing = false;
    private bool throwForceDirectionUp = true;
    // Internal can throw is true if the previous bat has been deleted and the player is ready to throw again
    private bool internalCanThrow = true;
    // External can throw is true if the pins have stopped moving and the environment is ready for the player to throw again
    private bool externalCanThrow = true;

    // Active physics game objects
    private GameObject activeBat = null;

    // Throw stats
    private int throwCountTotal = 0;
    private int throwCountCurrentPinGroup = 0;

    private void Start()
    {
        throwForce = throwForceMinLimit;
    }

    private void Update()
    {
        // When the throw is finished
        if (InputManager.Instance.ThrowWasReleased && throwing)
        {
            throwing = false;
        }

        // When the throw is started
        if (InputManager.Instance.ThrowIsHeld && CanThrow())
        {
            startedThrow = true;
            throwing = true;
        }


        // If the throw button is currently held down
        if (throwing)
        {
            UpdateThrowForceSlider();
        }

        // If the throw button has just been released, throw the bat
        if (!throwing && startedThrow)
        {
            ThrowBat();
        }

        // If there is no active bat set the toggle
        if (activeBat == null && !internalCanThrow)
        {
            internalCanThrow = true;
        }
    }

    // Update the throw force as the throw button is held down
    private void UpdateThrowForceSlider()
    {
        // Increase or decrease the throw force
        if (throwForceDirectionUp == true)
        {
            throwForce += (throwForceSlideSpeed * Time.deltaTime);
        }
        else
        {
            throwForce -= (throwForceSlideSpeed * Time.deltaTime);
        }

        // Flip the direction when the throw force reaches the minimum or maximum
        if (throwForceDirectionUp == true && throwForce > throwForceMaxLimit)
        {
            throwForceDirectionUp = false;
        }
        if (throwForceDirectionUp == false && throwForce < throwForceMinLimit)
        {
            throwForceDirectionUp = true;
        }
    }
    // Apply the throw force and insantiate a new bat
    private void ThrowBat()
    {
        // Player should not be able to throw until the bat is deleted and all pins have stopped moving
        internalCanThrow = false;
        externalCanThrow = false;
        
        // Increment the throw counts
        throwCountTotal++;
        throwCountCurrentPinGroup++;

        // Instantiate a new bat and add the force
        activeBat = Instantiate(batPrefab, transform.position, transform.rotation);
        activeBat.transform.position += aimingTransform.forward;
        activeBat.transform.Rotate(0.0f, 0.0f, 90.0f, Space.Self);
        Rigidbody rb = activeBat.GetComponent<Rigidbody>();

        rb.angularVelocity = Vector3.down * 10.0f;
        rb.AddForce(aimingTransform.forward * throwForce, ForceMode.VelocityChange);
        rb.AddTorque(Vector3.down * 10000.0f);

        AudioManager.Instance?.PlaySoundEffect(throwSoundClip);

        ResetAfterThrow();
    }

    private void ResetAfterThrow()
    {
        throwForce = throwForceMinLimit;
        startedThrow = false;
        throwForceDirectionUp = true;
    }

    // Set the aiming direction
    public void SetAimingDirection(Transform aim)
    {
        aimingTransform = aim;
    }
    // Whether the player can throw the bat or not
    public bool CanThrow()
    {
        return internalCanThrow && externalCanThrow;
    }
    // Sets the environment as ready for the player to throw
    public void SetCanThrow()
    {
        externalCanThrow = true;
    }
    // Whether or not the thrower is waiting on the bat or the pins to be able to throw again
    public bool WaitingOnBat()
    {
        return !internalCanThrow;
    }
    public bool WaitingOnPins()
    {
        return !externalCanThrow;
    }
    // Cancels the throw by resetting all flags and force values
    public void CancelThrow()
    {
        startedThrow = false;
        throwing = false;
        ResetAfterThrow();
    }
    
    public bool IsThrowing()
    {
        return throwing;
    }
    public float GetThrowForce()
    {
        return throwForce;
    }
    public int GetTotalThrowCount()
    {
        return throwCountTotal;
    }
    public int GetCurrentThrowCount()
    {
        return throwCountCurrentPinGroup;
    }
    public void ResetCurrentThrowCount()
    {
        throwCountCurrentPinGroup = 0;
    }
}