using UnityEngine;
using System.Collections;

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
		Destroy(gameObject);
	}
}
