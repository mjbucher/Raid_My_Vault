using UnityEngine;
using System.Collections;

public class DetectionMethod : MonoBehaviour 
{
	public DetectionMode mode;
	public float range;
	//public float radius;
	public float angle;
	//float sphereRadius = 0.5;
	Vector3 playerPos; 
	bool foundPlayer = false;


	// constructor that takes a DetectionMode
	public DetectionMethod (DetectionMode _mode, float _range, float _angle)
	{
		mode = _mode;
		range = _range;
		//radius = _radius;
		angle = _angle;
		CheckMode();
	}
		
	void Awake ()
	{
		playerPos = GameObject.FindGameObjectWithTag("Player").transform.position; // player position
	}


	/// <summary>
	/// Checks for player, first by the distance between them, then by the angle between us, then by a raycast to see if something is blocking line of sight.
	/// </summary>
	void CheckForPlayer ()
	{
		float distanceBetween = Vector3.Distance(transform.position, playerPos); // get distance between 
		// check if not in range
		if (distanceBetween > range)
		{
			return;
		}
		Vector3 targetDirection = playerPos - transform.position;
		Vector3 forward = transform.forward;
		float angleBetween = Vector3.Angle(targetDirection, forward); // get angle between
		// check if angle is outside the range of my forward direction
		if (angleBetween > angle)
		{
			return;
		}
		// check if there is a wall inbetween player and me
		RaycastHit hit;
		if (Physics.Raycast(transform.position, targetDirection, out hit, range))
		{
			if (hit.collider.tag != "Player")
			{
				foundPlayer = false;
				return;
			}
			else
			{
				foundPlayer = true;	
			}
		}
		// what occurs when the AI finds the player
		if (foundPlayer)
		{
			
		}
	}





	// checks the Detection mode of an AI
	void CheckMode ()
	{
		switch (mode)
		{
		case DetectionMode.Line:
			CheckAsLine();
			break;
		case DetectionMode.Radial:
			CheckAsRadial();
			break;
		case DetectionMode.Spherical:
			CheckAsSphere();
			break;
		case DetectionMode.None:
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
