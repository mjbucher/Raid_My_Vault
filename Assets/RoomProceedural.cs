using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[ExecuteInEditMode][System.Serializable]
public class RoomProceedural : MonoBehaviour 
{
	#region Variables
	#region Room Variables
	// Prefabs
	[Header("Room Blocks")]
	public GameObject floorPrefab;
	public GameObject wallPrefab;
	public GameObject enterancePrefab;

	#endregion

	#region Floor Variables
	[Header("Room Size")]
	// grid size
	[Range(1, 20)]
	public int roomWidth;// = 1;
	[Range(1, 20)]
	public int roomDepth;// = 1;

	public GameObject floorManager;

	#endregion

	#region Wall Variables
	[Header("North -------------------")]
	[Header("***** Walls and Entrances *****")]
	// North Wall Controls
	[SerializeField]
	public WallManager northWallManager;
	public bool enableNorthWall = true;
	GameObject[] northWalls;
	// placement, for enterances and size for each
	public bool enableNorthEnterance = false;
	[Range(-10, 10)]
	public int enteranceNorthOffset = 0;

	[Header("East --------------------")]
	// East wall Controls
	public bool enableEastWall = true;
	public WallManager eastWallManager;
	GameObject[] eastWalls; 
	public bool enteranceEast = false;
	[Range(-10, 10)]
	public int enteranceEastOffset = 0;

	[Header("South -------------------")]
	// South wall controls
	public bool enableSouthWall = true;
	public WallManager southWallManger;
	GameObject[] southWalls;
	public bool enteranceSouth = false;
	[Range(-10, 10)]
	public int enteranceSouthOffset = 0;

	[Header("West --------------------")]
	// West wall controls
	public bool enableWestWall = true;
	public WallManager westWallManager;
	GameObject[] westWalls;
	public bool enteranceWest = false;
	[Range(-10, 10)]
	public int enteranceWestOffset = 0;

	#endregion

	public GameObject[,] grid;
	public List<WallManager> allWallManagers = new List<WallManager>();
	#region Controls
	[Space(20)]
	[Header("Quick Controls")]
	public bool setToOne = false;
	public bool reset = false;
	public bool PerpetualGeneration = false;

	#endregion

	#endregion

	#region Main Logic
	void Awake ()
	{
		// set option to false on initial attachment
		reset = false;
		if (PerpetualGeneration == true)
		{
			PerpetualGeneration = false;
		}
		allWallManagers.Add(northWallManager);
		allWallManagers.Add(eastWallManager);
		allWallManagers.Add(southWallManger);
		allWallManagers.Add(westWallManager);
	}

	void Update ()
	{
		CheckControls();
	}

	void CheckControls ()
	{
		if (reset)
		{
			ClearAllManagers();
		}
		else if (setToOne)
		{
			ResetTo(1);
			setToOne = false;
		}
		else if (PerpetualGeneration)
		{
			GenerateRoom();
		}
		// wall enable
//		northWallManager.wallEnabled = enableNorthWall;
//		eastWallManager.wallEnabled = enableEastWall;
//		southWallManger.wallEnabled = enableSouthWall;
//		westWallManager.wallEnabled = enableWestWall;
//		// enterance enabled
//		northWallManager.enteranceEnabled = enableNorthEnterance;
	}

	void GenerateRoom ()
	{
		// delete old instances
		DeleteFloors();
		DeleteWalls();
		// update floor
		CheckFloorForUpdates();
		// update all walls
		CheckAllWallsForUpdates();
		// generics not working
		//UpdateAllWalls();
	} 

	#endregion

	#region Floor Functions	
	void CheckFloorForUpdates ()
	{
		grid = new GameObject[roomWidth, roomDepth];
		// generate the floors
		for (int x = 0; x < roomWidth; x++)
		{
			for (int z = 0; z < roomDepth; z++)
			{
				Vector3 spawnPos = transform.position + new Vector3(x, 0, z) + new Vector3(1, 0, 1);
				grid[x,z] = Instantiate(floorPrefab, spawnPos, Quaternion.identity) as GameObject;
				grid[x,z].transform.SetParent(floorManager.transform);
			}
		}
		ResizeFloorCollider();
	}

