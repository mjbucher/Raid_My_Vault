using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[ExecuteInEditMode]
[System.Serializable]
public class RoomProceedural : MonoBehaviour
{
    #region Variables
    #region Room Variables
    // Prefabs
    [Header("Room Prefabs")]
    public GameObject floorPrefab;
    public GameObject wallPrefab;
    public GameObject enterancePrefab;

    #endregion

    #region Wall Variables
    [Header("Managers")]
    public GameObject floorManager;
    public WallManager northWallManager;
    public WallManager eastWallManager;
    public WallManager southWallManger;
    public WallManager westWallManager;
    #endregion

    #region Floor Variables
    [Header("Room Size")]
    // grid size
    [Range(1, 20)]
    public int roomWidth;// = 1;
    [Range(1, 20)]
    public int roomDepth;// = 1;
    GameObject[,] grid;

    #endregion

    #region Restore Variables
    // floor
    int oldWidth;
    int oldDepth;
    // wall enables
    bool oldEnabledNorthWall;
    bool oldEnabledSouthWall;
    bool oldEnabledEastWall;
    bool oldEnabledWestWall;
    // enterance enabled
    bool oldEnteranceNorthWall;
    bool oldEnteranceSouthWall;
    bool oldEnteranceWestWall;
    bool oldEnteranceEastWall;
    // enterance offsets 
    int oldEnteranceNorthOffset;
    int oldEnteranceSouthOffset;
    int oldEnteranceEastOffset;
    int oldEnteranceWestOffset;
    #endregion

    #region Controls
    [Space(20)]
    //[Header("Quick Controls")]
    [HideInInspector]
    public bool setToOne = false;
    //[HideInInspector]
    //public bool reset = false;
    [HideInInspector]
    public bool PerpetualGeneration;// = false;

    #endregion
    #endregion

    #region Main Logic
    void Update()
    {

        CheckControls();
    }

    void CheckControls()
    {
        if (PerpetualGeneration)
        {
            GenerateRoom();
        }
    }

    public void GenerateRoom()
    {
        // delete old instances
        DeleteFloors();
        //DeleteWalls();
        // update floor
        CheckFloorForUpdates();
        // update all walls
        //CheckAllWallsForUpdates();
        // generics not working
        //UpdateAllWalls();
    }

    #endregion

    #region Floor Functions	
    void CheckFloorForUpdates()
    {
        grid = new GameObject[roomWidth, roomDepth];
        // generate the floors
        for (int x = 0; x < roomWidth; x++)
        {
            for (int z = 0; z < roomDepth; z++)
            {
                Vector3 spawnPos = transform.position + new Vector3(x, 0, z) + new Vector3(1, 0, 1);
                grid[x, z] = Instantiate(floorPrefab, spawnPos, Quaternion.identity) as GameObject;
                grid[x, z].transform.SetParent(floorManager.transform);
            }
        }
        //set wall manager positons
        northWallManager.transform.position = new Vector3(1, 0, roomDepth + 1) + transform.position;
        eastWallManager.transform.position = new Vector3(roomWidth + 1, 0, roomDepth) + transform.position;
        southWallManger.transform.position = new Vector3(roomWidth, 0, 0) + transform.transform.position;
        westWallManager.transform.position = new Vector3(0, 0, 1) + transform.position;

        ResizeFloorCollider();
    }

    void ResizeFloorCollider()
    {
        BoxCollider col = floorManager.GetComponent<BoxCollider>();
        col.center = new Vector3((roomWidth / 2.0f) - 0.5f, 0.1f, (roomDepth / 2.0f) - 0.5f);
        col.size = new Vector3(roomWidth, 0.2f, roomDepth);
    }

    void DeleteFloors()
    {
        DestroyAllChildren(floorManager);
    }

    #endregion

    #region Control Functions
    public void ClearAllManagers()
    {
        // disable generation
        //bool tempGen = PerpetualGeneration;
        //PerpetualGeneration = false;
        // check for leftovers
        //DestroyAllChildren(floorManager);
        //DestroyAllChildren(northWallManager);
        //DestroyAllChildren(eastWallManager);
        //DestroyAllChildren(southWallManger);
        //DestroyAllChildren(westWallManager);
        //Debug.Log("destoryed all children");
        // Reset Doors
        //ResetAllDoors(false);
        //Debug.Log("reset all doors");
        //ResetTo(3);
        //GenerateRoom();
        //PerpetualGeneration = tempGen;
        //Debug.Log(PerpetualGeneration);
        // reset bool
        //clearAll = false;
    }

