using UnityEngine;
using System.Collections;
using AStar;
using System.Collections.Generic;

public class InputManager : MonoBehaviour 
{
	public bool onMobile;
    public bool liveDebug;
	public Input singleTap;
    [SerializeField]
    private Camera playerCamera;
	public Vector3 worldTarget;
    public LayerMask raycastLayerExclusion;
    public GameObject playerModel;
    public bool isWalking = false;
    private GameMaster GM;
    private NavMeshAgent agent;


    Player player;


    PathfindingUnit pathUnit;

    GameObject debugSphere;

    public void Awake ()
	{
        GM = GameObject.FindGameObjectWithTag("GameMaster").GetComponent<GameMaster>();
		pathUnit = GetComponent<PathfindingUnit>();
        player = GetComponent<Player>();
        agent = GetComponent<NavMeshAgent>();
        //agent.autoBraking = false;
	}

    public void Start ()
    {
        agent.autoBraking = false;
        agent.speed = player.speed;
        playerCamera = GameObject.FindGameObjectWithTag("Isometric Camera").GetComponent<Camera>();
    }

    void Update ()
	{
        if (onMobile)
            CheckMobileControls();   
        else
            CheckPCControls();
        
        //CheckMobileControls();
        //CheckPCControls();
	}

    void CheckMobileControls ()
    {
        if (Input.GetTouch(0).phase == TouchPhase.Began)
        {
            Debug.Log("Touch Pressed");
            RayCheck(Input.GetTouch(0).position);
        }
    }

    void CheckPCControls ()
    {
        Debug.Log("Checking PC Controls");
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("mouse clicked");
            RayCheck(Input.mousePosition);
        }
        if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
        {
            //Transform tempT = gameObject.transform;
            //tempT.position += Vector3.right * Input.GetAxisRaw("Horizontal") * Time.deltaTime * player.speed;
            //Debug.Log("Stop pathing");
            //pathUnit.StopMoving();
            //Debug.Log("Create new path");
            Vector3 tempPos = gameObject.transform.position;
            tempPos.x += Input.GetAxisRaw("Horizontal");
            tempPos.z += Input.GetAxisRaw("Vertical");
            //tempT.LookAt(tempT.position + (hor * Vector3.right) + (ver * Vector3.forward));
            //tempT.position += ((Vector3.forward * ver) + (Vector3.right * hor)) * Time.deltaTime * player.speed;
            //play walking
            isWalking = true;
            RayCheck(tempPos);
            //transform.rotation = tempT.rotation;
            //pathUnit.Update_Path(tempT);
        }
        else
        {
            isWalking = false;
        }
    }

	public void RayCheck(Vector3 _target)
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
            Debug.DrawLine(ray.origin, hit.point, Color.cyan, 1.2f, false);
			Debug.Log("Create new path");
            // float nX = hit.point.x % 0.5f;
            // float nZ = hit.point.x % 0.5f == 0 ? hit.point.x : (int)hit.point.x;
            //worldTarget = RoundToGrid(hit.point);//new Vector3(nX, hit.point.y, nZ);
            worldTarget = hit.point;  //hit.collider.gameObject.transform.position + RoundToGrid(hit.point) + new Vector3( -1, 0, -1); // this is an offset
            // debug sphere for checking raycast
            if (liveDebug == true)
            {
                if (debugSphere != null)
                    Destroy(debugSphere);
                debugSphere = Instantiate(GameObject.FindWithTag("Debug Locator")) as GameObject;
                debugSphere.transform.position = worldTarget;
            }

            // check what I hit
            string targetTag = hit.collider.gameObject.tag.ToLower();
            Debug.Log(targetTag);
            if (targetTag == "walkable" || targetTag == "untagged")
            {
                //agent.destination = transform.position; // stop moving
                //transform.LookAt(worldTarget); // look towards next destination
                agent.destination = worldTarget; // move towards destination
            }
            else if (targetTag == "enemy") { }
            //hit.collider.gameObject.GetComponent<Enemy>()
            else if (targetTag == "door") { }
            //player.checkInventory("key");
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

        return roundedPoint;  
    }

    public void OnDrawGrizmos ()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawCube(worldTarget, Vector3.one * 0.3f);
    }
}
