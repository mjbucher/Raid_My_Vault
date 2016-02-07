using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour 
{
	[RequireComponent (typeof(BoxCollider))]

	ProjectileManager manager;
	void Awake ()
	{
		manager = transform.parent.gameObject.GetComponent<ProjectileManager>();
	}

	void OnTriggerEnter(Collider other)
	{
		manager.Detonate();
		// perbullet detonation
		Destroy(gameObject);
	}
}
