using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCamera : MonoBehaviour
{
    [Header("References")]
    public Transform orientation;
    public Transform playerObject;
    public Rigidbody rb;
    public float rotationSpeed;
    public TextMeshProUGUI notificationText;

    public Transform shootingLookAt;

    private Vector2 input;

    public enum CameraStyle
    {
        Exploration,
        Shooting
    }

    public CameraStyle currentStyle;

    private void Start()
    {
        currentStyle = CameraStyle.Shooting;
    }

    private void Update()
    {
        Vector3 viewDirection = playerObject.position - new Vector3(transform.position.x, playerObject.position.y, transform.position.z);
        orientation.forward = viewDirection.normalized;

        if (currentStyle == CameraStyle.Exploration)
        {
            Vector3 inputDirection = orientation.forward * input.y + orientation.right * input.x;

            // smoothen the movement
            if (inputDirection != Vector3.zero)
                playerObject.forward = Vector3.Slerp(playerObject.forward, inputDirection.normalized, Time.deltaTime * rotationSpeed);
        }
        else if (currentStyle == CameraStyle.Shooting)
        {
            Vector3 directionToShootLookAt = shootingLookAt.position - new Vector3(transform.position.x, shootingLookAt.position.y, transform.position.z);
            orientation.forward = directionToShootLookAt.normalized;

            playerObject.forward = directionToShootLookAt.normalized;
        }

    }

    // input events (subscribed to in inspector)
    public void OnCameraChange(InputAction.CallbackContext context)
    {
        currentStyle = (currentStyle == CameraStyle.Exploration) ? CameraStyle.Shooting : CameraStyle.Exploration;
        StartCoroutine(SendNotification(notificationText,"Camera style: " + currentStyle.ToString(), 3));
    }
    public void OnMove(InputAction.CallbackContext context)
    {
        // read move input
        input = context.ReadValue<Vector2>();
    }

    private IEnumerator SendNotification(TextMeshProUGUI textHolder, string text, int timeout)
    {
        textHolder.text = text;
        yield return new WaitForSeconds(timeout);
        textHolder.text = "";
    }
}