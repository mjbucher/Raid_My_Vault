using UnityEngine;
using System.Collections;

public class Enemy : Entity , IKillable, IDamageable
{
	public int cost = 0;

	public WeaponType weakness = WeaponType.None;
	public float weaknessMultiplier = 2.0f;
	public int damage = 10;
	[Header("Detection Variables")]
	public float detectionTime = 3.0f;
	public float discoverRate = 1.0f;
	public float forgetRate = 1.0f;

	//public DetectionMode detectionAction =DetectionMode.None;
	public DetectionMode detectionMode = DetectionMode.None;
	public DetectionMethod detectionMethod;
	public DetectionState detectionState = DetectionState.None;

	private Collider detectCol;

	public void Awake ()
	{
		detectCol = GetComponentInChildren<Collider>();
		detectionMethod = (detectionMethod == null) ? GetComponent<DetectionMethod>():  detectionMethod;

	}


	public IEnumerator FoundPlayer (GameObject target)
	{
		// if enemmy should alert
		if (detectionMode == DetectionMode.Alert)
		{
			
		}
		// if enemy should attack
		if (detectionMode == DetectionMode.Attack)
		{
			// if not confused
			if (condition != StatusEffect.Confused)
			{
				//GameObject target = GameObject.FindGameObjectWithTag("Player");
				yield return StartCoroutine("AttackTarget",target);
			}
		}
	}


	IEnumerator AttackTarget (GameObject _target)
	{
		 	
	}



	override public void Take_Damage (int _damage, WeaponType _weaponType)
	{
		if (_weaponType == weakness)
		{
			_damage = Mathf.RoundToInt(_damage * weaknessMultiplier); 
		}
		base.Take_Damage(_damage);
	}

	public void Found (GameObject _target)
	{

	}
		

	public void Kill_Sequence ()
	{
		Destroy(gameObject);
	}

	public void Take_Damage (int _damage, DamageType _damageType)
	{
		
	}
		
}
