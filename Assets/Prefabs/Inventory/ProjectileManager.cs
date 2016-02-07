using UnityEngine;
using System.Collections;
using DG.Tweening;

public class ProjectileManager : MonoBehaviour
{
	public GameObject projectileObject;
	public WeaponType weaponType;
	public float velocity;

	Weapon weapon;

	void Awake ()
	{
		weapon = GetComponent<Weapon>();
		DOTween.Init();
	}

	public void FireProjectile (Ray targetRay) 
	{
		// spawn the projectile
		GameObject projectile = Instantiate(projectileObject);
		// set it's parent to this
		projectile.transform.parent.SetParent(gameObject.transform);
		// find target from ray
		Vector3 _targetPoint = (projectile.transform.position + targetRay.direction);
		// calculate time it should take based on distance
		float _distance = Vector3.Distance(projectile.transform.position, _targetPoint);
		float _time = _distance / velocity;
		// move the projectile and destorys itself when hitting something (calls detonate)
		projectile.transform.DOLocalMove(_targetPoint, _time, false);
	}

	public void Detonate ()
	{
		
	}








}
