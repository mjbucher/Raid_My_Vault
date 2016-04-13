using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

//[ExecuteInEditMode]
public class DetectionMethod : MonoBehaviour 
{
	[Header("Detection Options")] 
	[Tooltip("Action that should occur when I spot and enemy")]
	public DetectionMode mode;
	//[Header("Not Implemented")]
	[HideInInspector] public DetectionShape shape;
	public Transform detectionRayStart;
	//[Space(20.0f)]
	//public Weapon weapon;
	[Range(1,7)] [Tooltip("The range of AI's vision")]
	public float range = 5.0f;
	//public float radius;
	[Range(0,360)] [Tooltip("Field of view for the AI, works best in increments of 10 degrees")]
	public float angle;
	//float sphereRadius = 0.5;
	// grabbing player and responsible locks
	GameObject player;
	Vector3 playerPos;
    bool foundPlayer = false;
	bool lockOnPlayer = false;

    public bool targetIsPlayer = true;
    public bool targetIsEntity = false;
    GameObject mainTarget;
    string targetTag;
    float distanceBetween;
    Vector3 targetDirection;
    float angleBetween;
    bool foundMainTarget = false;
    bool lostMainTarget = false;
    bool searchingMainTarget = false;
    Vector3 mainTargetPos;

	[Header("Detection Time Lengths")]

	[Range(0.0f,10.0f)] [Tooltip("Time (in seconds) is takes for player to be spotted before action")]
	public float discoverTime = 2.0f;

	[Range(0.0f,10.0f)] [Tooltip("Time (in seconds) is takes to forget the player after being spotted and lost")]
	public float forgetTime = 5.0f;

	[Header("Experimental")] [Tooltip("For multi targeting")]
	public Player primaryTarget;
	public List<Enemy> secondaryTargets;
	public SphereCollider sphereTrigger;

	Enemy enemy;

	// constructor that takes a DetectionMode
	public DetectionMethod (DetectionMode _mode, float _range, float _angle)
	{
		mode = _mode;
		range = _range;
		//radius = _radius;
		angle = _angle;
		//CheckShape();
	}
		
	void Awake ()
	{ 
		// get references
		enemy = GetComponent<Enemy>();
		player = GameObject.FindGameObjectWithTag("Player"); 
		primaryTarget = player.GetComponent<Player>() != null ? player.GetComponent<Player>(): null;
		playerPos = player.transform.position; // player position
	}

	void Start ()
	{
		// start coroutines
		StartCoroutine ("CheckForTarget"); 
	}

	void Update ()
	{
	}
	/// <summary>
	/// Checks for player, first by the distance between them, then by the angle between us, then by a raycast to see if something is blocking line of sight.
	/// </summary>
	public IEnumerator CheckForTarget ()
	{
        // while the enemy has not detected anything
		while (enemy.detectionState != DetectionState.None)
		{
            // get target
            if (targetIsPlayer)
                targetTag = "Player";
            mainTarget = GameObject.FindGameObjectWithTag(targetTag);
            // find other enemies here
            if (targetIsEntity)
                targetTag = "Enemy";
            // get relative data from target
            mainTargetPos = mainTarget.transform.position; // get the target's position
			distanceBetween = Vector3.Distance(transform.position, mainTargetPos); // get distance between target and self
			// check if not in range & break
			if (distanceBetween > range)
			{
				Debug.Log("Too far away: " + gameObject.name);
				yield return null;
			}
            // get direction
			targetDirection = playerPos - transform.position;
			//Debug.DrawLine(playerPos, detectionRayStart.position);
			//Vector3 forward = transform.forward;
			//Debug.DrawLine(detectionRayStart.position, transform.forward);
			angleBetween = Vector3.Angle(targetDirection, transform.forward);  // get angle between
			// check if angle is outside the range of my forward direction
			if (Mathf.Abs(angleBetween) > angle / 2.0f)
			{
				yield return null;
			}
			// check if there is a wall or cover inbetween player and me
			RaycastHit hit;
            //Ray ray = new Ray (detectionRayStart.position, targetDirection);
            Ray ray = new Ray(transform.position, targetDirection);
			//Debug.DrawRay(ray.origin,ray.direction,Color.red);
			if (Physics.Raycast(ray.origin, ray.direction, out hit)) // no range needed because already proved true
			{
                foundMainTarget = hit.collider.tag != targetTag ? false : true; // check if it hit the target
			}
			// what occurs when the AI finds the player
			if (foundMainTarget)
			{
				yield return StartCoroutine("Discovering");
			}
		}
		Debug.Log("detection state changed");
        yield return null;
	}


	IEnumerator Discovering ()
	{
		// wait for designated time
		yield return new WaitForSeconds(discoverTime);
		// check if player was found again
		lockOnPlayer = foundPlayer ? true : false;
		// if true make found player
		enemy.detectionState = lockOnPlayer ? DetectionState.FoundPlayer : enemy.detectionState;
		if (lockOnPlayer)
		{
			yield return StartCoroutine(enemy.FoundTarget(player));
		}
		StopCoroutine("Discovering");
	}

	void OnTriggerEnter (Collider _other)
	{
		// if the player entered range sphere
		if (_other.gameObject.tag == "Enemy" || _other.gameObject.tag == "Player")
		{
			//playerPos = _other.gameObject.transform.position;
		}
		// if enemy is confused
		if (enemy.condition == StatusEffect.Confused &&  _other.gameObject.tag == "Enemy")
		{
			
		}

	}

    public void OnTriggerEnter(Collision _other)
    {
 
    }

}


// AI is pathing along its route --> trying to find player while looking



