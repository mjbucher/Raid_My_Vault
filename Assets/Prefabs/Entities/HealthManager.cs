using UnityEngine;
using System.Collections;

public class HealthManager : MonoBehaviour 
{
	Entity entity;

	[HideInInspector] public float invulnerableTime = 0.5f;

	int maxHealth;
	int maxShield;

	void Awake ()
	{
		entity = GetComponent<Entity>();
		maxHealth = entity.health;
		maxShield = entity.shield;
	}
		

	public IEnumerator HealHealth (int _healAmount)
	{
		// Add Health checking that it doesnt go over max
		if (entity.health + _healAmount > maxHealth)
		{
			entity.health = maxHealth;
		}
		else
		{
			entity.health += _healAmount;
		}
		// play visual effect
		//........
		// Wait  before can init again
		yield return new WaitForSeconds(invulnerableTime);
		// end visual effec
		//.....
		// stop couroutine
		StopCoroutine("HealHealth");
	}

	public IEnumerator RechargeShield (int _rechargeAmount)
	{
		// Add Shield and limit to max shield
		if (entity.shield + _rechargeAmount > maxShield)
		{
			entity.shield = maxShield;
		}
		else
		{
			entity.shield += _rechargeAmount;
		}
		// play visual effect
		//........
		// Wait  before can init again
		yield return new WaitForSeconds(invulnerableTime);
		// end visual effec
		//.....
		// stop couroutine
		StopCoroutine("AddHealth");
	}

	public IEnumerator DealDamage (int _damage)
	{
		// check if player is alive
		if (entity.health > 0)
		{
			// init variables
			int shieldDamage = 0;
			int healthDamage = 0;
			// check if shield is active
			if (entity.shield > 0)
			{
				///check if there is more damage than shield strength
				if (_damage > entity.shield)
				{
					// split the damage accordingly
					shieldDamage = _damage - entity.shield;
					healthDamage = _damage - shieldDamage;
					yield return StartCoroutine(SubtractShield(shieldDamage));
					yield return StartCoroutine(SubractHealth(healthDamage));
				}
				else
				{
					yield return StartCoroutine(SubtractShield(_damage));
				}

			}
			// just subtract health
			else
			{
				yield return StartCoroutine(SubractHealth(_damage));
			}

		}
		// stop coroutine
		StopCoroutine("DealDamage");
	}

	IEnumerator SubtractShield (int _damage)
	{
		// Add Health
		entity.shield -= _damage;
		// play visual effect
		//........
		// Wait  before can init again
		if (entity.shield <= 0)
		{
			// play visual effect for breaking shield
			entity.shield = 0;
		}
		yield return new WaitForSeconds(invulnerableTime);
		// end visual effec
		//.....
		//stop courooutine
		StopCoroutine("SubtractHealth");
	}

	IEnumerator SubractHealth (int _damage)
	{
		// Add Health
		entity.health -= _damage;
		// play visual effect
		//........
		// check if player died if so change thier state
		if (entity.health <= 0)
		{
			entity.lifeState = LifeState.Dead;
			StartCoroutine(entity.Check_LifeState());
		}
		// Wait  before can init again
		yield return new WaitForSeconds(invulnerableTime);
		// end visual effec
		//.....
		//stop courooutine
		StopCoroutine("SubtractHealth");
	}
}
