using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class BatThrower : MonoBehaviour
{
    [Header("Throwing Force")]
    private float throwForce;
    public float throwForceMinLimit;
    public float throwForceMaxLimit;
    [Range(5, 50)]
    public int throwForceSlideSpeed = 15;

    public GameObject batPrefab;
    public TextMeshProUGUI forceText;
    public TextMeshProUGUI throwsText;
    public AudioClip throwSoundClip;

    private bool startedThrow = false;
    private bool throwing = false;
    private bool throwableAgain = true;
    private bool throwForceDirectionUp = true;

    // 
    private int throwCountTotal;
    private int throwCountCurrentPinGroup;

    private void Start()
    {
        throwForce = throwForceMinLimit;
    }

    private void Update()
    {
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

        // Update text fields
        throwsText.text = "Total Throws: " + throwCountTotal;
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

        // Format the force and update the text
        string formattedThrowForce = string.Format("{0:0}", throwForce);
        forceText.text =  "Throw force: " + formattedThrowForce;
    }
    // Apply the throw force and insantiate a new bat
    private void ThrowBat()
    {
        // Player should not be able to throw until after all objects have stopped moving (handled in CollisionProcessor.cs)
        throwableAgain = false;
        
        // Increment the throw counts
        throwCountTotal++;
        throwCountCurrentPinGroup++;

        // Instantiate a new bat and add the force
        GameObject bat = Instantiate(batPrefab, transform.position, transform.rotation);
        //bat.transform.position += new Vector3(1.0f, 0.0f, 1.0f);
        bat.transform.position += transform.forward;
        bat.transform.Rotate(0.0f, 0.0f, 90.0f, Space.Self);
        Rigidbody rb = bat.GetComponent<Rigidbody>();

        rb.angularVelocity = Vector3.down * 10.0f;
        rb.AddForce(transform.forward * throwForce, ForceMode.VelocityChange);
        rb.AddTorque(Vector3.down * 10000.0f);

        AudioManager.Instance?.PlaySoundEffect(throwSoundClip);

        ResetAfterThrow();
    }
    // Reset back
    private void ResetAfterThrow()
    {
        throwForce = throwForceMinLimit;
        startedThrow = false;
        throwForceDirectionUp = true;
    }

    public int GetTotalThrows()
    {
        return throwCountTotal;
    }
    public int GetCurrentThrows()
    {
        return throwCountCurrentPinGroup;
    }
    public void ResetCurrentThrows()
    {
        throwCountCurrentPinGroup = 0;
    }
    public float GetForceValue()
    {
        return throwForce;
    }
    public void ResetThrowableTrigger()
    {
        throwableAgain = true;
    }
    public bool IsThrowable()
    {
        return throwableAgain;
    }

    // events (subscribed to in inspector)
    public void OnThrow(InputAction.CallbackContext context)
    {
        // When the mouse button is pressed at all (not used)
        if (context.phase == InputActionPhase.Started)
        {
            
        }
        
        // When the mouse button is released (throw is finished)
        if (context.phase == InputActionPhase.Canceled)
        {
            throwing = false;
            forceText.text = "Throw force: N/A";
        }
        
        // When the mouse button is held down for at least the set time (throw is started)
        if (context.phase == InputActionPhase.Performed && IsThrowable())
        {
            startedThrow = true;
            throwing = true;
        }
    }
}