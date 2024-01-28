using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoleStopChecker : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        Rigidbody rb = transform.gameObject.GetComponent<Rigidbody>();

        if (rb.IsSleeping())
        {
            GameObject.Find("Player").GetComponent<PoleThrower>().resetThrowableTrigger();
            Destroy(transform.gameObject);
        }
    }
}
