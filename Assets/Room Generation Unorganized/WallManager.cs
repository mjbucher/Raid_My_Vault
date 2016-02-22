using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
[System.Serializable]
public class WallManager : MonoBehaviour 
{
	public bool drawGizmos;
	[SerializeField]
	public bool wallEnabled = true;
	public WallDirection wallDirection;
	public bool enteranceEnabled = false;
	//public GameObject managerObject;


	[HideInInspector]
	public GameObject[] walls;

	[Range(-7,8)]

	public int enteranceOffset = 0;
	[HideInInspector][Range(3,9)]
	public int enteranceWidth = 3;

	[Header("References")]
	public RoomProceedural room; 
	GameObject wallPrefab;

	[Header("Guides")]
	public Transform leftSpawnerStart;
	public Transform leftSpawnerEnd;

	//[Range(0,15)]
	//public int leftSpawnerOffset;

	public Transform rightSpawnerStart;
	public Transform rightSpawnerEnd;

	//[Range(0,15)]
	//public int rightSpawnerOffset;

	public GameObject geometrySpawner;
	public BoxCollider col;
	Vector3 newColSize;
	Vector3 newColCenter;
	 
	public WallManager()
	{
		
	}


	void Awake ()
	{
		room = GetComponentInParent<RoomProceedural>();
		col = GetComponent<BoxCollider>();

	}

	void Start ()
	{
		wallPrefab = room.wallPrefab;
	}

	public void ResizeWall (int _roomWidth, int _roomDepth)
	{
		
	}

	public void ModifyWalls ()
	{
		if (wallDirection == WallDirection.North)
		{
			walls = new GameObject[room.roomWidth];
			int z = room.roomDepth - 1 ;
			for (int x = 0; x < room.roomWidth; x++)
			{
				SpawnWall(x, z, x);
			}
		}
		else if (wallDirection == WallDirection.East)
		{
			walls = new GameObject[room.roomDepth];
			int x = room.roomWidth - 1 ;
			for (int z = 0; z < room.roomDepth; z++)
			{
				SpawnWall(x, z, z);
			}
		}
		else if (wallDirection == WallDirection.West)
		{
			walls = new GameObject[room.roomDepth];
			int x = 0 ;
			for (int z = 0; z < room.roomDepth; z++)
			{
				SpawnWall(x, z, z);
			}
		}
		else if (wallDirection == WallDirection.South)
		{
			walls = new GameObject[room.roomWidth];
			int z = 0;
			for (int x = 0; x < room.roomWidth; x++)
			{
				SpawnWall(x, z, x);
			}
		}


	}


	public void SpawnWall (int x, int z, int c)
	{
		Vector3 spawnPos = new Vector3 (x, 0, z) + transform.localPosition + room.transform.position; // + wallPrefab.transform.position;
		walls[c] = Instantiate(wallPrefab, spawnPos, Quaternion.identity) as GameObject;
		walls[c].transform.SetParent(geometrySpawner.transform);
		// position offset
		Vector3 posOffset = walls[c].transform.localPosition + wallPrefab.transform.position;
		walls[c].transform.localPosition = posOffset;
		//rotation offset
		walls[c].transform.localRotation = wallPrefab.transform.rotation;
	}


	public void ResizeCollider ()
	{
		newColCenter = col.center;
		newColSize = col.size;
		if (wallDirection == WallDirection.North)
		{
			newColCenter.x = (room.roomWidth / 2.0f) - 0.5f; 
			newColCenter.z = room.roomDepth - 1; 
			newColSize.x = room.roomWidth;
		}
		else if (wallDirection == WallDirection.East)
		{
			newColCenter.x = (1 - room.roomDepth) / 2.0f;
			newColCenter.z = room.roomWidth - 1;
			newColSize.x = room.roomDepth; 
		}
		else if (wallDirection == WallDirection.South)
		{
			newColCenter.x = (1 - room.roomWidth) / 2.0f;
			newColSize.x = room.roomWidth;
		}
		else if (wallDirection == WallDirection.West)
		{
			newColCenter.x = (room.roomDepth / 2.0f) - 0.5f;
			newColSize.x = room.roomDepth; 
		}
		else
		{
			Debug.Log("No direction found on: " + gameObject.name);
		}
		col.center = newColCenter;
		col.size = newColSize;

		UpdateSideSpawners();
	}


