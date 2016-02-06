using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour 
{

	public Color gizmoColor = Color.magenta;

	public enum spawnType
	{
		None,
		Key,
		PowerCell,
		Vent
	}

	public spawnType itemType = spawnType.None;

	SpawnerManager spawnManager;

	void Awake () 
	{
		spawnManager = GetComponentInParent<SpawnerManager>();
	}


	public void OnDrawGizmos()
	{
		
		Gizmos.color = gizmoColor;
		Gizmos.DrawSphere(transform.position, 0.25f);
		Gizmos.DrawWireCube(transform.position, Vector3.one * 0.5f);
	}
}
