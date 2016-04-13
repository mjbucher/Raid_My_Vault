using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class AttackManager : MonoBehaviour 
{
	[Space(10)]
	[Header("Weapon Variables")]
	public Transform weaponSpawnLocation;
	public GameObject weaponPrefab;
	[HideInInspector]
	public GameObject weaponObject;
	public bool reloadWeapon = false;

	Weapon weapon;

	void Awake ()
	{
		SpawnWeapon();
	}


	void Update ()
	{
		if (reloadWeapon)
		{
			DestroyImmediate(weaponObject);
			SpawnWeapon();
			reloadWeapon = false;
			Debug.Log("Weapon Reloaded on: " + gameObject.name);
		}
	}


	void SpawnWeapon ()
	{
		// spawn weapon
		weaponObject = (GameObject)Instantiate(weaponPrefab, weaponSpawnLocation.position, Quaternion.identity);
		weaponObject.transform.SetParent(weaponSpawnLocation);
		// grab weapon script
		weapon = weaponObject.GetComponent<Weapon>();
	}
	// call the weapon's attack
	public void Attack ()
	{
		Debug.Log("FIRING!!!!!!!!!!!");
		weapon.Attack();
	}

	public void AssignWeapon (GameObject _weapon)
	{
		weaponObject = _weapon;
	}

    public void SpaceOverride ()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            weapon.Attack();
        }
    }

}