	void UpdateSideSpawners ()
	{
		switch (wallDirection)
		{
		case WallDirection.North:
			// set start points
			leftSpawnerStart.position = new Vector3(0, 0, col.center.z) + transform.position;
			rightSpawnerStart.position =  new Vector3(room.roomWidth - transform.position.x, 0, col.center.z) + transform.position; 
			// determine how to use end points
			if (enteranceEnabled)
			{
				// enable end points
				leftSpawnerEnd.gameObject.SetActive(true);
				rightSpawnerEnd.gameObject.SetActive(true);
				// set position end points
				float colX = col.center.x + enteranceOffset;
				float spacerX = (enteranceWidth / 2.0f) + 0.5f;
				rightSpawnerEnd.position = new Vector3(colX + spacerX , 0, col.center.z) + transform.position;
				leftSpawnerEnd.position = new Vector3(colX - spacerX , 0, col.center.z ) + transform.position; 
			}
			else 
			{
				// disable end points
				leftSpawnerEnd.gameObject.SetActive(false);
				rightSpawnerEnd.gameObject.SetActive(false);
			}
			break;
		case WallDirection.East:
			// set start points
			leftSpawnerStart.position = new Vector3(room.roomWidth - 1, 0, room.roomDepth - 1) + transform.position;
			rightSpawnerStart.position =  new Vector3(room.roomWidth - 1, 0, 0) + transform.position; 
			// determine how to use end points
			if (enteranceEnabled)
			{
				// enable end points
				leftSpawnerEnd.gameObject.SetActive(true);
				rightSpawnerEnd.gameObject.SetActive(true);
				// set position end points
				float colZ = 0 - col.center.x + enteranceOffset;
				float spacerZ = (enteranceWidth / 2.0f) + 0.5f; 
				rightSpawnerEnd.position = new Vector3(col.center.z , 0, colZ - spacerZ)  + transform.position ;
				leftSpawnerEnd.position = new Vector3(col.center.z , 0, colZ  + spacerZ) + transform.position ;
			}
			else  
			{
				// disable end points
				leftSpawnerEnd.gameObject.SetActive(false);
				rightSpawnerEnd.gameObject.SetActive(false);
			}
			break;
		case WallDirection.South:
			// set start points
			rightSpawnerStart.position = new Vector3(0, 0, 0) + transform.position;
			leftSpawnerStart.position =  new Vector3(room.roomWidth - transform.position.x, 0, 0) + transform.position; 
			// determine how to use end points
			if (enteranceEnabled)
			{
				// enable end points
				rightSpawnerEnd.gameObject.SetActive(true);
				leftSpawnerEnd.gameObject.SetActive(true);
				// set position end points
				float colX = -col.center.x + enteranceOffset;
				float spacerX = (enteranceWidth / 2.0f) + 0.5f;
				leftSpawnerEnd.position = new Vector3(colX + spacerX , 0, col.center.z) + transform.position;
				rightSpawnerEnd.position = new Vector3(colX - spacerX , 0, col.center.z ) + transform.position; 
			}
			else 
			{
				// disable end points
				leftSpawnerEnd.gameObject.SetActive(false);
				rightSpawnerEnd.gameObject.SetActive(false);
			}
			break;
		case WallDirection.West:
			rightSpawnerStart.position = new Vector3(0, 0, room.roomDepth - 1) + transform.position;
			leftSpawnerStart.position =  new Vector3(0, 0, 0) + transform.position; 
			// determine how to use end points
			if (enteranceEnabled)
			{
				// enable end points
				rightSpawnerEnd.gameObject.SetActive(true);
				leftSpawnerEnd.gameObject.SetActive(true);
				// set position end points
				float colZ = col.center.x + enteranceOffset;
				float spacerZ = (enteranceWidth / 2.0f) + 0.5f; 
				leftSpawnerEnd.position = new Vector3(0, 0, colZ - spacerZ)  + transform.position ;
				rightSpawnerEnd.position = new Vector3(0, 0, colZ  + spacerZ) + transform.position ;
			}
			else  
			{
				// disable end points
				rightSpawnerEnd.gameObject.SetActive(false);
				leftSpawnerEnd.gameObject.SetActive(false);
			}
			break;
		case WallDirection.None:
			Debug.Log("No wall direction set on: " + gameObject.name);
			break;
		default:
			break;
		}
	}

	void OnDrawGizmos ()
	{
		if (drawGizmos)
		{
			if (enteranceEnabled)
			{
				Gizmos.color = Color.green;
				Gizmos.DrawLine(leftSpawnerStart.position, leftSpawnerEnd.position);
				Gizmos.color = Color.green;
				Gizmos.DrawLine(leftSpawnerEnd.position, leftSpawnerEnd.position + leftSpawnerEnd.up);
				Gizmos.color = Color.red;
				Gizmos.DrawLine(rightSpawnerStart.position, rightSpawnerEnd.position);
				Gizmos.color = Color.red;
				Gizmos.DrawLine(rightSpawnerEnd.position, rightSpawnerEnd.position + rightSpawnerEnd.up);	
			}
			else
			{
				Gizmos.color = Color.blue;
				Gizmos.DrawLine(leftSpawnerStart.position, rightSpawnerStart.position);
			}
		}
	}
}
