using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SphereCollider))]
public class Trap : MonoBehaviour 
{
	public TriggerType triggerType = TriggerType.None;
	public float detectionRadius = 0.0f;

	SphereCollider collider;

	void Awake ()
	{
		collider = GetComponent<SphereCollider>();
	}

}
