using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Weapon : MonoBehaviour
{
	
	public WeaponType weaponType;
	public DamageType damageType; 
	public StatusEffect statusEffect;

	public float range = 1 * 1.0f;
	public int weaponDamage = 0;
	public float damageRadius = 0;

	// maybe use a delegate that assembles the core actions on start so that if doesn't have to check every time it is fired?

	//public delegate void CoreAction ();
	//public CoreAction coreAction;

	ProjectileManager projectileManager;
	public Transform projectileSpawnLocation;
	public GameObject projectile;
	public float projectileSpeed;

	public ParticleSystem particles;


	public bool firing = false;

	List<Entity> targetsEffected = new List<Entity>();


	void Awake ()
	{
		projectileManager = GetComponent<ProjectileManager>();
	}

	void Update ()
	{
		if (firing)
		{
			//Instantiate();
		}
	}

	// User must be facing the target in their positive local Z axis
	public void Attack ()
	{
		if (weaponType == WeaponType.Projectile)
		{
			Ray ray = new Ray();
			RaycastHit hit;
			// set ray
			ray =  new Ray(transform.position, Vector3.forward);
			// Find center of attack if it is projectile
			if (weaponType == WeaponType.Projectile)
			{
				// assuming using normal projectile
				if (Physics.Raycast(ray.origin, ray.direction,  out hit, range)) 
				{
					GameObject _target = hit.collider.gameObject;
					Entity _targetEntity = _target.GetComponent<Entity>();
					// change this check if I want it to effect other things than Enitities
					if (_targetEntity)
					{
						// add Entitiy to list
						targetsEffected.Add(_targetEntity);
						StartCoroutine(FireWeapon(hit));
					}
				}
			}
			else if (weaponType == WeaponType.Grenade)
			{
				//Raycast that ignores cover (direct line of sight, not needed)
				if (Physics.Raycast(ray, out hit, range, LayerMask.NameToLayer("Cover")))
				{
					GameObject _target = hit.collider.gameObject;
					Entity _targetEntity = _target.GetComponent<Entity>();
					// change this check if I want it to effect other things than Enitities
					if (_targetEntity)
					{
						// add Entitiy to list
						targetsEffected.Add(_targetEntity);
						StartCoroutine(ThrowGrenade(hit));
					}
				}
			}
			else if (weaponType == WeaponType.Trap)
			{
				// place trap at player location
				// make it so that it only attacks enemies
				// on tigger enter
			}
			// if it should be an AOE raycast and add to target list
			// separate this out as antoher coroutine or function to be asdded to  delegate.
			if (damageRadius > 0)
			{
				// set ray
				Vector3 _origin = transform.position + (Vector3.forward * range);
				ray = new Ray(_origin, Vector3.zero);
				RaycastHit[] allHit = Physics.SphereCastAll(ray.origin, damageRadius, Vector3.zero);
				if (allHit.Length > 0) 
				{
					foreach(RaycastHit _hit in allHit)
					{
						Entity _target = _hit.collider.gameObject.GetComponent<Entity>();
						if (_target)
						{
							targetsEffected.Add(_target);
						}
						else
						{
							Debug.Log("not an enemy");	
						} 
					}
				}
			}
			else if (damageRadius < 0)
			{
				Debug.Log("damageRadius value of " + damageRadius + ". Value should be >= 0");
			}
		}
			// play effects for firing
			// maybe add in counter code for burst firing and rapid firing
			// launch projectile in include the ray for calculations 

			//projectileManager.FireProjectile(ray);
	}

	IEnumerator FireWeapon (RaycastHit _hit) 
	{
		// play effects
		// spawn projectile and fire then call damage;
		GameObject _projectile = Instantiate(projectile, projectileSpawnLocation.position, Quaternion.identity) as GameObject;
		// move it forward
		float _speed = projectileSpeed * Time.deltaTime;
		_projectile.transform.position = Vector3.MoveTowards(transform.position, _hit.point, _speed);
		DestroyObject(_projectile);
		// deal damage
		yield return StartCoroutine(DamageTargets(targetsEffected));
		//StopCoroutine("FireWeapon");
	}
	 
	IEnumerator MeleeAttack ()  
	{ 
		// play effects

		// deal damage
		yield return StartCoroutine(DamageTargets(targetsEffected));
		//StopCoroutine("MeleeAttack");
	}

	IEnumerator ThrowGrenade (RaycastHit _hit) 
	{
		// play effects
		// spawn projectile and fire then call damage;
		GameObject _projectile = Instantiate(projectile, projectileSpawnLocation.position, Quaternion.identity) as GameObject;
		// move it forward
		float _speed = projectileSpeed * Time.deltaTime;
		// ** add arch into target vector
		_projectile.transform.position = Vector3.MoveTowards(transform.position, _hit.point, _speed);
		// deal damage
		yield return StartCoroutine(DamageTargets(targetsEffected));
		//StopCoroutine("ThrowGrenade");
	}

	IEnumerator PlaceTrap ()
	{
		// play effects

		// Danage Enemies
		yield return StartCoroutine(DamageTargets(targetsEffected));
		// terminate couroutine
		//StopCoroutine("PlaceTrap");
	}


	IEnumerator DamageTargets(List<Entity> _targets)
	{
		// make enemies take damage
		foreach (Entity _target in _targets)
		{
			int _damage = damageType == _target.weakness ? Mathf.RoundToInt(weaponDamage * _target.weaknessMultiplier) : weaponDamage;
			yield return StartCoroutine(_target.healthManager.DealDamage(_damage));
		}
		// clear the list
		_targets.Clear();
		// stop coroutine
		//StopCoroutine("DamageEnemies");
	}
}
