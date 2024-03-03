using UnityEngine;
using UnityEngine.EventSystems;

public class PlaySoundOnSlide : MonoBehaviour, IPointerUpHandler
{
    [SerializeField]
    private AudioClip audioClip;

    public void OnPointerUp(PointerEventData eventData)
    {
        AudioManager.Instance.PlayUISound(audioClip);
    }
}