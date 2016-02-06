using UnityEngine;
using System.Collections;

/// <summary>
/// Has the ability to attack or damage someone / something.
/// Contains: void Attack(GameObject _target)
/// </summary>
public interface ICanAttack <GameObject>
{
	void Attack(GameObject _target);
}
