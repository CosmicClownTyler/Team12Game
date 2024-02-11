using UnityEngine;
using UnityEngine.EventSystems;

public class PlaySoundOnClick : MonoBehaviour, IPointerUpHandler, IPointerExitHandler, IPointerDownHandler
{
    [SerializeField]
    private AudioClip audioClip;

    // Whether or not the button press was a valid
    private bool validClick;

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log(eventData.button);
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            validClick = true;
        }
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        if (validClick)
        {
            validClick = false;
        }
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        if (validClick)
        {
            AudioManager.Instance.PlayUISound(audioClip);
        }
    }
}