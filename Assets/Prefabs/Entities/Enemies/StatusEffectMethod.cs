using UnityEngine;
using System.Collections;

public class StatusEffectMethod : MonoBehaviour 
{
	Enemy me;

	void Awake ()
	{
		me = GetComponent<Enemy>();
	}

	// called to apply effect
	public void ApplyCondition (StatusEffect _effect)
	{
		me.condition = _effect; // set the condition
		// sort through which function to use
		switch (_effect)
		{
		case StatusEffect.Confused:
			break;
		case StatusEffect.Dazed:
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
	void ApplyDaze ()
	{
		
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
