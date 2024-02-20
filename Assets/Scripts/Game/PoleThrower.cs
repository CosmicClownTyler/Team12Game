using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System.Diagnostics;

public class PoleThrower : MonoBehaviour
{
    [Header("Throwing Force")]
    private float throwForce;
    public float throwForceMinLimit;
    public float throwForceMaxLimit;

    public GameObject polePrefab;
    public GameObject gameLogic;
    public Transform orientation;
    public Text strenghtText;
    public Text throwsText;
    public AudioClip throwSoundClip;


    private bool startedThrow = false;
    private bool throwing = false;
    private bool throwableAgain = true;
    private bool throwForceDirectionUp = true;

    private void Start() {
        throwForce = throwForceMinLimit;
    }

    private void Update()
    {
        // If the throw button is currently held down
        if (throwing)
        {
            UpdateThrowForceSlider();
        }

        // If the throw button has just been released, throw the pole
        if (!throwing && startedThrow)
        {
            ThrowPole();
        }
    }

    // Update the throw force as the throw button is held down
    private void UpdateThrowForceSlider()
    {
        if (throwForceDirectionUp == true)
        {
            throwForce += (25 * Time.deltaTime);
        }
        else
        {
            throwForce -= (25 * Time.deltaTime);
        }

        if (throwForceDirectionUp == true && throwForce > throwForceMaxLimit)
        {
            throwForceDirectionUp = false;
        }
        if (throwForceDirectionUp == false && throwForce < throwForceMinLimit)
        {
            throwForceDirectionUp = true;
        }

        strenghtText.text =  "Throw strenght: " + throwForce;
    }
    // Apply the throw force and insantiate a new pole
    private void ThrowPole()
    {
        // Player should not be able to throw until after all objects have stopped moving (handled in CollisionProcessor.cs)
        throwableAgain = false;

        GameObject pole = Instantiate(polePrefab, transform.position, transform.rotation);
        pole.transform.position += new Vector3(1.0f, 0.0f, 1.0f);
        pole.transform.Rotate(0.0f, 0.0f, 90.0f, Space.Self);
        Rigidbody rb = pole.GetComponent<Rigidbody>();

        rb.angularVelocity = Vector3.down * 10.0f;
        rb.AddForce(orientation.transform.forward * throwForce, ForceMode.VelocityChange);
        rb.AddTorque(Vector3.down * 10000.0f);

        // Check if the AudioClip is assigned to avoid null reference exceptions
        if (throwSoundClip != null)
        {
            AudioManager.Instance.PlaySoundEffect(throwSoundClip);
        }
        else
        {
            UnityEngine.Debug.LogWarning("Throw sound clip not assigned.");
        }

        ResetAfterThrow();
    }
    // Reset back
    private void ResetAfterThrow()
    {
        gameLogic.GetComponent<GameLogic>().increaseThrows();
        throwForce = throwForceMinLimit;
        startedThrow = false;
        throwForceDirectionUp = true;
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
            throwsText.text = "Total Throws: " + gameLogic.GetComponent<GameLogic>().getTotalThrows();
            strenghtText.text = "Throw strenght: N/A";
        }
        
        // When the mouse button is held down for at least the set time (throw is started)
        if (context.phase == InputActionPhase.Performed/* &&  IsThrowable() */)
        {
            startedThrow = true;
            throwing = true;
        }
    }
}