using UnityEngine;

public class AreaPinChecker : MonoBehaviour
{
    // The count of pins in this collider
    private int pinCount = 0;

    private void Update()
    {
        if (pinCount < 0)
        {
            Debug.LogWarning("There should never be a negative number of pins counted. ");
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Pin")
        {
            pinCount++;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Pin")
        {
            pinCount--;
        }
    }

    public bool ContainsPins()
    {
        if (pinCount == 0)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
}