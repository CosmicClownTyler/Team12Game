using UnityEngine;

public class PinGroupStopChecker : MonoBehaviour
{
    private PinStopChecker[] stopCheckers;

    private void Start()
    {
        stopCheckers = GetComponentsInChildren<PinStopChecker>();
    }

    // Return true if all the child pins are stopped; otherwise false. 
    public bool IsStopped()
    {
        foreach (PinStopChecker checker in stopCheckers)
        {
            if (!checker.IsStopped())
            {
                return false;
            }
        }

        return true;
    }
}