using UnityEngine;

public class Tab : MonoBehaviour
{
    // The menu group this tab belongs to
    public TabGroup tabGroup;

    // Whether this tab is the default or not
    public bool isDefaultTab;

    // Start is called before the first frame update
    void Start()
    {
        tabGroup.Subscribe(this);
    }
}