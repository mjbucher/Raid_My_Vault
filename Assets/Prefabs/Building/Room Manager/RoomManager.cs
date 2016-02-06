using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RoomManager : MonoBehaviour 
{
	[Header("AI")]
	public bool enableAI = true;
	public int maxAI = 2;
	bool maxAIReached = false;
	[Header("Traps")]
	// maybe use state machine instead?
	public bool trapsEnabled = true;
	public int maxTraps = 4;
	public List<GameObject> totalTraps;
	List<GameObject> activeTraps;
	List<GameObject> usedTraps;
	List<GameObject> diabledTraps;
	[Header("Item Spawn points")]
	public List<GameObject> spawners;

}
