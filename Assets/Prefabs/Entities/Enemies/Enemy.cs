using UnityEngine;
using System.Collections;

[RequireComponent(typeof(DetectionMethod))]
//[AddComponentMenu("Entities/Enemy")]
public class Enemy : Entity 
{
//	[Header("Cost of Unit")]
//	//[HideInInspector] 
//	[Tooltip("The cost for this unit, as int")]
//	public int cost = 0;
//	[Header("Weakness of Unit")]
//	[Tooltip("The weapon weakness of this unit")]
//	public WeaponType weakness = WeaponType.None;
//	[Tooltip("Weakness Multiplier")] [Range(0.0f, 5.0f)]
//	public float weaknessMultiplier = 2.0f;
	//public int damage = 10;
	//[Header("Detection Variables")]
	//public float detectionTime = 3.0f;
	//public float discoverRate = 1.0f;
	//public float forgetRate = 1.0f;
	//[Header("Current State")]
	//[HideInInspector][Tooltip("The current state of the player")]
	//public DetectionState detectionState = DetectionState.Patrolling;
	//public DetectionMode detectionAction =DetectionMode.None;
	// public DetectionMode detectionMode = DetectionMode.None;


	[HideInInspector] public DetectionMethod detectionMethod;
	[HideInInspector] public AttackManager attackManager;
	//public DetectionState detectionState = DetectionState.None;
	//[HideInInspector] public DeathSequence deathSequence;
	//[HideInInspector] public HealthManager healthManager;

	private Collider detectCol;

	void Awake ()
	{
		detectCol = GetComponentInChildren<Collider>();
		detectionMethod = GetComponent<DetectionMethod>();
		attackManager = GetComponent<AttackManager>();
		//deathSequence = GetComponent<DeathSequence>();
		//healthManager = GetComponent<HealthManager>();

	}
		
	void Update ()
	{
		if (Input.GetKeyDown(KeyCode.Space))
		{
			StartCoroutine(healthManager.DealDamage(1));
			Debug.Log(health);
		}
	}


	public IEnumerator FoundTarget(GameObject target)
	{
		// if enemmy should alert
		if (detectionMethod.mode == DetectionMode.Alert)
		{
			// play alert effects
			// add alert count
			yield return StartCoroutine(GM.AddDetection(1));
		}
		// if enemy should attack
		else if (detectionMethod.mode == DetectionMode.Attack)
		{
			transform.LookAt(target.transform);
			attackManager.Attack();
		}
	}
}
