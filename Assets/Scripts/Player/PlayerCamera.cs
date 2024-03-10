using System.Collections;
using TMPro;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [Header("References")]
    public Transform orientation;
    public Transform playerObject;
    public Rigidbody rb;
    public float rotationSpeed;
    public TextMeshProUGUI notificationText;

    public Transform shootingLookAt;

    private Vector2 moveInput;
    private bool shouldChangeCamera = false;
    private bool canChangeCamera = true;

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
        shouldChangeCamera = false;

        if (InputManager.Instance.CameraChangeWasPressed)
        {
            shouldChangeCamera = true;
        }
        else if (InputManager.Instance.CameraChangeWasReleased)
        {
            canChangeCamera = true;
        }

        if (shouldChangeCamera && canChangeCamera)
        {
            ChangeCamera();
            canChangeCamera = false;
        }

        Vector3 viewDirection = playerObject.position - new Vector3(transform.position.x, playerObject.position.y, transform.position.z);
        orientation.forward = viewDirection.normalized;

        if (currentStyle == CameraStyle.Exploration)
        {
            Vector3 inputDirection = orientation.forward * moveInput.y + orientation.right * moveInput.x;

            // smoothen the movement
            if (inputDirection != Vector3.zero)
            {
                playerObject.forward = Vector3.Slerp(playerObject.forward, inputDirection.normalized, Time.deltaTime * rotationSpeed);
            }
        }
        else if (currentStyle == CameraStyle.Shooting)
        {
            Vector3 directionToShootLookAt = shootingLookAt.position - new Vector3(transform.position.x, shootingLookAt.position.y, transform.position.z);
            orientation.forward = directionToShootLookAt.normalized;

            playerObject.forward = directionToShootLookAt.normalized;
        }
    }

    private void ChangeCamera()
    {
        currentStyle = (currentStyle == CameraStyle.Exploration) ? CameraStyle.Shooting : CameraStyle.Exploration;
        StartCoroutine(SendNotification(notificationText, "Camera style: " + currentStyle.ToString(), 3));
    }

    private IEnumerator SendNotification(TextMeshProUGUI textHolder, string text, int timeout)
    {
        textHolder.text = text;
        yield return new WaitForSeconds(timeout);
        textHolder.text = "";
    }
}