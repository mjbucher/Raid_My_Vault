/// <summary>
/// Entity. This Script handles universal stats for entities as well as health and death mechanisms for each
/// </summary>

using UnityEngine;
using System.Collections;

[RequireComponent(typeof(DeathSequence))]
[RequireComponent(typeof(HealthManager))]
public class Entity : MonoBehaviour 
{
	[HideInInspector] public GameMaster GM;

	[Header("Unit Stats")]
	[Tooltip("The amount of health this unit has at a maximun, int")] [Range(0, 400)]
	public int health = 100;
	[Tooltip("The speed at which a unit moves, float")] [Range(0.0f, 10.0f)]
	public float speed = 1.0f;
	[Tooltip("Shield health is deducted first before base")] [Range(0, 100)]
	public int shield = 0;

	[Header("Cost of Unit")]
	//[HideInInspector] 
	[Tooltip("The cost for this unit, as int")]
	public int cost = 0;
	[Header("Weakness of Unit")]
	[Tooltip("The weapon weakness of this unit")]
	public DamageType weakness = DamageType.None;
	[Tooltip("Weakness Multiplier")] [Range(0.0f, 5.0f)]
	public float weaknessMultiplier = 2.0f;

	// Current State variables
	[Header("Current State")]
	public LifeState lifeState = LifeState.Alive;
	public MovementState movementState = MovementState.Stationary;
	public DetectionState detectionState = DetectionState.None;
	public StatusEffect condition = StatusEffect.None;


    public bool isSelected;

	// Proporties for subclasses
	[HideInInspector] public HealthManager healthManager;
	[HideInInspector] public DeathSequence deathSequence; // needs a check for death couroutine

	[HideInInspector] public Direction direction = Direction.None; 
	[HideInInspector] public Vector3 gridSnapVector = new Vector3(0.5f, 0.5f, 0.5f);
    public NavMeshAgent agent;

	void Awake()
	{
		GM = GameMaster.GM;
		deathSequence = GetComponent<DeathSequence>();
		healthManager = GetComponent<HealthManager>();
        agent = GetComponent<NavMeshAgent>();

	}

	void Start ()
	{
		//Init coroutines
		StartCoroutine(Check_LifeState());
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
		
	public IEnumerator Check_LifeState ()
	{
		// check if dead
		if (lifeState == LifeState.Dead || lifeState == LifeState.FullDeath)
		{
            // start death sequence
            // stop checking life
            StartCoroutine(deathSequence.DeathInit());
            StopCoroutine(Check_LifeState());
        }
        yield return null;
    }



}
