using System;
using UnityEngine;

[Serializable]
public enum GamePlayers
{
    One = 0,
    Two = 1,
}

public enum GameType
{
    Simple = 0,
    Basic = 1,
    Intermidiate = 2,
    Full = 3,
}

public class GameTypeManager : MonoBehaviour
{
    // Return a list of indices used to determine which pin groups to spawn for this game type
    public static int[] GetPinGroupList(GameType type)
    {
        if (type == GameType.Basic)
        {
            return new int[] { 0, 4, 7, 10, 12 };
        }
        else if (type == GameType.Intermidiate)
        {
            return new int[] { 0, 1, 3, 5, 7, 9, 10, 11, 12 };
        }
        else if (type == GameType.Full)
        {
            return new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14 };
        }
        else
        {
            return new int[] { 0 };
        }
    }

    // Return the number of pin groups this game type has
    public static int GetPinGroupCount(GameType type)
    {
        return GetPinGroupList(type).Length;
    }

    // Return true if this game has a next pin group
    public static bool HasNextPinGroup(GameType type, int currentPinGroupIndex)
    {
        int pinGroupCount = GetPinGroupCount(type);

        if (currentPinGroupIndex < pinGroupCount - 1)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}