using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[ExecuteInEditMode]
public class FloorProceedural : MonoBehaviour 
{
	// Prefabs
	[Header("Room Blocks")]
	public GameObject floorPrefab;
	public GameObject wallPrefab;

	[Header("Room Size")]
	// grid size
	[Range(1, 20)]
	public int roomWidth;// = 1;
	[Range(1, 20)]
	public int roomDepth;// = 1;

	public GameObject floorManager;

	#region Walls
	[Header("-------------------")]
	[Header("Walls and Entrances")]
	// North Wall Controls
	public bool enableNorthWall = false;
	public GameObject northWallManager;
	GameObject[] northWalls;
	// placement, for enterances and size for each
	public bool enteranceNorth = false;
	[Range(1, 5)]
	public int NorthSize = 1;

	[Header("-------------------")]
	// East wall Controls
	public bool enableEastWall = false;
	public GameObject eastWallManager;
	GameObject[] eastWalls; 
	public bool enteranceEast = false;
	[Range(1, 5)]
	public int EastSize = 1;

	[Header("-------------------")]
	// South wall controls
	public bool enableSouthWall = false;
	public GameObject southWallManger;
	GameObject[] southWalls;
	public bool enteranceSouth = false;
	[Range(1, 5)]
	public int SouthSize = 1;

	[Header("-------------------")]
	// West wall controls
	public bool enableWestWall = false;
	public GameObject westWallManager;
	GameObject[] westWalls;
	public bool enteranceWest = false;
	[Range(1, 5)]
	public int WestSize = 1;

	[Header("-------------------")]
	int oldWidth;
	int oldDepth;
	#endregion

	public GameObject[,] grid;

	
	[Space(20)]
	[Header("Quick Controls")]
	public bool clearAll = false;
	public bool reset = false;
	public bool GenerateFloorsNow = false;
	public bool PerpetualGeneration = false;
	bool FloorGenerated; // = false;



	//public bool DeleteFloorsNow = false;
	void Awake ()
	{
		// set option to false
		reset = false;
		GenerateFloorsNow = false;
		PerpetualGeneration = false;

	}

	void Update ()
	{
//		if (clearAll)
//		{
//			ClearAllManagers();
//		}

		if (reset)
		{
			ResetToOne();
		}

		else if (PerpetualGeneration)
		{
			GenerateFloors();
		}

		else if (GenerateFloorsNow)
		{
			GenerateFloors();
			GenerateFloorsNow = false;
		}
//		if (DeleteFloorsNow)
//		{
//			DeleteFloors();
//			DeleteFloorsNow = false;
//		}


	}

	void GenerateFloors ()
	{
		// delete old
		DeleteFloors();
		// record most recent for next load
		oldWidth = roomWidth;
		oldDepth = roomDepth;

		/// ** issue is here I am creating a new grid (unlinking the old one) before it has been fully destroyed

		// set new grid size
		grid = new GameObject[roomWidth, roomDepth];
		// generate the floors
		for (int x = 0; x < roomWidth; x++)
		{
			for (int z = 0; z < roomDepth; z++)
			{
				grid[x,z] = Instantiate(floorPrefab, new Vector3(
					(transform.position.x + x /*- (roomWidth / 2)*/), 
					transform.position.y, 
					(transform.position.z + z /*- (roomLength / 2)*/)
				), Quaternion.identity) as GameObject;

				grid[x,z].transform.SetParent(floorManager.transform);
			}
		}
		ResizeFloorCollider();
		if (enableNorthWall)
		{
			ModifyNorthWalls();
			ResizeNorthWallCollider();
		}
		if (enableEastWall)
		{
			ModifyEastWalls();
			ResizeEastWallCollider();
		}
		if (enableSouthWall)
		{
			ModifySouthWalls();
			ResizeSouthWallCollider();
		}
		if (enableWestWall)
		{
			ModifyWestWalls();
			ResizeWestWallCollider();
		}
		FloorGenerated = true;
	}

	void DeleteFloors ()
	{
		if (grid != null) 
		{
			//Debug.Log("deleting floor / size of :" + oldWidth + " x " + oldDepth);
			for (int x = 0; x < oldWidth; x++)
			{
				for (int z = 0; z < oldDepth; z++)
				{
					DestroyImmediate(grid[x,z]);
				}
			}
		}
		if (northWalls != null)
		{
			for (int x = 0; x < northWalls.Length; x++)
			{
				DestroyImmediate(northWalls[x]);
			}
		}

		if (eastWalls != null)
		{
			for (int x = 0; x < eastWalls.Length; x++)
			{
				DestroyImmediate(eastWalls[x]);
			}
		}


		if (southWalls != null)
		{
			for (int x = 0; x < southWalls.Length; x++)
			{
				DestroyImmediate(southWalls[x]);
			}
		}

		if (westWalls != null)
		{
			for (int x = 0; x < westWalls.Length; x++)
			{
				DestroyImmediate(westWalls[x]);
			}
		}
			
	}

	void ResizeFloorCollider ()
	{
		BoxCollider col = floorManager.GetComponent<BoxCollider>();  
		col.center =  new Vector3 ((roomWidth / 2.0f) - 0.5f , 0.1f , (roomDepth / 2.0f) - 0.5f);
		col.size = new Vector3 (roomWidth, 0.2f, roomDepth);
	}



	void ResetToOne ()
	{
		roomWidth = 1;
		roomDepth = 1;
		GenerateFloors();
		reset = false;
	}

