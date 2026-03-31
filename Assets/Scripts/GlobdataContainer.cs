using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GlobdataContainer : MonoBehaviour
{
    private static GlobdataContainer instance;
    private List<GameObject> gameObjectsToKeepAlive = new List<GameObject>();
    private PlayerInput[] playerInputs = new PlayerInput[4];

    private void Awake()
    {
        if(instance  == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public static GlobdataContainer GetInstance()
    {
        return instance; 
    }

    public static void DestroyInstance()
    {
        if(instance != null)
        {
            Destroy(instance.gameObject);
            instance = null;
        }
    }

    public static void KeepObjectAlive(GameObject gameObject)
    {
        if (instance != null)
        {
            DontDestroyOnLoad(gameObject);
            instance.gameObjectsToKeepAlive.Add(gameObject);
        }
    }

    public static void RegisterInputGlobal(GameObject gameobject, int slotID)
    {
        PlayerInput PI = gameobject.GetComponent<PlayerInput>();
        if (instance != null && PI != null)
        {
            gameobject.transform.SetParent(null);
            DontDestroyOnLoad(gameobject);
            KeepObjectAlive(gameobject);
            instance.playerInputs[slotID] = PI;
        }
    }

    public static void UnregisterInputGlobal(int slotID)
    {
        if (instance != null)
        {
            instance.playerInputs[slotID] = null;
        }
    }

    public static void ReleaseObjectFromGlobal(GameObject gameobject)
    {
        if (instance != null && gameobject != null)
        {
            instance.gameObjectsToKeepAlive.Remove(gameobject);
        }
    }

    //these get sent in as: ARM1, ARM2, ARM3 and ARM4
    //these are already ordered, not by player number but by arm, which means you can just use IDs 0-3 to get the correct input
    public static PlayerInput GetPlayerIDInput(int ID)
    {
        if (instance != null)
        {
            return instance.playerInputs[ID];
        }
        return null; 
    }

    private void OnDestroy()
    {
        //kill any data that's left behind
        foreach(var obj in gameObjectsToKeepAlive)
        {
            if (obj != null)
            {
                Destroy(obj);
            }
        }
    }
}
