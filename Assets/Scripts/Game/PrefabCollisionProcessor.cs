using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabCollisionProcessor : MonoBehaviour
{
    const int MAX_PIECES = 5;

    private bool checkForStop = false;
    
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        int stoppedCountInside = 0, stoppedCountOutside = 0;

        Rigidbody rb = transform.gameObject.GetComponent<Rigidbody>();

        if (checkForStop)
        {
            foreach (Transform t in transform)
            {
                if (t.gameObject.GetComponent<ChildCollisionTrigger>().isFullyStoppedInside())
                {
                    stoppedCountInside++;
                }

                if (t.gameObject.GetComponent<ChildCollisionTrigger>().isFullyStoppedOutside())
                {
                    stoppedCountOutside++;
                }
            }
        }

        if ((stoppedCountInside + stoppedCountOutside) == MAX_PIECES && checkForStop)
        {
            if (stoppedCountInside == MAX_PIECES)
            {
                Debug.Log("WHOLE Gorodok stopped inside: " + stoppedCountInside);
            }
            else if (stoppedCountOutside == MAX_PIECES)
            {
                Debug.Log("WHOLE Gorodok stopped outside: " + stoppedCountOutside);
                GameObject.Find(GameLogic.GameAreaObjectName).GetComponent<GameLogic>().spawnNextObject();
            }
            else
            {
                Debug.Log("WHOLE Gorodok stopped in a mixed area: IN: " + stoppedCountInside + "; OUT: " + stoppedCountOutside);
                GameObject.Find(GameLogic.GameAreaObjectName).GetComponent<GameLogic>().movePlayerFurther();
            }

            // Make pole throwable again
            GameObject.Find("Player").GetComponent<PoleThrower>().ResetThrowableTrigger();

            // Reset stop checks.
            checkForStop = false;
        }

    }
    public void OnChildTriggerEntered(Collision other, Vector3 childPosition)
    {

        if (other.collider.tag == GameLogic.ThrowingPoleTag)
        {
            checkForStop = true;
            Debug.Log("Hit by a pole");
        }

    }
}
