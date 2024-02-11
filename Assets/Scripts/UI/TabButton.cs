using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class TabButton : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Tab")]
    // 
    public TabGroup tabGroup;
    public Tab tab;

    [Header("Colours")]
    // Colors for interaction
    public Color defaultColor = Color.white;
    public Color activeColor = new Color32(187, 187, 187, 255);
    [Range(0, 1)] public float hoverMultiplier;

    // The background image
    private Image background;

    private void Awake()
    {
        background = GetComponent<Image>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        tabGroup.SelectTab(tab);
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        SetHoverColor(true);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        SetHoverColor(false);
    }

    // 
    public void SetHoverColor(bool isHover)
    {
        if (isHover)
        {
            background.color = MultiplyColor(background.color, hoverMultiplier);
        }
        else
        {
            background.color = DivideColor(background.color, hoverMultiplier);
        }
    }
    public void SetActiveColor(bool isActive)
    {
        if (isActive)
        {
            background.color = MultiplyColor(activeColor, hoverMultiplier);
        }
        else
        {
            background.color = defaultColor;
        }
    }
    // Use a different method when setting the initial active color,
    // because all subsequent active colors are the active color multiplied by the hover multiplier,
    // because the tab must be hovered over to be clicked
    public void SetDefaultActiveColor()
    {
        background.color = activeColor;
    }

    // Multiply the color by the float value, keeping full alpha
    public static Color MultiplyColor(Color color, float value)
    {
        return new Color(color.r * value, color.g * value, color.b * value, 1.0f);
    }
    // Divide the color by the float value, keeping full alpha
    public static Color DivideColor(Color color, float value)
    {
        return new Color(color.r / value, color.g / value, color.b / value, 1.0f);
    }
}