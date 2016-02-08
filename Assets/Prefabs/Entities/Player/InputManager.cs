using UnityEngine;
using System.Collections;
using AStar;

public class InputManager : MonoBehaviour 
{
	public bool onMobile;
	public Input singleTap;
	public Camera cameraMain;
	 
	PathfindingUnit pathUnit;



	public void Awake ()
	{
		pathUnit = GetComponent<PathfindingUnit>();
		cameraMain = Camera.main;


	}

	void Update ()
	{
		if (Input.GetMouseButtonDown(0))
		{
			//Debug.Log("mouse clicked");
			Ray_Check();
		}
	}

	public void Ray_Check()
	{
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		Debug.Log (ray);
		//ray.direction = Vector3.forward;
		RaycastHit hit;
		//Debug.Log("casting ray");
		//Debug.DrawRay(Input.mousePosition, Camera.main.ScreenPointToRay(Input.mousePosition));
		if (Physics.Raycast(ray.origin, ray.direction, out hit, 1000.0f))
		{
			//Debug.Log("Hit something");
			// here maybe add a check for if the target is on layer walkable, if not do nothing?
			Debug.DrawRay(ray.origin, hit.point, Color.cyan);
			//Debug.Log("Stop pathing");
			pathUnit.StopMoving();
			//Debug.Log("Create new path");
			pathUnit.Update_Path(hit.collider.transform);
		}
		else
		{
			//Debug.Log("hit nothing");
		}
	}
}
