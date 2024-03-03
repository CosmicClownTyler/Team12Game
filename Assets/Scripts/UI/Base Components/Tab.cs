using UnityEngine;

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

        SetActiveChildren(true);
    }
    public void ActivateTab()
    {
        tabButton.SetActiveColor(true);

        SetActiveChildren(true);
    }

    public void DeactivateTab()
    {
        tabButton.SetActiveColor(false);

        SetActiveChildren(false);
    }

    // Activate or deactivate all children recursively
    private void SetActiveChildren(bool state)
    {
        SetActiveChildren(gameObject.GetComponent<RectTransform>(), state);
    }
    private void SetActiveChildren(RectTransform rectTransform, bool state)
    {
        rectTransform.gameObject.SetActive(state);
        foreach (RectTransform rt in rectTransform.GetComponentInChildren<RectTransform>())
        {
            SetActiveChildren(rt, state);
        }
    }
}