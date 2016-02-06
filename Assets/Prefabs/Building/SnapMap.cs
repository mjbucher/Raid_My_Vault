using UnityEngine;
using System.Collections;

public class SnapMap : MonoBehaviour 
{
	 
	public Rigidbody placeableObject;
	//mouse drag
	private Vector3 screenPoint;
	private Vector3 offset;
	//grid snapping
	public bool snapToGrid = true;
 

	void OnMouseDown()
	{
		if (Input.GetMouseButton(0))
		{
			screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);
			offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
		}
		if (Input.GetMouseButton(1))
		{
			Destroy(gameObject);
		}
	}

	void OnMouseDrag()
	{
		Vector3 cursorPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
		Vector3 cursorPosition = Camera.main.ScreenToWorldPoint(cursorPoint) + offset;
		transform.position = cursorPosition;
	}



 
	// Adjust position
	void Update ()
	{
		if (snapToGrid) 
		{
			transform.position = RoundTransform (transform.position);

		}
	}
	
 

	private Vector3 RoundTransform (Vector3 v)
	{
		return new Vector3(Mathf.Round(v.x),Mathf.Round(v.y),Mathf.Round(v.z));
	}



	 
}
