using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[ExecuteInEditMode]
public class WallManager : MonoBehaviour 
{
	#region Variables
	// Control Booleans
	[HideInInspector]
	public bool drawGizmos;
	[HideInInspector]
	public bool generate;
	[HideInInspector]
	public bool perpetualGeneration;
	[HideInInspector]
	public bool wallEnabled = true;
	[HideInInspector]
	public bool enteranceEnabled = false;

	// Wall Variables
	[Header("Wall Variables")]
	public WallDirection wallDirection;
	[Range(-7,8)]
	public int enteranceOffset = 0;
	[HideInInspector][Range(3,9)] // not current used. Would be used if enterance size was set to proceedurally generate
	public int enteranceWidth = 3;

	GameObject[] walls;
	GameObject enterance;

	// public object references
	[Header("Guides")]
	public GameObject leftSpawnerStart;
	public Transform leftSpawnerEnd;

	public GameObject rightSpawnerStart;
	public Transform rightSpawnerEnd;

	public Transform enteranceSpawnPoint;
	public GameObject geometrySpawner;

	public BoxCollider col;
	public BoxCollider leftCol;
	public BoxCollider rightCol;

	// Object references
	RoomProceedural room; 
	GameObject wallPrefab;
	GameObject enterancePrefab;

	// set by Wall Direction Type 
	int wallLength;
	int otherWallLength;
	#endregion
		
	void Update ()
	{
		CheckControls();
	}

	#region Main Logic
	public void CheckControls()
	{
		if (perpetualGeneration && wallEnabled)
		{
			GenerateEverything();
		}
		else if (generate && wallEnabled)
		{
			GenerateEverything();
			//reset to false
			generate = false;
		}
		else if (!wallEnabled)
		{
			DestroyAllChildren(geometrySpawner);
			DestroyAllChildren(enteranceSpawnPoint.gameObject);
		}
	}

	void GenerateEverything()
	{
		// Delete old
		DestroyAllChildren(geometrySpawner);
		DestroyAllChildren(enteranceSpawnPoint.gameObject);
		// grab and set variables from input
		GrabReferences();
		SetWallVariables();
		// update spawners
		PlaceSpawners(wallLength);
		//spawn objects
		SpawnAllWalls(wallLength, otherWallLength);
		if (enteranceEnabled)
		{
			SpawnEnterance();
		}
		// update colliders
		UpdateActiveColliders();
		ResizeColliders();
	}
	#endregion

	void GrabReferences ()
	{
		room = GetComponentInParent<RoomProceedural>();
		wallPrefab = room.wallPrefab;
		enterancePrefab = room.enterancePrefab;
	}


	void SetWallVariables ()
	{
		switch (wallDirection)
		{
			case WallDirection.North:
			wallLength =  room.roomWidth;
			otherWallLength = room.roomDepth;
				break;
			case WallDirection.East:
				wallLength = room.roomDepth;
				otherWallLength = room.roomWidth;
				break;
			case WallDirection.South:
				wallLength = room.roomWidth;
				otherWallLength = room.roomDepth;
				break;
			case WallDirection.West:
				wallLength = room.roomDepth;
				otherWallLength = room.roomWidth;
				break;
			case WallDirection.None:
				Debug.Log("No wall direction assigned on: " + gameObject.name);
				break;
			default:
			Debug.Log("Exited as 'Default' wall on: " + gameObject.name);
				break;
		}
	}

	#region Spawning
	/// <summary>
	/// Places the spawners. For North and South walls use the room depth. For East and West wall use room width
	/// </summary>
	/// <param name="_length">Length.</param>
	void PlaceSpawners (int _length)
	{
		leftSpawnerStart.transform.localPosition = Vector3.zero ;
		rightSpawnerStart.transform.localPosition = new Vector3(_length - 1 , 0, 0 ) + leftSpawnerStart.transform.localPosition;
		enteranceSpawnPoint.localPosition = ((rightSpawnerStart.transform.localPosition - leftSpawnerStart.transform.localPosition) / 2.0f) + new Vector3(enteranceOffset, 0, 0);
		// determine how to use end points
		if (enteranceEnabled)
		{
			// set position end points
			Vector3 midpoint = enteranceSpawnPoint.localPosition;
			Vector3 offset = new Vector3(2 + (enteranceWidth - 3), 0, 0 );
			rightSpawnerEnd.localPosition =  midpoint + offset;
			leftSpawnerEnd.localPosition = midpoint -  offset;
		}
	}
	void SpawnAllWalls (int _wallLength, int _otherWallLength)
	{
		//Debug.Log("SpawnAllWalls called");
		walls = new GameObject[_wallLength];
		int a = _otherWallLength - 1;
		for (int b = 0; b < _wallLength; b++)
		{
			SpawnWall(b, a, b);
		}
	}

