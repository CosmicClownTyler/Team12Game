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
    }
}