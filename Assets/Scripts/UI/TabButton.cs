using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class TabButton : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    // Colors for interaction
    public Color hoverColor = Color.white;
    public Color clickColor = new Color();
    public Color activeColor;

    // The background image
    private Image background;



    // Start is called before the first frame update
    private void Start()
    {
        background = GetComponent<Image>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("click");
    }
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        background.color = Color.magenta;
    }
    
    public void OnPointerExit(PointerEventData eventData)
    {
        background.color = Color.white;
    }
}