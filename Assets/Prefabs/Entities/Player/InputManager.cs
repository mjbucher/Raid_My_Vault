using UnityEngine;
using System.Collections;
using AStar;

public class InputManager : MonoBehaviour 
{
	public bool onMobile;
	public Input singleTap;

	PathfindingUnit pathUnit;



	public void Awake ()
	{
		pathUnit = GetComponent<PathfindingUnit>();


	}

	void Update ()
	{
		if (Input.GetMouseButtonDown(0))
		{
			Debug.Log("mouse clicked");
			Ray_Check();
		}
	}

	public void Ray_Check()
	{
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		Debug.Log("casting ray");
		//Debug.DrawRay(Input.mousePosition, Camera.main.ScreenPointToRay(Input.mousePosition));
		if (Physics.Raycast(ray, out hit))
		{
			Debug.Log("Hit something");
			// here maybe add a check for if the target is on layer walkable, if not do nothing?
			Debug.DrawRay(ray.origin, hit.point, Color.cyan);
			Debug.Log("Stop pathing");
			pathUnit.StopMoving();
			Debug.Log("Create new path");
			pathUnit.Update_Path(hit.collider.transform);
		}
		else
		{
			Debug.Log("hit nothing");
		}
	}
}