	void ResizeFloorCollider ()
	{
		BoxCollider col = floorManager.GetComponent<BoxCollider>();  
		col.center =  new Vector3 ((roomWidth / 2.0f) - 0.5f , 0.1f , (roomDepth / 2.0f) - 0.5f);
		col.size = new Vector3 (roomWidth, 0.2f, roomDepth);
	}

	void DeleteFloors ()
	{
		DestroyAllChildren(floorManager);
	}

	#endregion

	#region Wall Functions
	void CheckAllWallsForUpdates()
	{
//		foreach (WallManager _manager in allWallManagers)
//		{
//			if (_manager.wallEnabled == true )
//			{
//				_manager.ModifyWalls();
//				_manager.col.enabled = true;
//				_manager.ResizeCollider();
//			}
//			else
//			{
//				_manager.col.enabled = false;
//			}
//		}

		if (enableNorthWall)
		{
			northWallManager.ModifyWalls();
			northWallManager.GetComponent<BoxCollider>().enabled = true;
			northWallManager.ResizeCollider();
		}
		else 
		{
			northWallManager.GetComponent<BoxCollider>().enabled = false;
		}

		// check east wall
		if (enableEastWall)
		{
			eastWallManager.ModifyWalls();
			eastWallManager.GetComponent<BoxCollider>().enabled = true;
			eastWallManager.ResizeCollider();
		}
		else
		{
			eastWallManager.GetComponent<BoxCollider>().enabled = false;
		}

		// check soouth wall
		if (enableSouthWall)
		{
			southWallManger.ModifyWalls();
			southWallManger.GetComponent<BoxCollider>().enabled = true;
			southWallManger.ResizeCollider();
		}
		else
		{
			southWallManger.GetComponent<BoxCollider>().enabled = false;
		}

		// check west wall
		if (enableWestWall)
		{
			westWallManager.ModifyWalls();
			westWallManager.GetComponent<BoxCollider>().enabled = true;
			westWallManager.ResizeCollider();
		}
		else
		{
			westWallManager.GetComponent<BoxCollider>().enabled = false;
		}
	}

	#region Generic Walls

	#endregion

	#region Modify Walls
//	void ModifyNorthWalls ()
//	{
//		northWallManager.ModifyWalls();
//	}
//
//	void ModifySouthWalls ()
//	{
//		southWallManger.ModifyWalls();
//	}
//
//	void ModifyEastWalls ()
//	{
//		eastWallManager.ModifyWalls();
//	}
//
//	void ModifyWestWalls ()
//	{
//		westWallManager.ModifyWalls();
//	}
//
//	void SmartResizeWallCollider (WallManager _manager)
//	{
//		_manager.ResizeCollider();
//	}

	void DeleteWalls ()
	{
		DestroyAllChildren(northWallManager);
		DestroyAllChildren(eastWallManager);
		DestroyAllChildren(southWallManger);
		DestroyAllChildren(westWallManager);
	}
	#endregion

	#endregion

	#region Control Functions
	void ClearAllManagers ()
	{
		// disable generation
		PerpetualGeneration = false;
		// check for leftovers
		DestroyAllChildren(floorManager);
		DestroyAllChildren(northWallManager);
		DestroyAllChildren(eastWallManager);
		DestroyAllChildren(southWallManger);
		DestroyAllChildren(westWallManager);
		// Reset Doors
		ResetAllDoors(false);
		// reset wall Colliders
		ResetAllColliderState(true);
		// reset to basic
		ResetTo(3);
		// re-enable generation
		PerpetualGeneration = true;
		// reset bool
		//clearAll = false;
	}

	void DestroyAllChildren (WallManager _manager)
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
	}

	void DestroyAllChildren (GameObject _parent)
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

	void ResetTo (int _num)
	{
		roomWidth = _num;
		roomDepth = _num;
		GenerateRoom();
		reset = false;
		Debug.Log("Proceedural Room Reset!");
	}

	void ResetAllColliderState (bool _state)
	{
		enableNorthWall = _state;
		enableWestWall = _state;
		enableSouthWall = _state;
		enableEastWall = _state;
	}

	void ResetAllDoors (bool _state)
	{
		enableNorthEnterance = _state;
		enteranceNorthOffset = 0;
		enteranceWest = _state;
		enteranceWestOffset = 0;
		enteranceEast = _state;
		enteranceEastOffset = 0;
		enteranceSouth = _state;
		enteranceSouthOffset = 0;
	}

	#endregion
}
