using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class GameMaster : MonoBehaviour 
{
	public enum SubState
	{
		None,
        Player,
		Debug,
		Developer,
        God
	}

	[HideInInspector] public static GameMaster GM;
	[HideInInspector] public MasterManager MM;
	public GameStateEnum currentState = GameStateEnum.Active;
	public SubState currentSubState = SubState.None;
    //public TestingStateEnum currentTestingState;
	int detectionAmount = 0;

	public void Awake()
	{
		if ( GM == null) 
		{
			DontDestroyOnLoad(gameObject);
			GM = this;
		}
		else if (GM != this)
		{
			Destroy(gameObject);
		}

		MM = gameObject.GetComponent<MasterManager>();
	}

	// update this later ***
	public void Switch_GameState (GameStateEnum _targetState)
	{
		// get current state --> _temp
		// enable targeted states Manager
		// switch over to it (camera and all)
		// disable current
		// update GM state to the new one
	}

	public void Get_Manager (GameStateEnum _targetManager)
	{
		// update this later ***
		switch (_targetManager)
		{
			case GameStateEnum.Building:
				break;
			case GameStateEnum.Hub:
				break;
			case GameStateEnum.Map:
				break;
			case GameStateEnum.Paused:
				break;
			case GameStateEnum.Raiding:
				break;
			default:
				break;
		}
        Debug.Log("wHY SO SERIOUS???");
	}

	/// <summary>
	/// Adds the detected stat, and deals with reprocussions
	/// </summary>
	public IEnumerator  AddDetection (int _amount)
	{
		// return
		yield return null;
		// add detection
		detectionAmount += _amount;
		// check if exposed
			// if so played visual effect and start things
		// else nothing
		//stop coroutine
		StopCoroutine("AddDetection");
	}


	public void Save ()
	{
		BinaryFormatter bf = new BinaryFormatter();
		string fileName = Application.persistentDataPath + "/playerInfo.dat";
		FileStream file;
		if (File.Exists(fileName))
		{
			file = File.Open(fileName, FileMode.Open);
		}
		else
		{
			file = File.Create(fileName);
		}
		PlayerData data = new PlayerData();
		//set variables here or use a constructor
		bf.Serialize(file,data);
		file.Close();
	}

	public void Load ()
	{
		if (File.Exists(Application.persistentDataPath + "/playerInfo.dat"))
		{
			BinaryFormatter bf = new BinaryFormatter();
			FileStream file = File.Open(Application.persistentDataPath + "/playerInfo.dat", FileMode.Open);
			PlayerData data = (PlayerData)bf.Deserialize(file);
			file.Close();

			// set variable in local class to that of PlayerData
		}
	}


}

/// <summary>
/// used for save file and resistant data
/// </summary>
[Serializable]
class PlayerData
{
	public float health;
	public float experience;
}

