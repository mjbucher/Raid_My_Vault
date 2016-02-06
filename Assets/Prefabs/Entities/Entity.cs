using UnityEngine;
using System.Collections;

public class Entity : MonoBehaviour 
{
	[HideInInspector] public GameMaster GM;

	public enum LifeState
	{
		Alive,
		Dead,
		Unconscious
	};

	public enum Condition
	{
		None,
		Slowed,
		Blind,
		Confused
	}

	public enum MovementState 
	{
		Move,
		LookAround,
		Stationary
	};

	public enum Direction 
	{
		Left,
		Right,
		Forward,
		Back,
		None
	};

	public enum DetectionState
	{
		Attack,
		Alert,
		InteractEnabled,
		None
	};

	public float speed = 1.0f;
	public int health = 100;
	public MovementState movementType = MovementState.Stationary;
	public DetectionState detectionType = DetectionState.None;
	public Condition condition = Condition.None;

	[HideInInspector] public LifeState lifeState = LifeState.Alive;
	[HideInInspector] public Direction direction = Direction.None; 

	[HideInInspector] public Vector3 gridSnapVector = new Vector3(0.5f, 0.5f, 0.5f);

	public void Awake()
	{
		GM = GameMaster.GM;
	}


	/// <summary>
	/// Converts passed Direction into a Vector 3
	/// </summary>
	/// <returns>The to vector3.</returns>
	/// <param name="_direction">Direction.</param>
	public Vector3 Direction_To_Vector3 (Direction _direction)
	{
		Vector3 _directionVector;
		// convert enum to vector 3
		switch (_direction)
		{
			case Direction.Left:
				_directionVector = Vector3.left;
				break;
			case Direction.Right:
				_directionVector = Vector3.right;
				break;
			case Direction.Forward:
				_directionVector = Vector3.forward;
				break;
			case Direction.Back:
				_directionVector = Vector3.back;
				break;
			default:
				_directionVector = Vector3.zero;
				break;
		}
		return _directionVector;
	}

	public void Move_Entity (Vector3 _moveDirection)
	{
		// Allows for future slow mo
		gameObject.transform.Translate(_moveDirection * speed * Time.deltaTime * (1 / Time.timeScale));
	}

	public void Look_Turn (Vector3 _lookDirection)
	{
		// Allows for future slow mo
		gameObject.transform.Rotate(_lookDirection * Time.deltaTime * (1 / Time.timeScale));
	}

	virtual public void Take_Damage (int _damage)
	{
		health -= _damage;
		Check_LifeState();
	}

	virtual public void Take_Damage (int _damage, Weapon.WeaponType _weaponType)
	{
		health -= _damage;
		Check_LifeState();
	}

	virtual public void Attack(GameObject _target)
	{
		//_target.Take_Damage();
	}

	virtual public void Alert ()
	{
		GM.Add_Detected();
	}


	virtual public void Check_LifeState ()
	{
		if (health <= 0)
		{
			lifeState = LifeState.Dead;
		}
	}



}
