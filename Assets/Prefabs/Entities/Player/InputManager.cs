using UnityEngine;
using System.Collections;
using AStar;
using System.Collections.Generic;

public class InputManager : MonoBehaviour 
{
	public bool onMobile;
	public Input singleTap;
	public Camera playerCamera;
	public Vector3 worldTarget;
    public LayerMask raycastLayerExclusion;
    public GameObject playerModel;
    public bool isWalking = false;
    private GameMaster GM;
    private NavMeshAgent agent;


    Player player;


    PathfindingUnit pathUnit;

    GameObject ds;

    public void Awake ()
	{
        GM = GameObject.FindGameObjectWithTag("GameMaster").GetComponent<GameMaster>();
		pathUnit = GetComponent<PathfindingUnit>();
        player = GetComponent<Player>();
        agent = GetComponent<NavMeshAgent>();
        agent.autoBraking = false;
	}

    void Update ()
	{
        if (onMobile)
        {
            CheckMobileControls();   
        }
        else
        {
            CheckPCControls();
        }
	}

    void CheckMobileControls ()
    {
        if (Input.GetTouch(0).phase == TouchPhase.Began)
        {
            Debug.Log("Touch Pressed");
            Ray_Check(Input.GetTouch(0).position);
        }
    }

    void CheckPCControls ()
    {
        Debug.Log("Checking PC Controls");
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("mouse clicked");
            Ray_Check(Input.mousePosition);
        }
        if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
        {
            //Transform tempT = gameObject.transform;
            //tempT.position += Vector3.right * Input.GetAxisRaw("Horizontal") * Time.deltaTime * player.speed;
            //Debug.Log("Stop pathing");
            //pathUnit.StopMoving();
            //Debug.Log("Create new path");
            Transform tempT = gameObject.transform;
            float hor = Input.GetAxisRaw("Horizontal");
            float ver = Input.GetAxisRaw("Vertical");
            tempT.LookAt(tempT.position + (hor * Vector3.right) + (ver * Vector3.forward));
            tempT.position += ((Vector3.forward * ver) + (Vector3.right * hor)) * Time.deltaTime * player.speed;
            //play walking
            isWalking = true;
            //transform.rotation = tempT.rotation;
            //pathUnit.Update_Path(tempT);
        }
        else
        {
            isWalking = false;
        }
    }

	public void Ray_Check(Vector3 _target)
	{
		Ray ray = playerCamera.ScreenPointToRay(_target);
		//Debug.Log (ray);
		//ray.direction = Vector3.forward;
		RaycastHit hit;
        Debug.Log("casting ray");
        //Debug.DrawRay(Input.mousePosition, playerCamera.ScreenPointToRay(_target));
        if (Physics.Raycast(ray.origin, ray.direction, out hit))
		{
            Debug.Log("ray hit something!");
            //Debug.Log("Hit something");
            // here maybe add a check for if the target is on layer walkable, if not do nothing?
            //Debug.DrawRay(ray.origin, hit.point, Color.cyan);
            Debug.DrawLine(ray.origin, hit.point, Color.cyan, 1.2f, false);
			//Debug.Log("Stop pathing");
			//pathUnit.StopMoving();
			Debug.Log("Create new path");
            // float nX = hit.point.x % 0.5f;
            // float nZ = hit.point.x % 0.5f == 0 ? hit.point.x : (int)hit.point.x;
            //worldTarget = RoundToGrid(hit.point);//new Vector3(nX, hit.point.y, nZ);
            worldTarget = hit.point;  //hit.collider.gameObject.transform.position + RoundToGrid(hit.point) + new Vector3( -1, 0, -1); // this is an offset
            //pathUnit.Update_Path(worldTarget);
            // debug sight
            agent.destination = worldTarget;
            
            // debug sphere for checking raycast
            if (ds != null)
                Destroy(ds);
            ds = Instantiate(GameObject.FindWithTag("Debug Locator")) as GameObject;
            ds.transform.position = worldTarget;// + Vector3.up * 0.5f; // hit.point;
		}
		else
		{
			Debug.Log("hit nothing");
		}
	}

    public Vector3 RoundToGrid(Vector3 _hitPoint)
    {
        Vector3 roundedPoint = _hitPoint;

        roundedPoint.x = Mathf.RoundToInt(roundedPoint.x);
        roundedPoint.y = Mathf.RoundToInt(roundedPoint.y);
        roundedPoint.z = Mathf.RoundToInt(roundedPoint.z);

        // round x
        //int n = Mathf.RoundToInt(_hitPoint.x);
        //if (_hitPoint.x - n < 0.5f)
        //{
        //    roundedPoint.x = (float) n;
        //}
        //else
        //{
        //    roundedPoint.x = (float)(n + 0.5f);
        //}
        // round y


        // round z


        return roundedPoint;  
    }

    public void OnDrawGrizmos ()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawCube(worldTarget, Vector3.one * 0.3f);
    }
}
