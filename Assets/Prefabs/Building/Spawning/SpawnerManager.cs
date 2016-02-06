using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpawnerManager : MonoBehaviour 
{
	public GameObject spawnerPrefab;
	public Spawner[] allSpawners;
	List<Spawner> keySpawners;
	List<Spawner> powercellSpawners;
	List<Spawner> ventSpawners;
	List<Spawner> usedSpawners;

	void Awake()
	{
		GetAllSpawners();

	}

	void Start ()
	{
		//sort spawners
		SortSpawners(allSpawners);

	}

	void GetAllSpawners ()
	{
		allSpawners = GetComponentsInChildren<Spawner>();
	}

	void SortSpawners(Spawner[] _spawnerList)
	{
		foreach (Spawner spawner in _spawnerList)
		{
			switch(spawner.itemType)
			{
			case Spawner.spawnType.None:
				break;
			case Spawner.spawnType.Key:
				keySpawners.Add(spawner);
				break;
			case Spawner.spawnType.PowerCell:
				powercellSpawners.Add(spawner);
				break;
			case Spawner.spawnType.Vent:
				ventSpawners.Add(spawner);
				break;
			default:
				break;
			}
		}
	}

	public void RemoveSpawnerFromPool(Spawner _spawner)
	{
		usedSpawners.Add(_spawner);
		switch (_spawner.itemType)
		{
		case Spawner.spawnType.Key:
			keySpawners.Remove(_spawner);
			break;
		case Spawner.spawnType.PowerCell:
			powercellSpawners.Remove(_spawner);
			break;
		case Spawner.spawnType.Vent:
			ventSpawners.Remove(_spawner);
			break;
		default:
			break;
		}

	}

}
