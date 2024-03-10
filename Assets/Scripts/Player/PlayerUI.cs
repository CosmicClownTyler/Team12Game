using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerUI : MonoBehaviour
{
    public Canvas canvas;
    public TextMeshProUGUI currentPinGroupText;
    public TextMeshProUGUI throwForceText;
    public TextMeshProUGUI totalThrowCountText;
    public TextMeshProUGUI currentThrowCountText;
    public Image notificationImage;
    public TextMeshProUGUI notificationText;
    public TextMeshProUGUI waitingToThrowText;

    private bool wasNotificationActive = false;

    public bool IsWaitingAfterThrow { get; set; }

    private void Update()
    {
        if (IsWaitingAfterThrow)
        {
            waitingToThrowText.text = "waiting for pins to stop...";
        }
        else
        {
            waitingToThrowText.text = "";
        }
    }

    // UI methods
    private IEnumerator SendNotification(string text, int timeout)
    {
        notificationImage.enabled = true;
        notificationText.text = text;
        yield return new WaitForSeconds(timeout);
        notificationText.text = "";
        notificationImage.enabled = false;
    }
    public void ShowTextOnScreen(string text, int time)
    {
        StartCoroutine(SendNotification(text, time));
    }

    public void EnableUI()
    {
        canvas.enabled = true;
        notificationImage.enabled = wasNotificationActive;
    }
    public void DisableUI()
    {
        wasNotificationActive = notificationImage.enabled;
        canvas.enabled = false;
    }
}