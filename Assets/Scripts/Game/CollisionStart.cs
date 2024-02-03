using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionStart : MonoBehaviour
{
    Rigidbody[] rigidbodies;

    // Start is called before the first frame update
    void Start()
    {
        rigidbodies = GetComponentsInChildren<Rigidbody>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Bat") {
            
            foreach (Rigidbody rb in rigidbodies) {
                rb.isKinematic = false;
            }
        }
    }
}