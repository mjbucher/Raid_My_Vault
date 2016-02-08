using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InventoryManager : MonoBehaviour 
{
	// Prefab for a generic button
	public GameObject buttonPrefab;
	// Lists of all assets that can be built (these may be different tabs)
	public List <GameObject> weapons;
	public List <GameObject> levelPieces;
	public List <GameObject> traps;
	public List <GameObject> AI;
	List <GameObject> allItems;

	public int numberOfItems;
	int[,] buttonPositions;

	void Awake ()
	{
		// sort all lists
		SortLists();
	}

	// sort all lists
	void SortLists ()
	{
		weapons.Sort();
		levelPieces.Sort();
		traps.Sort();
		AI.Sort();
	}

}
