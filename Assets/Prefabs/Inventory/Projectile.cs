using UnityEngine;
using System.Collections;

[RequireComponent (typeof(BoxCollider))]
public class Projectile : MonoBehaviour 
{
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
