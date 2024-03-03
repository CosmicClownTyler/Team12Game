using UnityEngine;

public class MenuPage : MonoBehaviour
{
    // The menu group this page belongs to
    public MenuPageGroup pageGroup;

    // Whether this page is the default or not
    public bool isDefaultPage;

    private void Start()
    {
        pageGroup.Subscribe(this);

        if (isDefaultPage)
        {
            ActivatePage();
        }
        else
        {
            DeactivatePage();
        }
    }

    // Activate all child objects for this page
    public void ActivatePage()
    {
        foreach (RectTransform rt in GetComponentInChildren<RectTransform>())
        {
            rt.gameObject.SetActive(true);
        }
    }

    // Deactivate all child objects for this page
    public void DeactivatePage()
    {
        foreach (RectTransform rt in GetComponentInChildren<RectTransform>())
        {
            rt.gameObject.SetActive(false);
        }
    }
}