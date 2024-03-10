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
        if (stopCheckers == null)
        {
            Debug.LogWarning("The pins haven't loaded yet. ");
            return false;
        }

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

//using UnityEngine;

//public class PinGroupStopChecker : MonoBehaviour
//{
//    private PinStopChecker[] stopCheckers;

//    private bool stopped = true;
//    private float stoppedTimer = 0;

//    private void Start()
//    {
//        stopCheckers = GetComponentsInChildren<PinStopChecker>();
//    }

//    private void Update()
//    {
//        if (stopCheckers == null)
//        {
//            Debug.LogWarning("The pins haven't loaded yet. ");
//            return;
//        }

//        foreach (PinStopChecker checker in stopCheckers)
//        {
//            if (!checker.IsStopped())
//            {
//                stopped = false;
//                stoppedTimer = 0;
//                return;
//            }
//        }

//        stoppedTimer += Time.deltaTime;

//        if (stoppedTimer > 2)
//        {
//            stopped = true;
//        }
//    }

//    // Return true if all the child pins are stopped; otherwise false. 
//    public bool IsStopped()
//    {
//        //if (stopCheckers == null)
//        //{
//        //    Debug.LogWarning("The pins haven't loaded yet. ");
//        //    return false;
//        //}

//        //foreach (PinStopChecker checker in stopCheckers)
//        //{
//        //    if (!checker.IsStopped())
//        //    {
//        //        return false;
//        //    }
//        //}

//        //return true;
//        return stopped;
//    }
//}