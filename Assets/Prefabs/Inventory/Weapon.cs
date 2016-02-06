using UnityEngine;
using System.Collections;

public class Weapon : MonoBehaviour 
{
	public enum WeaponType
	{
		EMP,
		Explosive,
		Bullet,
		None
	};

	public float range = 0;
	public int weaponDamage = 0;
	public float damageRadius = 0;


}
