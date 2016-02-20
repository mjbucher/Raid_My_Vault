using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Weapon : MonoBehaviour
{
	
	public WeaponType weaponType;
	public DamageType damageType; 

	public float range = 1 * 1.0f;
	public int weaponDamage = 0;
	public float damageRadius = 0;

	public delegate void CoreAction ();
	public CoreAction coreAction;

	ProjectileManager projectileManager;
	public Transform projectileSpawnLocation;
	public GameObject pSrojectile;

	public bool firing = false;

	List<Enemy> enemiesEffected = new List<Enemy>();


	void Awake ()
	{
		projectileManager = GetComponent<ProjectileManager>(); 
	}

	// assembles the delegate based on choices
	void BuildWeapon ()
	{
		
		// use weapon type to determine what type of attack function should be used
		

		// use damage type to determine what type of effets should be disable on AttackComplete()


		// use Status effect to dtermine what special effect should be applied to the target of the weapon

	}

	void Update ()
	{
		if (firing)
		{
			Instantiate()
		}
	}


	public void Attack ()
	{
		Ray ray = new Ray();
		RaycastHit hit;
		// make new range 
		float _range = 1.0f * range;
		// if only effects 1 enemy
		if (damageRadius == 0)
		{
			// set ray
			ray =  new Ray(transform.position, Vector3.forward);
			if (Physics.Raycast(ray.origin, ray.direction, out hit, 1.0f * range)) 
			{
				GameObject _target = hit.collider.gameObject;
				Enemy _targetEnemy = _target.GetComponent<Enemy>();
				// change this check if I want it to effect other things than enemies
				if (_targetEnemy)
				{
					// add enemy to list
					enemiesEffected.Add(_targetEnemy);
				}
			}
		}
		// if it should be an AOE
		else if (damageRadius > 0)
		{
			// set ray
			Vector3 _origin = transform.position + (Vector3.forward * range);
			ray = new Ray(_origin, Vector3.zero);
			RaycastHit[] allHit = Physics.SphereCastAll(ray.origin, damageRadius, Vector3.zero);
			if (allHit.Length > 0) 
			{
				foreach(RaycastHit _hit in allHit)
				{
					Enemy _target = _hit.collider.gameObject.GetComponent<Enemy>();
					if (_target)
					{
						enemiesEffected.Add(_target);
					}
					else
					{
						Debug.Log("not an enemy");	
					} 
				}
			}
		}
		else
		{
			Debug.Log("damageRadius value of " + damageRadius + ". Value should be >= 0");
		}
		// play effects for firing
		// maybe add in counter code for burst firing and rapid firing
		// launch projectile in include the ray for calculations 

		projectileManager.FireProjectile(ray);
	}


	IEnumerator DamageEnemies()
	{
		yield return null;
		// make enemies take damage
		foreach (Enemy _enemy in enemiesEffected)
		{
			// change weapon damage if nessary
			if (damageType == _enemy.weakness)
			{
				yield return StartCoroutine(_enemy.healthManager.DealDamage( Mathf.RoundToInt(weaponDamage * _enemy.weaknessMultiplier)));
			}
			yield return StartCoroutine(_enemy.healthManager.DealDamage(weaponDamage));
		}
		// clear the list
		enemiesEffected.Clear();
		// stop coroutine
		StopCoroutine("DamageAllEnemies");
	}
}
