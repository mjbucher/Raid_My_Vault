using UnityEngine;
using System.Collections;

public class LaserScript : MonoBehaviour 
{
	public int numOfShots;
	LineRenderer lineRenderer;

	void Awake ()
	{
		lineRenderer = GetComponent<LineRenderer>();
		lineRenderer.enabled = false;
	}

	void Update ()
	{
		if(Input.GetButtonDown("Fire1"))
		{
			StartCoroutine("FireLaser");
		}

	}


	IEnumerator FireLaser ()
	{
		lineRenderer.enabled  = true;
		while (Input.GetButton("Fire1"))
		{
			Ray ray = new Ray (transform.position, transform.forward);
			lineRenderer.SetPosition(0,ray.origin);
			lineRenderer.SetPosition(1,ray.GetPoint(100));

			yield return null;

		}

		//yield return new WaitForSeconds(0.5);
		lineRenderer.enabled = false;	


	}




}
