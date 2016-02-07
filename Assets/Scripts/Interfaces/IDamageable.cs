using UnityEngine;
using System.Collections;


/// <summary>
/// Meant for Objects that have health and can be damaged.
/// 
/// Contains: void Take_Damage(int _damage, Weapon.WeaponType _weaponType)
/// </summary>
public interface IDamageable
{
	void Take_Damage(int _damage, DamageType _damageType);
}
