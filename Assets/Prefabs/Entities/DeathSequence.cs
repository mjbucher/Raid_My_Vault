using UnityEngine;
using System.Collections;

public class DeathSequence : MonoBehaviour 
{
	//[Header("My Death Type")]
	[Space(10)]
	[Tooltip("This is used to determine which death sequence to use")]
	public DeathEnum deathType = DeathEnum.None;

	Enemy enemy;
	Vector3 spawnPos;

	void Awake ()
	{
		// get components
		enemy = GetComponent<Enemy>();
		// set local variables
		spawnPos = transform.position;
	}
		

	public IEnumerator DeathInit ()
	{
		// yield
		yield return null;
		// turn off unit's detection
		enemy.detectionMethod.StopCoroutine("CheckForPlayer");
		// switch statement for determining which function to use
		switch (deathType)
		{
			case DeathEnum.CameraBot:
				CameraBotDeath();
				break;
			case DeathEnum.DroneBot:
				DroneBotDeath();
				break;
			case DeathEnum.GuardBot:
				GuardBotDeath();
				break;
			case DeathEnum.Player:
				PlayerDeath();
				break;
			case DeathEnum.TurretBot:
				TurretBotDeath();
				break;
			case DeathEnum.None:
				break;
			default:
				break;
		}
		// stop coroutine
		StopCoroutine("DeathInit");
	}


	// death Sequence for player 
	void PlayerDeath ()
	{
		// get nessesary components
		// play animations
		// initialize particle effect
		// at end kill / destory
	}

	// death sequence for camera
	void CameraBotDeath ()
	{
		
	}

	// death sequence for drone
	void DroneBotDeath ()
	{
		// get nessesary components
		// play animations
		// initialize particle effect
		// at end kill / destory
	}

	// death sequence for guard
	void GuardBotDeath ()
	{
		// get nessesary components
		// play animations
		// initialize particle effect
		// at end kill / destory
	}

	// death sequence for turret
	void TurretBotDeath ()
	{
		// get nessesary components
		// play animations
		// initialize particle effect
		// at end kill / destory
	}

	// death sequence for recone drone
	void ReconBotDeath ()
	{
		// get nessesary components
		// play animations
		// initialize particle effect
		// at end kill / destory
	}

}
