using UnityEngine;
using System.Collections;

public class LookingForPlayer : MonoBehaviour 
{
	public bool targetPresent = false;
	public GameObject target;
	private Enemy _enemy;
	private float _currentTime = 0.0f;

	public void Awake ()
	{
		_enemy = GetComponentInParent<Enemy>();
	}

	public IEnumerator Detecting_Player()
	{
		if (targetPresent)
		{
			_currentTime += Time.deltaTime * _enemy.discoverRate;
		}
		// when player is not there, forget
		else
		{
			if (_currentTime != 0.0f)
			{
				_currentTime -= Time.deltaTime * _enemy.forgetRate;
			}
		}
		// if full time has expired perform action
		if (_currentTime >= _enemy.detectionTime)
		{
			_enemy.Found(target);
		}
		else 
		{
			yield return null;
		}
	}


	public void OnTriggerEnter (Collider _other)
	{
		if (_other.tag == "Player")
		{
			targetPresent = true;
			targetPresent = _other.gameObject;
		}
		// may cause wierd things to happen if player and AI enter
		if (_enemy.condition == StatusEffect.Confused)
		{
			if (_other.tag == "Enemy")
			{
				target = _other.gameObject;
				_enemy.Attack(target);

			}
		}
	}

	public void OnTriggerExit (Collider _other)
	{
		if (_other.tag == "Player")
		{
			targetPresent = false;
			_enemy.Search(target);
		}
		// may cause wierd things to happen if player and AI enter
		if (_enemy.condition == StatusEffect.Confused)
		{
			if (_other.tag == "Enemy")
			{
				_enemy.Search(_other.gameObject);
			}
		}
	}

}
