using UnityEngine;

public class PinStopChecker : MonoBehaviour
{
    private Rigidbody rb;

    private bool isStopped = false;
    
    private void Start()
    {
        rb = transform.gameObject.GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (rb.IsSleeping())
        {
            isStopped = true;
        }
        else
        {
            isStopped = false;
        }
    }

    public bool IsStopped()
    {
        return isStopped;
    }
}