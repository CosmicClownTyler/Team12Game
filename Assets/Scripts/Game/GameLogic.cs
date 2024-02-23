using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System.Diagnostics;
using System;
using System.Linq;
using UnityEngine.SceneManagement;

public class GameLogic : MonoBehaviour
{
    [Header("Gorod Prefabs")]
    public GameObject[] GorodPrefabsArray;

    public const string MovementConstraintInvisibleWallTag = "MovementConstraint";
    public const string GameAreaObjectName = "GameLogicArea";
    public const string GameAreaTag = "GameContactArea";
    public const string UntaggedAreaTag = "Untagged";
    public const string ThrowingPoleTag = "Pole";

    private const int INITIAL_FIGURE = -1;

    // Data:
    public Image IngameNotification;
    public Text CurrentGameObject;
    public GameObject playerObject;

    private int totalThrows;
    private int totalPerObjectThrows;
    private GameObject currentObject;
    private int currentObjectIndex;

    private bool playerForward = false;
    private bool playerBack = true;

    Dictionary<int, int[]> GameTypesList;

    public enum GameTypes {
        One = 0,
        Basic = 1,
        Intermidiate = 2,
        Full = 3
    }

    public GameTypes selectedGameType = GameTypes.One;

    // Start is called before the first frame update
    void Start()
    {
        // Disable IngameNotification if it is assigned
        if (IngameNotification != null)
        {
            IngameNotification.enabled = false;
        }
        else
        {
            UnityEngine.Debug.LogError("IngameNotification is not assigned!");
        }

        totalThrows = 0;
        totalPerObjectThrows = 0;
        currentObject = null;
        currentObjectIndex = INITIAL_FIGURE;

        GameTypesList = new Dictionary<int, int[]>
        {
            { (int)GameTypes.One, new int[]{0} },
            { (int)GameTypes.Basic, new int[]{0,4,7,10,12} },
            { (int)GameTypes.Intermidiate, new int[]{0,1,3,5,7,9,10,11,12} },
            { (int)GameTypes.Full, new int[]{0,1,2,3,4,5,6,7,8,9,10,11,12,13} }
        };

        spawnNextObject();
    }

    public void spawnNextObject()
    {        
        UnityEngine.Debug.Log("selectedGameType is " + (int)selectedGameType);
        int prefabId = this.getNextFigure(selectedGameType, currentObjectIndex);

        if (prefabId == INITIAL_FIGURE) {
            StartCoroutine(
                SendNotification(
                    IngameNotification,
                    String.Format(
                        "Congratulations! \n You finished the game with\n a total of {0} throws. \n Well done!", 
                        this.getTotalPerObjectThrows()
                    ), 
                    7
                )                
            );

            StartCoroutine(ReturnToMenu(3));
            // TODO: Game over! Show score!
            return;
        }

        if (currentObject != null)
        {
            Destroy(currentObject);
            movePlayerBack();
        }

        //var goPrefab = GetPrefabByTag("GO" + currentObjectIndex);
        var goPrefab = GetPrefabByArray(prefabId);
        if (goPrefab != null)
        {
            GameObject prefab = Instantiate(goPrefab, transform.position, transform.rotation);
            currentObject = prefab;
            currentObjectIndex++;
            UnityEngine.Debug.Log("Object spawned: " + prefabId);
        }
        else
        {
            UnityEngine.Debug.Log("Can't find the right prefab under number " + currentObjectIndex);
        }

        CurrentGameObject.text = "Current Figure: #" + currentObjectIndex;
    }

    public void movePlayerFurther()
    {
        if (playerForward)
            return;

        if (getTotalThrows() > 0)
        {
            StartCoroutine(
                SendNotification(
                    IngameNotification,
                    "You knocked some *gorodki* \n from the *city* playing area. \n Moving you closer to 6.5m", 
                    7
                )
            );
            UnityEngine.Debug.Log("Moving further");
            Vector3 move = new Vector3(0, 0, 10f);
            playerObject.transform.position += move;

            playerBack = false;
            playerForward = true;
        }
    }

    public void movePlayerBack()
    {
        if (playerBack) {
            StartCoroutine(
                SendNotification(
                    IngameNotification,
                    "Congratulations! \n You knocked the whole figure \n out of the *city* in one shot!", 
                    7
                )
            );
            return;
        }

        if (getTotalThrows() > 0)
        {
            StartCoroutine(
                SendNotification(
                    IngameNotification,
                    String.Format(
                        "Congratulations! \n You knocked the whole figure \n out of the *city* in {0} throws. \n Moving you back to 13m", 
                        this.getTotalPerObjectThrows()
                    ), 
                    7
                )
            );
            UnityEngine.Debug.Log("Moving back");
            Vector3 move = new Vector3(0, 0, -10f);
            playerObject.transform.position += move;

            playerBack = true;
            playerForward = false;
        }

        // Reset throws per figure
        totalPerObjectThrows = 0;
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
        totalPerObjectThrows++;
    }

    public int getTotalThrows()
    {
        return totalThrows;
    }


    public int getTotalPerObjectThrows()
    {
        return totalPerObjectThrows;
    }

    public int getScoredGorodkov()
    {
        return currentObjectIndex - 1;
    }

    private int getMaxFiguresPerGameplay(GameTypes type) {        
        return GameTypesList[(int)type].Count();
    }

    private int getNextFigure(GameTypes type, int current) {
        if ((current+1) == this.getMaxFiguresPerGameplay(type)) {
            return INITIAL_FIGURE;
        }
        return GameTypesList[(int)type][++current];
    }

    IEnumerator SendNotification(Image textHolder, string text, int timeout)
    {
        textHolder.enabled = true;
        Text notificationText = textHolder.gameObject.GetComponentInChildren<Text>();
        notificationText.text = text;
        yield return new WaitForSeconds(timeout);
        notificationText.text = "";
        textHolder.enabled = false;
    }  

    IEnumerator ReturnToMenu(int timeout)
    {
        yield return new WaitForSeconds(timeout);
        UnityEngine.Debug.Log("Loading main menu");
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        SceneManager.LoadScene("Menu", LoadSceneMode.Single);
    }  

}