	public void SpawnWall (int x, int z, int c)
	{
		Vector3 spawnPos = new Vector3 (x, 0, -0.5f);
		// check if wall should not exist there
		if (enteranceEnabled)
		{
			if (c == enteranceSpawnPoint.localPosition.x || c == enteranceSpawnPoint.localPosition.x - 1  || c == enteranceSpawnPoint.localPosition.x + 1)
			{
				return;
			}
		}
		walls[c] = Instantiate(wallPrefab);
		// parent and move
		walls[c].transform.SetParent(geometrySpawner.transform);
		walls[c].transform.localPosition = spawnPos;
		// set rotation
		walls[c].transform.localRotation = wallPrefab.transform.rotation;
	}

	void SpawnEnterance ()
	{
		// spawn enterance
		enterance = Instantiate(enterancePrefab, enteranceSpawnPoint.position, Quaternion.identity) as GameObject;
		enterance.transform.SetParent(enteranceSpawnPoint);
		enterance.transform.rotation = transform.rotation;
	}
	#endregion

	#region Collider Functions
	void UpdateActiveColliders ()
	{
		if (enteranceEnabled)
		{
			// switch enabled colliders
			col.enabled = false;
			leftCol.enabled = true;
			rightCol.enabled = true;
			// enable end points
			leftSpawnerEnd.gameObject.SetActive(true);
			rightSpawnerEnd.gameObject.SetActive(true);
		}
		else
		{
			// switch enabled colliders
			col.enabled = true;
			leftCol.enabled = false;
			rightCol.enabled = false;
			// disable end points
			leftSpawnerEnd.gameObject.SetActive(false);
			rightSpawnerEnd.gameObject.SetActive(false);
		}
	}

	public void ResizeColliders ()
	{
		if (enteranceEnabled)
		{
			ResizeOneCollider(leftCol, leftSpawnerStart.transform.localPosition, leftSpawnerEnd.localPosition, 1.0f);
			ResizeOneCollider(rightCol, rightSpawnerStart.transform.localPosition, rightSpawnerEnd.localPosition, -1.0f);
		}
		else
		{
			ResizeOneCollider(col, leftSpawnerStart.transform.localPosition, rightSpawnerStart.transform.localPosition, 1.0f);
		}
	}

	/// <summary>
	/// Resizes the collider.  Multiplier: Use -1 for the right side or 1 for the left side and no side
	/// </summary>
	/// <param name="_col">Col.</param>
	/// <param name="_startPos">Start position.</param>
	/// <param name="_endPos">End position.</param>
	/// <param name="_multiplier">Multiplier. Multiplier: Use -1 for the right side or 1 for the left side and no side</param>
	void ResizeOneCollider (BoxCollider _col, Vector3 _startPos, Vector3 _endPos, float _multiplier)
	{
		float totalDistance = Vector3.Distance(_startPos, _endPos);

		_col.center = new Vector3((totalDistance * _multiplier) / 2.0f, 1.1f, 0);
		_col.size = new Vector3(totalDistance + 1, 2, 1);
	}
	#endregion

	#region Utility
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

	public void Reset ()
	{
		drawGizmos = true;
		generate =  false;
		perpetualGeneration = true;
		wallEnabled = true;
		enteranceEnabled = false;
		enteranceOffset = 0;
		GenerateEverything();
	}

	#endregion	

