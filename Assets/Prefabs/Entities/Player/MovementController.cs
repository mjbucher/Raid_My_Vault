using UnityEngine;
using System.Collections;

public class MovementController : MonoBehaviour 
{
	[HideInInspector] public Entity thisEntity;
	// used for new coordinate points
	public Transform[] targetPoints;

	private Vector3 nextTarget;

	public void Awake()
	{
		thisEntity = gameObject.GetComponent<Entity>();
	}

	IEnumerator Start ()
	{
		yield return StartCoroutine("Pathing"); 

	}
	public void Update ()
	{
		
	}

	public IEnumerator Pathing ()
	{
		return null;
	}
}
