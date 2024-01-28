using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildCollisionTrigger : MonoBehaviour
{
    private bool inMotion = false;
    private bool fullyStopped = false;
    private bool touchedGroundSurface = false;
    List<GameObject> currentCollisions = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Rigidbody rb = transform.gameObject.GetComponent<Rigidbody>();

        if (rb.IsSleeping() && inMotion)
        {
            fullyStopped = true;
            inMotion = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        fullyStopped = false;
        inMotion = true;
        transform.parent.gameObject.GetComponent<PrefabCollisionProcessor>().OnChildTriggerEntered(collision, transform.position);
        currentCollisions.Add(collision.gameObject);
    }

    void OnCollisionExit(Collision collision)
    {
        // Remove the GameObject collided with from the list.
        currentCollisions.Remove(collision.gameObject);
    }

    public bool isFullyStoppedInside()
    {
        return fullyStopped && isInsidePlayingArea() && touchedGroundSurface;
    }
    public bool isFullyStoppedOutside()
    {
        return fullyStopped && !isInsidePlayingArea() && touchedGroundSurface;
    }

    public bool isInsidePlayingArea()
    {
        foreach (GameObject gObject in currentCollisions)
        {
            if (gObject != null)
            {
                if (gObject.tag == GameLogic.GameAreaTag)
                {
                    touchedGroundSurface = true;
                    return true;
                }

                // just check if we are touching SOME ground with every stick of the gorodok. Sometimes they are on top of each other and it counts as outside, hence the hacky check
                if (gObject.tag != GameLogic.UntaggedAreaTag)
                {
                    Debug.Log("Something touched " + gObject.tag);
                    touchedGroundSurface = true;
                }
            }
        }

        return false;
    }
}
