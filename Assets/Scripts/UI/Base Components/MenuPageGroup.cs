using System.Collections.Generic;
using UnityEngine;

public class MenuPageGroup : MonoBehaviour
{
    // The list of pages for this menu group
    [HideInInspector]
    public List<MenuPage> menuPages;

    // The currently active page shown on screen
    private MenuPage activePage = null;

    private bool hasDefaultPage = false;

    // Subscribe a menu page to this group
    public void Subscribe(MenuPage page)
    {
        // Create the list if it does not exist
        if (menuPages == null)
        {
            menuPages = new List<MenuPage>();
        }

        // Add the new page
        menuPages.Add(page);

        // Set the default page as active
        if (page.isDefaultPage)
        {
            ActivatePage(page);
        }
        // Deactivate all elements in case any are active
        else
        {
            DeactivatePage(page);
        }

        // Show a warning if there are more than one default pages
        if (page.isDefaultPage && !hasDefaultPage)
        {
            hasDefaultPage = true;
        }
        else if (page.isDefaultPage && hasDefaultPage)
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

    private void ActivatePage(MenuPage page)
    {
        page.ActivatePage();
        activePage = page;
    }
    private void DeactivatePage(MenuPage page)
    {
        page.DeactivatePage();
    }
}