	void OnDrawGizmos ()
	{
		if (drawGizmos)
		{
			Vector3 cubeSize = Vector3.one * 0.25f;
			if (wallEnabled)
			{
				if (enteranceEnabled)
				{
					// draw colliders
					Gizmos.color = Color.green;
					Gizmos.DrawWireCube(leftCol.bounds.center, leftCol.bounds.size); // left collider
					Gizmos.color = Color.red;
					Gizmos.DrawWireCube(rightCol.bounds.center, rightCol.bounds.size); // right collider
					// drawing left collider arrow
					Gizmos.color = Color.green;
					//Gizmos().DrawArrow(leftSpawnerStart.transform, rightSpawnerStart, 0.5f, GizmosExtensions.AxisDirection.NegativeX);
					Gizmos.DrawLine(leftSpawnerStart.transform.position + leftSpawnerStart.transform.up, leftSpawnerEnd.position + leftSpawnerEnd.up); // left line
					Gizmos.DrawLine(leftSpawnerEnd.position + leftSpawnerEnd.up, leftSpawnerEnd.position + leftSpawnerEnd.up * 1.5f - leftSpawnerEnd.right * 0.25f); // left top arrow
					Gizmos.DrawLine(leftSpawnerEnd.position + leftSpawnerEnd.up, leftSpawnerEnd.position + leftSpawnerEnd.up * 0.5f - leftSpawnerEnd.right * 0.25f); // left bottom arrow
					// drawing right collider arrow
					Gizmos.color = Color.red;
					Gizmos.DrawLine(rightSpawnerStart.transform.position + rightSpawnerStart.transform.up, rightSpawnerEnd.position + rightSpawnerEnd.up); // right line
					Gizmos.DrawLine(rightSpawnerEnd.position + rightSpawnerEnd.up, rightSpawnerEnd.position + rightSpawnerEnd.up * 1.5f + rightSpawnerEnd.right * 0.25f); // right top arrow
					Gizmos.DrawLine(rightSpawnerEnd.position + rightSpawnerEnd.up, rightSpawnerEnd.position + rightSpawnerEnd.up * 0.5f + rightSpawnerEnd.right * 0.25f); // right bottom arrow
					// enterance opening frame
					Gizmos.color = Color.blue;
					Gizmos.DrawLine(leftSpawnerEnd.position, rightSpawnerEnd.position); // enterance bottom line				
					Gizmos.DrawLine(rightSpawnerEnd.position + rightSpawnerEnd.up * 2.0f, leftSpawnerEnd.position + leftSpawnerEnd.up * 2.0f); // enterance top line
					Gizmos.DrawLine(leftSpawnerEnd.position + leftSpawnerEnd.up * 2.0f, leftSpawnerEnd.position); // enterance left up line
					Gizmos.DrawLine(rightSpawnerEnd.position + rightSpawnerEnd.up * 2.0f, rightSpawnerEnd.position); // enterance right up line					
					// enterance direction arrow
					Gizmos.color = Color.yellow;
					Gizmos.DrawRay(enteranceSpawnPoint.position, enteranceSpawnPoint.forward); // enterance forward line
					Gizmos.DrawRay(enteranceSpawnPoint.position + enteranceSpawnPoint.forward, (enteranceSpawnPoint.right + enteranceSpawnPoint.forward ) * -0.25f); // left arrow line				
					Gizmos.DrawRay(enteranceSpawnPoint.position + enteranceSpawnPoint.forward, (enteranceSpawnPoint.right - enteranceSpawnPoint.forward ) * 0.25f); // right arrow line
				}
				else
				{
					// draw collider
					Gizmos.color = Color.cyan;
					Gizmos.DrawWireCube(col.bounds.center, col.bounds.size); // main collider
					// draw progression arrow
					Gizmos.DrawLine(leftSpawnerStart.transform.position + leftSpawnerStart.transform.up, rightSpawnerStart.transform.position + rightSpawnerEnd.transform.up); // main line
					Gizmos.DrawLine(rightSpawnerStart.transform.position + rightSpawnerEnd.transform.up, rightSpawnerStart.transform.position + rightSpawnerEnd.transform.up * 1.5f - rightSpawnerEnd.right * 0.25f); // main top arrow
					Gizmos.DrawLine(rightSpawnerStart.transform.position + rightSpawnerEnd.transform.up, rightSpawnerStart.transform.position + rightSpawnerEnd.transform.up * 0.5f - rightSpawnerEnd.right * 0.25f); // main bottom arrow
				}
			}
		}
	}
}
