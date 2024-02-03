using UnityEngine;

public class MenuPage : MonoBehaviour
{
    // The menu group this page belongs to
    public MenuPageGroup pageGroup;

    // Whether this page is the default or not
    public bool isDefaultPage;

    // Start is called before the first frame update
    void Start()
    {
        pageGroup.Subscribe(this);
    }
}