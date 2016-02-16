using UnityEngine;
using System.Collections;

public class StatusEffectMethod : MonoBehaviour 
{
	Enemy me;
	public float dazeTime = 3.0f;
	public float empTime  = 3.0f;
	public float confusedTime = 3.0f;
	public float stuckTime = 3.0f;
	public float slowedTime = 3.0f;
	public float knockbackSpeed = 10.0f;
	public float knockbackDistance = 4.0f;


	void Awake ()
	{
		me = GetComponent<Enemy>();
	}

	// called to apply effect
	public IEnumerator ApplyCondition (StatusEffect _effect)
	{
		me.condition = _effect; // set the condition
		// sort through which function to use
		switch (_effect)
		{
		case StatusEffect.Confused:
			break;
		case StatusEffect.Dazed:
			yield return StartCoroutine("ApplyDaze");
			break;
		case StatusEffect.EMP:
			break;
		case StatusEffect.KnockedBack:
			break;
		case StatusEffect.Slowed:
			break;
		case StatusEffect.Stuck:
			break;
		case StatusEffect.None:
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
		// record original speed and detection
		float _speed = me.speed;
		DetectionState _state = me.detectionState;
		// stop speed and detection
		me.speed = 0;
		me.detectionState = DetectionState.None;
		// wait to wear off
		yield return new WaitForSeconds(dazeTime);
		// restore detection and speed
		me.detectionState = _state;
		me.speed = _speed; 
		// terminate couroutine
		StopCoroutine("AppyDaze");
	}

	// Ai will be diabled for a time
	void ApplyEMP ()
	{
		
	}

	// AI will be pushed / pulled towards a point
	void ApplyKnockBack ()
	{
		
	}

	// AI speed will be reduced
	void ApplySlow ()
	{
		
	}

	// AI movement will be disabled
	void ApplyStuck ()
	{
		
	}

	// AI function are returned to normal
	void ApplyNone ()
	{
		
	}



}