	#region Modify Walls
	void ModifyNorthWalls ()
	{
		// Init
		northWalls = new GameObject[roomWidth];
		// set spawners shorthand
		Vector3 spawnPos = northWallManager.transform.position;
		Vector3 spawnerOffset = new Vector3(0, 0, -0.5f);
		Vector3 northSpawnRotation = new Vector3(0, 90.0f, 0);
		// find highest z
		int z = roomDepth - 1;
		for (int x = 0; x < roomWidth; x++)
		{
			northWalls[x] = Instantiate(wallPrefab, (spawnPos + new Vector3(x, 0, z) + spawnerOffset), Quaternion.identity) as GameObject;
			northWalls[x].transform.SetParent(northWallManager.transform);
			// no rotation required
			northWalls[x].transform.Rotate(new Vector3(0, 90.0f, 0));
		}


	}

	void ModifySouthWalls ()
	{
		// Init
		southWalls = new GameObject[roomWidth];
		// set spwn shorthand
		Vector3 spawnPos = southWallManger.transform.position;
		Vector3 spawnerOffset = new Vector3 (0, 0, 0.5f);
		// find lowest z
		int z = 0;
		for (int x = 0; x < roomWidth; x++)
		{
			// spawn and grouping
			southWalls[x] = Instantiate(wallPrefab, (spawnPos + new Vector3(x, 0, z) + spawnerOffset), Quaternion.identity) as GameObject;
			southWalls[x].transform.SetParent(southWallManger.transform);
			// rotate to side
			southWalls[x].transform.Rotate(new Vector3(0, 90.0f, 0));
		}

	}

	void ModifyEastWalls ()
	{
		// Init
		eastWalls = new GameObject[roomDepth];
		// set spawn shorthand
		Vector3 spawnPos = eastWallManager.transform.position;
		Vector3 spawnerOffset = new Vector3(-0.5f, 0 ,0);
		// find highest x
		int x = roomWidth - 1 ;
		for (int z = 0; z < roomDepth; z++)
		{
			// spawn and grouping
			eastWalls[z] = Instantiate(wallPrefab, (spawnPos + new Vector3(x, 0, z) + spawnerOffset), Quaternion.identity) as GameObject;
			eastWalls[z].transform.SetParent(eastWallManager.transform);
			// rotate to side
			//eastWalls[z].transform.Rotate(new Vector3(0, 0.0f, 0));
		}
	}

	void ModifyWestWalls ()
	{
		// Init
		westWalls = new GameObject[roomDepth];
		// set spawn shorthand
		Vector3 spawnPos = westWallManager.transform.position;
		Vector3 spawnerOffset = new Vector3 (0.5f, 0, 0);
		// find lowest x
		int x = 0 ;
		for (int z = 0; z < roomDepth; z++)
		{
			// spawn and grouping
			westWalls[z] = Instantiate(wallPrefab, (spawnPos + new Vector3(x, 0, z) + spawnerOffset), Quaternion.identity) as GameObject;
			westWalls[z].transform.SetParent(westWallManager.transform);
			// rotate to side
			//westWalls[z].transform.Rotate(new Vector3(0, 0.0f, 0));
		}
	}

	void ResizeNorthWallCollider ()
	{
		BoxCollider col = northWallManager.GetComponent<BoxCollider>();
		col.center = new Vector3 (((roomWidth / 2.0f) - 0.5f), 1.2f, roomDepth - 1);
		col.size = new Vector3(roomWidth, 2.2f, 1.0f);
	}

	void ResizeSouthWallCollider ()
	{
		BoxCollider col = southWallManger.GetComponent<BoxCollider>();
		col.center = new Vector3(((roomWidth / 2.0f) - 0.5f), 1.2f, 0.0f);
		col.size = new Vector3(roomWidth, 2.2f, 1.0f);
	}

	void ResizeEastWallCollider ()
	{
		BoxCollider col = eastWallManager.GetComponent<BoxCollider>();
		col.center = new Vector3((roomWidth - 1), 1.2f, ((roomDepth / 2.0f ) - 0.5f ));
		col.size = new Vector3(1.0f, 2.2f, roomDepth );
	}

	void ResizeWestWallCollider ()
	{
		BoxCollider col = westWallManager.GetComponent<BoxCollider>();
		col.center = new Vector3(0.0f, 1.2f, ((roomDepth / 2.0f ) - 0.5f ));
		col.size = new Vector3(1.0f, 2.2f, roomDepth );
	}



	#endregion

	void ClearAllManagers ()
	{
		//clearAll = false;
		PerpetualGeneration = false;
		//DeleteFloors();
//		while (floorManager.GetComponentInChildren<Transform>())
//		{
//			Debug.Log(floorManager.GetComponentInChildren<Transform>());
//			DestroyImmediate(floorManager.GetComponentInChildren<Transform>().gameObject);
//		}
//		while (northWallManager.GetComponentInChildren<Transform>())
//		{
//			DestroyImmediate(northWallManager.GetComponentInChildren<Transform>().gameObject);
//		}
//		
		Transform childT = floorManager.GetComponentInChildren<Transform>();
		childT = floorManager.transform.FindChild("Floor_Piece_1x1(Clone)");
		GameObject childG = childT.gameObject;
		Debug.Log(childG);
		DestroyImmediate(childG);
		ResetToOne();
		clearAll = false;

	}

	void DestroyAllChildren (GameObject _parent)
	{
		DestroyImmediate(_parent.GetComponentInChildren<Transform>().gameObject);
	}

}
