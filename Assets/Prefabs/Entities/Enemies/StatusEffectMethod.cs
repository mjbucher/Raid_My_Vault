using UnityEngine;
using System.Collections;

/// <summary>
/// Left for Implementation
/// ApplyConfusion
/// ApplyKnockback
/// </summary>

public class StatusEffectMethod : MonoBehaviour 
{
	Entity entity;

	[HideInInspector]public float dazeTime = 3.0f;
	[HideInInspector]public float empTime  = 3.0f;
	[HideInInspector]public float confusedTime = 3.0f;
	[HideInInspector]public float stuckTime = 3.0f;
	[HideInInspector]public float slowedPercent = 0.5f;
	[HideInInspector]public float slowedTime = 3.0f;
	[HideInInspector]public float knockbackSpeed = 10.0f;
	[HideInInspector]public float knockbackDistance = 4.0f;

	[Space(10)]
	public bool canBeDazed = true;
	public bool canBeEMP = true;
	public bool canBeConfused = true;
	public bool canBeStuck = true;
	public bool canBeSlowed = true;
	public bool canBeKnockedBack = true;

	void Awake ()
	{
		entity = GetComponent<Entity>();
	}

	// called to apply effect
	public IEnumerator ApplyCondition (StatusEffect _effect)
	{
		entity.condition = _effect; // set the condition
		// sort through which function to use
		switch (_effect)
		{
			case StatusEffect.Confused:
				if (canBeConfused)
				{
					//yield return StartCoroutine("ApplyConfusion");
				}
				break;
			case StatusEffect.Dazed:
				if (canBeDazed)
				{
					yield return StartCoroutine("ApplyDaze");
				}
				break;
			case StatusEffect.EMP:
				if (canBeEMP)
				{
					yield return StartCoroutine("ApplyEMP");
				}
				break;
			case StatusEffect.KnockedBack:
				if (canBeKnockedBack)
				{
					//yield return StartCoroutine("ApplyKnockBack");
				}
				break;
			case StatusEffect.Slowed:
				if (canBeSlowed)
				{
					yield return StartCoroutine("ApplySlow");
				}
				break;
			case StatusEffect.Stuck:
				if (canBeStuck)
				{
					yield return StartCoroutine("ApplyStuck");
				}
				break;
			case StatusEffect.None:
				yield return StartCoroutine("ApplyNone");
				break;
			default:
				yield return StartCoroutine("ApplyNone");
				break;
		}
	}

	// AI will harm other AI
	void ApplyConfusion ()
	{
		
	}

	// AI will be stuned
	IEnumerator ApplyDaze ()
	{
		// record original movement and detection
		MovementState _moveState = entity.movementState;
		DetectionState _detectState = entity.detectionState;
		// stop speed and detection
		entity.movementState = MovementState.None;
		entity.detectionState = DetectionState.None;
		// Apply effects here
		// ..............
		// wait to wear off
		yield return new WaitForSeconds(dazeTime);
		// restore detection and speed
		entity.detectionState = _detectState;
		entity.movementState = _moveState;
		// Turn off effects here
		// ..............
		// terminate couroutine
		StopCoroutine("AppyDaze");
	}

	// Ai will be diabled for a time
	IEnumerator ApplyEMP ()
	{
		// record original speed
		DetectionState _detectState = entity.detectionState;
		MovementState _moveState = entity.movementState;
		// stop everything
		entity.detectionState = DetectionState.None;
		entity.movementState = MovementState.None;
		// Apply effects here
		// ..............
		// wait to wear off
		yield return new WaitForSeconds(empTime);
		// restore
		entity.detectionState = _detectState;
		entity.movementState = _moveState;
		// Turn off effects here
		// ..............
		// terminate
		StopCoroutine("ApplyEMP");


	}

	// AI will be pushed / pulled towards a point
	void ApplyKnockBack ()
	{
		
	}

	// AI speed will be reduced
	// *** I need to modify this to be based on time.timeScale eventually
	IEnumerator ApplySlow ()
	{
		// record speed
		float _speed = entity.speed;
		// lower speed
		entity.speed *= slowedPercent;
		//Apply visual effect
		//................
		// wait for time
		yield return new WaitForSeconds (slowedTime);
		// return speed
		entity.speed = _speed;
		// End Effect
		//................
		// stop coroutine
		StopCoroutine("ApplySlow");
	}

	// AI movement will be disabled
	IEnumerator ApplyStuck ()
	{
		//record movement state
		MovementState _moveState = entity.movementState;
		// disable movement
		entity.movementState = MovementState.None;
		// Apply visual effect
		//....................
		// wait for time
		yield return new WaitForSeconds (stuckTime);
		// return movement
		entity.movementState = _moveState;
		// end visual effects
		//....................
		//stop coroutine
		StopCoroutine("ApplyStuck");

	}

	// AI function are returned to normal
	IEnumerator ApplyNone ()
	{
		yield return null;
		Debug.Log("For some reason you are trying to apply StatusEffect.None on an object! @ " + gameObject.ToString());
		StopCoroutine("ApplyNone");
	}



}
