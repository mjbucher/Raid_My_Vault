using UnityEngine;
using System.Collections;
using System;

public class DetectionMethod : MonoBehaviour 
{
	public DetectionMode mode;
	public DetectionShape shape;
	public Weapon weapon;
	public float range;
	//public float radius;
	public float angle;
	//float sphereRadius = 0.5;
	GameObject player;
	Vector3 playerPos; 
	bool foundPlayer = false;
	bool lockOnPlayer = false;

	[Header("Detection Lengths")]
	public float discoverTime = 2.0f;
	public float forgetTime = 5.0f;


	Enemy me;

	// constructor that takes a DetectionMode
	public DetectionMethod (DetectionMode _mode, float _range, float _angle)
	{
		mode = _mode;
		range = _range;
		//radius = _radius;
		angle = _angle;
		CheckShape();
	}
		
	void Awake ()
	{ 
		// get references
		me = GetComponent<Enemy>();
		player = GameObject.FindGameObjectWithTag("Player"); 
		playerPos = player.transform.position; // player position
	}

	void Start ()
	{
		// start coroutines
		StartCoroutine ("CheckForPlayer"); 
	}

	/// <summary>
	/// Checks for player, first by the distance between them, then by the angle between us, then by a raycast to see if something is blocking line of sight.
	/// </summary>
	IEnumerable CheckForPlayer ()
	{
		yield return null;
		if (me.detectionState != DetectionState.None)
		{
			float distanceBetween = Vector3.Distance(transform.position, playerPos); // get distance between 
			// check if not in range
			if (distanceBetween > range)
			{
				yield return null;
			}
			Vector3 targetDirection = playerPos - transform.position;
			Vector3 forward = transform.forward;
			float angleBetween = Vector3.Angle(targetDirection, forward); // get angle between
			// check if angle is outside the range of my forward direction
			if (angleBetween > angle / 2.0f)
			{
				yield return null;
			}
			// check if there is a wall inbetween player and me
			RaycastHit hit;
			if (Physics.Raycast(transform.position, targetDirection, out hit, range))
			{
				if (hit.collider.tag != "Player")
				{
					foundPlayer = false;
					yield return null;
				}
				else
				{
					foundPlayer = true;	
				}
			}
			// what occurs when the AI finds the player
			if (foundPlayer)
			{
				yield return StartCoroutine("Discovering");
			}
		}
	}


	IEnumerator Discovering ()
	{
		// wait for designated time
		yield return new WaitForSeconds(discoverTime);
		// check if player was found again
		lockOnPlayer = (foundPlayer) ? true : false;
		// if true make found player
		me.detectionState = lockOnPlayer ? DetectionState.FoundPlayer : me.detectionState;
		if (lockOnPlayer)
		{
			yield return me.StartCoroutine("FoundPlayer", player);
		}
		StopCoroutine("Discovering");
	}


	// checks the Detection mode of an AI
	void CheckShape ()
	{
		switch (shape)
		{
		case DetectionShape.Line:
			CheckAsLine();
			break;
		case DetectionShape.Radial:
			CheckAsRadial();
			break;
		case DetectionShape.Sphere:
			CheckAsSphere();
			break;
		case DetectionShape.Omni:
			break;
		case DetectionShape.None:
			break;
		default:
			break;
		}
	}
		
	// line detection
	void CheckAsLine ()
	{
//		bool foundPlayer = false;
//		Ray ray = new Ray(transform.position, Vector3.forward); // ray going forward
//		RaycastHit[] hits; // hit output
//		hits = Physics.SphereCastAll(ray.origin, sphereRadius, ray.direction, range);
//		foreach (RaycastHit _hit in hits)
//		{
//			// check if it was the player
//			if (_hit.collider.tag == "Player")
//			{
//				foundPlayer = true;
//				RaycastHit playerHit = _hit;
//				break;
//			}
//		}
//		// only if player was found 
//		if (foundPlayer)
//		{
//			// check that there isn't a wall in the way
//
//		}



	}

	// spherical detection
	void CheckAsSphere ()
	{
		
	}
		
	// radial detection
	void CheckAsRadial ()
	{
		
	}

}
