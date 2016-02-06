using UnityEngine;
using System.Collections;

//[ExecuteInEditMode]
public  class MasterManager : MonoBehaviour 
{
	[Header("Game Master Controller")]
	public GameMaster gameMaster;

	[Header("Managers")]
	public HubManager hubManager;
	public LevelEditorManager levelEditorManager;
	public DungeonManager dungeonManager;
	public CipherManager cipherManager;
	public MotiveManager motiveManager;
	public NetworkManager networkManager;

	public void Awake()
	{
		// grab game master
		gameMaster = GetComponent<GameMaster>();

		// grab child managers
		hubManager = GetComponentInChildren<HubManager>();
		levelEditorManager = GetComponentInChildren<LevelEditorManager>();
		dungeonManager = GetComponentInChildren<DungeonManager>();
		cipherManager = GetComponentInChildren<CipherManager>();
		motiveManager = GetComponentInChildren<MotiveManager>();
		networkManager = GetComponentInChildren<NetworkManager>();

	}

	#region Toggle Functions
	public void Toggle_Hub()
	{
		hubManager.enabled = !hubManager.enabled;
		Debug.Log("Hub Manager Now : " + hubManager.enabled);
	}

	public void Toggle_LevelEditor()
	{
		levelEditorManager.enabled = !levelEditorManager;
		Debug.Log("LevelEditor Manager Now : " + levelEditorManager.enabled);
	}

	public void Toggle_Dungeon()
	{
		dungeonManager.enabled = !dungeonManager.enabled;
		Debug.Log("Dungeon Manager Now : " + dungeonManager.enabled);
	}

	public void Toggle_Cipher()
	{
		cipherManager.enabled = !cipherManager.enabled;
		Debug.Log("Cipher Manager Now : " + cipherManager.enabled);
	}

	public void Toggle_Motive()
	{
		motiveManager.enabled = !motiveManager.enabled;
		Debug.Log("Motive Manager Now : " + motiveManager.enabled);
	}

	public void Toggle_Network()
	{
		networkManager.enabled = !networkManager.enabled;
		Debug.Log("Network Manager Now : " + networkManager.enabled);
	}
	#endregion
}