    void DestroyAllChildren(WallManager _manager)
    {
        GameObject _parent = _manager.geometrySpawner;
        Transform[] childrenT = _parent.GetComponentsInChildren<Transform>();
        List<GameObject> childrenG = new List<GameObject>();
        foreach (Transform child in childrenT)
        {
            childrenG.Add(child.gameObject);
        }
        // remove parent of list (manager)
        childrenG.Remove(_parent);
        foreach (GameObject child in childrenG)
        {
            DestroyImmediate(child);
        }
        // deestroy any enterances
        _parent = _manager.enteranceSpawnPoint.gameObject;
        childrenT = _parent.GetComponentsInChildren<Transform>();
        childrenG = new List<GameObject>();
        foreach (Transform child in childrenT)
        {
            childrenG.Add(child.gameObject);
        }
        // remove parent of list (manager)
        childrenG.Remove(_parent);
        foreach (GameObject child in childrenG)
        {
            DestroyImmediate(child);
        }

    }

    void DestroyAllChildren(GameObject _parent)
    {
        //GameObject _parent = _manager.geometrySpawner;
        Transform[] childrenT = _parent.GetComponentsInChildren<Transform>();
        List<GameObject> childrenG = new List<GameObject>();
        foreach (Transform child in childrenT)
        {
            childrenG.Add(child.gameObject);
        }
        // remove parent of list (manager)
        childrenG.Remove(_parent);
        foreach (GameObject child in childrenG)
        {
            DestroyImmediate(child);
        }
    }

    void ResetTo(int _num)
    {
        roomWidth = _num;
        roomDepth = _num;
        //GenerateRoom();
        //reset = false;
        //Debug.Log("Proceedural Room Reset!");
    }

    void ResetAllColliderState(bool _state)
    {
        //enableNorthWall = _state;
        //enableWestWall = _state;
        //enableSouthWall = _state;
        //enableEastWall = _state;
    }

    void ResetAllDoors(bool _state)
    {
        //enableNorthEnterance = _state;
        //enteranceNorthOffset = 0;
        //enteranceWest = _state;
        //enteranceWestOffset = 0;
        //enteranceEast = _state;
        //enteranceEastOffset = 0;
        //enteranceSouth = _state;
        //enteranceSouthOffset = 0;
    }

    public void Reload()
    {
        // grab restore values
        #region Grab Values
        // floor values
        //oldWidth = roomWidth;
        //oldDepth = roomDepth;
        //// wall enables
        //oldEnabledNorthWall = enableNorthWall;
        //oldEnabledSouthWall = enableSouthWall;
        //oldEnabledEastWall = enableEastWall;
        //oldEnabledWestWall = enableWestWall;
        //// enterance enabled
        //oldEnteranceNorthWall = enableNorthEnterance;
        //oldEnteranceSouthWall = enteranceSouth;
        //oldEnteranceWestWall = enteranceWest;
        //oldEnteranceEastWall = enteranceEast;
        //// enterance offsets 
        //oldEnteranceNorthOffset = enteranceNorthOffset;
        //oldEnteranceSouthOffset = enteranceSouthOffset;
        //oldEnteranceEastOffset = enteranceEastOffset;
        //oldEnteranceWestOffset = enteranceWestOffset;
        //#endregion
        //// reset
        //ClearAllManagers();
        //// assign new values
        //#region Assin Values
        //// floor values
        //roomWidth = oldWidth;
        //roomDepth = oldDepth;
        //// wall enables
        //enableNorthWall = oldEnabledNorthWall;
        //enableSouthWall = oldEnabledSouthWall;
        //enableEastWall = oldEnabledEastWall;
        //enableWestWall = oldEnabledWestWall;
        //// enterance enabled
        //enableNorthEnterance = oldEnteranceNorthWall;
        //enteranceSouth = oldEnteranceSouthWall;
        //enteranceWest = oldEnteranceWestWall;
        //enteranceEast = oldEnteranceEastWall;
        //// enterance offsets 
        //enteranceNorthOffset = oldEnteranceNorthOffset;
        //enteranceSouthOffset = oldEnteranceSouthOffset;
        //enteranceEastOffset = oldEnteranceEastOffset;
        //enteranceWestOffset = oldEnteranceWestOffset;
        #endregion
    }

    #endregion
}
