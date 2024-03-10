using UnityEngine;

public class WoodCollisionSound : MonoBehaviour
{
    public AudioClip collisionSoundClip;
    
    // Flag to control sound playing
    private bool canPlaySound = false;

    private void Start()
    {
        // Delay sound enabling to avoid initial collisions playing sound
        Invoke("EnableSound", 0.5f); // Adjust the delay as needed
    }

    // Method to enable sound playing
    private void EnableSound()
    {
        canPlaySound = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Check if collision is with a pin or a bat and sound is allowed to play
        if ((collision.gameObject.CompareTag("Pin") || collision.gameObject.CompareTag("Bat")) && canPlaySound)
        {
            AudioManager.Instance?.PlaySoundEffect(collisionSoundClip);
        }
    }
    
    // Call this when resetting or destroying the bat to prevent sounds from playing
    private void OnDisable()
    {
        canPlaySound = false;
    }
}