using Cinemachine.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCamera : MonoBehaviour
{
    [Header("References")]
    public Transform orientation;
    public Transform player;
    public Transform playerObject;
    public Rigidbody rb;
    public float rotationSpeed;
    public Text notificationText;

    public Transform shootingLookAt;

    public enum CameraStyle
    {
        Exploration,
        Shooting
    }

    public CameraStyle currentStyle;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        currentStyle = CameraStyle.Shooting;
    }

    private void Update()
    {
        Vector3 viewDirection = player.position - new Vector3(transform.position.x, player.position.y, transform.position.z);
        orientation.forward = viewDirection.normalized;

        if (Input.GetKeyDown(KeyCode.C))
        {
            currentStyle = (currentStyle == CameraStyle.Exploration) ? CameraStyle.Shooting : CameraStyle.Exploration;
            StartCoroutine(sendNotification("Camera style: " + currentStyle.ToString(), 3));
        }


        if (currentStyle == CameraStyle.Exploration)
        {
            // vertical and horizontal inputs
            float hInput = Input.GetAxis("Horizontal");
            float vInput = Input.GetAxis("Vertical");

            Vector3 inputDirection = orientation.forward * vInput + orientation.right * hInput;

            // smoothen the movement
            if (inputDirection != Vector3.zero)
                playerObject.forward = Vector3.Slerp(playerObject.forward, inputDirection.normalized, Time.deltaTime * rotationSpeed);
        }
        else if (currentStyle == CameraStyle.Shooting)
        {
            // vertical and horizontal inputs
            float hInput = Input.GetAxis("Horizontal");
            float vInput = Input.GetAxis("Vertical");

            Vector3 directionToShootLokAt = shootingLookAt.position - new Vector3(transform.position.x, shootingLookAt.position.y, transform.position.z);
            orientation.forward = directionToShootLokAt.normalized;

            playerObject.forward = directionToShootLokAt.normalized;
        }

    }

    IEnumerator sendNotification(string text, int timeout)
    {
        notificationText.text = text;
        yield return new WaitForSeconds(timeout);
        notificationText.text = "";
    }
}   