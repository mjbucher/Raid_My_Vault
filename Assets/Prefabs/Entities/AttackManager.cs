using UnityEngine;
using System.Collections;

public class AttackManager : MonoBehaviour 
{
	[Space(10)]
	[Header("Weapon Variables")]
	public Transform weaponSpawnLocation;
	public GameObject weaponObject;

	Weapon weapon;

	void Awake ()
	{
		// spawn weapon
		weaponObject = (GameObject)Instantiate(weaponObject, weaponSpawnLocation.position, Quaternion.identity);
		// gra weapon script
		weapon = weaponObject.GetComponent<Weapon>();
	}
		
	// call the weapon's attack
	public void Attack ()
	{
		weapon.Attack();
	}

	public void AssignWeapon (GameObject _weapon)
	{
		weaponObject = _weapon;
	}


}
