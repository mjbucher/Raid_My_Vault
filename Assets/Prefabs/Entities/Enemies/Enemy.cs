using UnityEngine;
using System.Collections;

public class Enemy : Entity , IKillable
{
	public int cost = 0;

	public Weapon.WeaponType weakness = Weapon.WeaponType.None;
	public float weaknessMultiplier = 2.0f;
	public int damage = 10;
	[Header("Detection Variables")]
	public float detectionTime = 3.0f;
	public float discoverRate = 1.0f;
	public float forgetRate = 1.0f;

	private Collider detectCol;

	public void Awake ()
	{
		detectCol = GetComponentInChildren<Collider>();
	}


	override public void Take_Damage (int _damage, Weapon.WeaponType _weaponType)
	{
		if (_weaponType.ToString() == weakness.ToString())
		{
			_damage = Mathf.RoundToInt(_damage * weaknessMultiplier); 
		}
		base.Take_Damage(_damage);
	}

	public void Found (GameObject _target)
	{
		if (detectionType == DetectionState.Alert)
		{
			
		}
		else if (detectionType == DetectionState.Attack)
		{
			
		}

	}

	// Search for entity once their target has left range
	public void Search (GameObject _target)
	{
		
	}

	public void Kill_Sequence ()
	{
		Destroy(gameObject);
	}
		
}
