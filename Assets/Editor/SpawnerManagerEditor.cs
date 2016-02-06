using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor( typeof(SpawnerManager))]
public class SpawnerManagerEditor : Editor
{


	public void OnInpectorGUI()
	{
		DrawDefaultInspector();

		if (GUILayout.Button("Generate New Spawner"))
		{
			CreateNewSpawner();
		}

		if (GUILayout.Button("Gather Child Spawners"))
		{
			//instance.allSpawners =  Component.GetComponentsInChildren<Spawner>();

			//target.allSpawners = Component.GetComponentsInChildren<Spawner>();
		}


	}


	public void CreateNewSpawner ()
	{
		//GameObject newSpawner = Instantiate(new GameObject().AddComponent(Spawner));
	}


}
