using System.Collections.Generic;
using UnityEngine;

public class MenuPageGroup : MonoBehaviour
{
    // The list of pages for this menu group
    [HideInInspector]
    public List<MenuPage> menuPages;

    // The currently active page shown on screen
    private MenuPage activePage = null;

    // Subscribe a menu page to this group
    public void Subscribe(MenuPage page)
    {
        // Create the list if it does not exist
        if (menuPages == null)
        {
            menuPages = new List<MenuPage>();
        }

        // Add the new page and deactivate all elements in case any are active
        menuPages.Add(page);
        DeactivatePage(page);

        // Set the active page
        if (activePage == null && page.isDefaultPage)
        {
            ActivatePage(page);
        }
        // Show a warning that more than one page is marked as default
        else if (page.isDefaultPage)
        {
            Debug.LogWarning("Only one page should be marked as default. ", page);
        }
    }

    // Activate the selected page and deactivate the previous page if it existed
    public void SelectPage(MenuPage page)
    {
        // Do nothing if the selected page is the currently active page
        if (activePage != null && activePage == page)
        {
            return;
        }

        // Deactivate the currently active page if it exists
        if (activePage != null)
        {
            DeactivatePage(activePage);
        }

        // Activate the current page
        ActivatePage(page);
    }

    // Activate all child objects for this page
    private void ActivatePage(MenuPage page)
    {
        foreach (RectTransform rt in page.GetComponentInChildren<RectTransform>())
        {
            rt.gameObject.SetActive(true);
            activePage = page;
        }
    }

    // Deactivate all child objects for this page
    private void DeactivatePage(MenuPage page)
    {
        foreach (RectTransform rt in page.GetComponentInChildren<RectTransform>())
        {
            rt.gameObject.SetActive(false);
        }
    }
}