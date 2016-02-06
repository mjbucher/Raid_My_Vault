using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[ExecuteInEditMode]
public class FloorManager : MonoBehaviour 
{
	public bool generateLists = false;
	public GameObject[] allFloorTiles;
	public List<UnityEngine.GameObject> validFloorTiles;

	void Update()
	{
		if (generateLists)
		{
			// get all tiles
			allFloorTiles = GetComponentsInChildren<GameObject>();

			// grab all valid tiles into list
			foreach (GameObject obj in allFloorTiles)
			{
				if (obj.tag == "Floor" && obj.layer == LayerMask.NameToLayer("Walkable"))
				{
					validFloorTiles.Add(obj);

				}
			}
			// remove any
			generateLists = false;
		}
	
	}


}
