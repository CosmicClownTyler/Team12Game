using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Diagnostics;

public class GameLogic : MonoBehaviour
{
    [Header("Gorod Prefabs")]
    public GameObject[] GorodPrefabsArray;

    public const string MovementConstraintInvisibleWallTag = "MovementConstraint";
    public const string GameAreaObjectName = "GameLogicArea";
    public const string GameAreaTag = "GameContactArea";
    public const string UntaggedAreaTag = "Untagged";
    public const string ThrowingPoleTag = "Pole";

    // Data:
    private int totalThrows;
    private GameObject currentObject;
    private int currentObjectIndex;

    private bool playerForward = false;
    private bool playerBack = true;

    // Start is called before the first frame update
    void Start()
    {
        totalThrows = 0;
        currentObject = null;
        currentObjectIndex = 0;
        spawnNextObject();
    }

    public void spawnNextObject()
    {
        if (currentObject != null)
        {
            Destroy(currentObject);
            movePlayerBack();
        }


        //var goPrefab = GetPrefabByTag("GO" + currentObjectIndex);
        var goPrefab = GetPrefabByArray(currentObjectIndex);
        if (goPrefab != null)
        {
            GameObject prefab = Instantiate(goPrefab, transform.position, transform.rotation);
            currentObject = prefab;
            currentObjectIndex++;
            UnityEngine.Debug.Log("Object spawned: " + currentObjectIndex);
        }
        else
        {
            UnityEngine.Debug.Log("Can't find the right prefab under number " + currentObjectIndex);
        }
    }

    public void movePlayerFurther()
    {
        if (playerForward)
            return;

        if (getTotalThrows() > 0)
        {
            triggerCollisionContrains();

            UnityEngine.Debug.Log("Moving further");
            Vector3 move = new Vector3(0, 0, -26f);

            // TODO: move player closer (as per game rules)

            playerBack = false;
            playerForward = true;

            triggerCollisionContrains();
        }
    }

    public void movePlayerBack()
    {
        if (playerBack)
            return;

        if (getTotalThrows() > 0)
        {
            triggerCollisionContrains();

            UnityEngine.Debug.Log("Moving back");
            Vector3 move = new Vector3(0, 0, 26f);

            // TODO: move player further away (as per game rules)

            playerBack = true;
            playerForward = false;

            triggerCollisionContrains();
        }
    }

    /*
     * This method removes box colliders from the invisible wall that stops player from moving player closer (as per rules)
     */

    public void triggerCollisionContrains()
    {
        // TODO: implement invisible walls
        return;

        GameObject[] constraints;
        constraints = GameObject.FindGameObjectsWithTag(MovementConstraintInvisibleWallTag);

        foreach (GameObject constraint in constraints)
        {
            constraint.GetComponent<BoxCollider>().enabled = !constraint.GetComponent<BoxCollider>().enabled;
        }
    }

    GameObject GetPrefabByTag(string name)
    {
        GameObject go = Resources.Load<GameObject>("Prefabs/Objects/Gorod Cities/" + name);

        return go;
    }
    GameObject GetPrefabByArray(int index)
    {
        GameObject go = GorodPrefabsArray[index];

        return go;
    }

    public void increaseThrows()
    {
        totalThrows++;
    }

    public int getTotalThrows()
    {
        return totalThrows;
    }

    public int getScoredGorodkov()
    {
        return currentObjectIndex - 1;
    }
}
