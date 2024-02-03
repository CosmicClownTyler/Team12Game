using System.Collections.Generic;
using UnityEngine;

public class TabGroup : MonoBehaviour
{
    // The list of tabs for this group
    [HideInInspector]
    public List<Tab> tabs;

    // The currently active tab shown on screen
    private Tab activeTab = null;

    // Subscribe a tab to this group
    public void Subscribe(Tab tab)
    {
        // Create the list if it does not exist
        if (tabs == null)
        {
            tabs = new List<Tab>();
        }

        // Add the new tab and deactivate all elements in case any are active
        tabs.Add(tab);
        DeactivateTab(tab);

        // Set the active tab
        if (activeTab == null && tab.isDefaultTab)
        {
            ActivateTab(tab);
        }
        // Show a warning that more than one tab is marked as default
        else if (tab.isDefaultTab)
        {
            Debug.LogWarning("Only one tab should be marked as default. ", tab);
        }
    }

    // Activate the selected tab and deactivate the previous tab if it existed
    public void SelectTab(Tab tab)
    {
        // Do nothing if the selected tab is the currently active tab
        if (activeTab != null && activeTab == tab)
        {
            return;
        }

        // Deactivate the currently active tab if it exists
        if (activeTab!= null)
        {
            DeactivateTab(activeTab);
        }

        // Activate the current tab
        ActivateTab(tab);
    }

    // Activate all child objects for this tab
    private void ActivateTab(Tab tab)
    {
        foreach (RectTransform rt in tab.GetComponentInChildren<RectTransform>())
        {
            rt.gameObject.SetActive(true);
            activeTab = tab;
        }
    }

    // Deactivate all child objects for this tab
    private void DeactivateTab(Tab tab)
    {
        foreach (RectTransform rt in tab.GetComponentInChildren<RectTransform>())
        {
            rt.gameObject.SetActive(false);
        }
    }
}