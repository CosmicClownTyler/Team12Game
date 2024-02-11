using UnityEngine;
using UnityEngine.UIElements;

public class Tab : MonoBehaviour
{
    // The menu group this tab belongs to
    public TabGroup tabGroup;
    public TabButton tabButton;

    // Whether this tab is the default or not
    public bool isDefaultTab;

    private void Start()
    {
        tabGroup.Subscribe(this);
    }

    public void ActivateDefaultTab()
    {
        tabButton.SetDefaultActiveColor();

        foreach (RectTransform rt in GetComponentInChildren<RectTransform>())
        {
            rt.gameObject.SetActive(true);
        }
    }
    public void ActivateTab()
    {
        tabButton.SetActiveColor(true);

        foreach (RectTransform rt in GetComponentInChildren<RectTransform>())
        {
            rt.gameObject.SetActive(true);
        }
    }

    public void DeactivateTab()
    {
        tabButton.SetActiveColor(false);

        foreach (RectTransform rt in GetComponentInChildren<RectTransform>())
        {
            rt.gameObject.SetActive(false);
        }
    }
}