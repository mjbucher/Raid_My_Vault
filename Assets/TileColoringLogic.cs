using UnityEngine;
using System.Collections;
using DG.Tweening;

[ExecuteInEditMode]
public class TileColoringLogic : MonoBehaviour 
{
	//[HideInInspector]
	public bool inSight = false;
	public float fadeTime = 0.3f;
	public float updateTime = 0.5f;
	GameObject target;
	float range;
	float sightAngle;
	public MeshRenderer meshRenderer;

	void Awake ()
	{
		//meshRenderer = GetComponent<MeshRenderer>();
	}

	void Start ()
	{
		StartCoroutine("Active");
	}


	IEnumerator Active ()
	{
		while(inSight)
		{
			meshRenderer.enabled = true;
//			if (Vector3.Distance(transform.position, target.transform.position) <= range && Mathf.Abs(Vector3.Angle(target.transform.position, transform.position)) <= (sightAngle / 2.0f))
//			{
//				yield return new WaitForSeconds(updateTime);
//			}
//			else
//			{
//				inSight = false;	
//			}

		}
		yield return null;
		float alpha = meshRenderer.material.color.a; 
		meshRenderer.sharedMaterial.DOFade(0.0f, fadeTime);
		meshRenderer.enabled = false;
		meshRenderer.sharedMaterial.DOFade(alpha, 0.0f);
		target = null;
		StopCoroutine("Active");
	}

	/// <summary>
	/// Activate the specified _target, _range and _sightAngle.
	/// </summary>
	/// <param name="_target">Target.</param>
	/// <param name="_range">Range.</param>
	/// <param name="_sightAngle">Sight angle.</param>
	public void Activate (GameObject _target, float _range, float _sightAngle)
	{
		inSight = true;
		target = _target;
		range = _range;
		sightAngle = _sightAngle;
		StartCoroutine("Active");
	}


}
