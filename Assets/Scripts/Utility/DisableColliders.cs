using UnityEngine;
using System.Collections;

public class DisableColliders : MonoBehaviour 
{
	Collider[] colliders;
	void Awake ()
	{
		// get each collider
		colliders = GetComponents<Collider>();
		// diable each collider found
		foreach(Collider _collider in colliders)
		{
			_collider.enabled = false;
		}
	}

}
