using UnityEngine;
using System.Collections;
using RMV;
using System.Collections.Generic;
using UnityEditor;

public class MovementController : MonoBehaviour 
{
	[HideInInspector] public Entity thisEntity;
    // used for new coordinate points
    //public Path targetPath; 
    public NavMeshPath path;
    int currentPathIndex;
    GameObject currentPathPoint;


    public Transform[] points;
    private int destPoint = 0;
    private NavMeshAgent agent;

    private Vector3 nextTarget;
    bool chasing = false;

    public void Awake()
    {
        thisEntity = gameObject.GetComponent<Entity>();
        agent = GetComponent<NavMeshAgent>();
        agent.destination = transform.position;
    }

    public void Start ()
    {
        agent.autoBraking = false;
        //if (points.Length > 0)
        //transform.localPosition = points[0].localPosition;
        GotoNextPoint();
    }


	public void Update ()
	{
        //if (Vector3.Distance(transform.position, targetPath.currentPoint.transform.position) < 0.5f)
        //{
        //    targetPath.UpdateToNextPoint();
        //}
        if (agent.remainingDistance < 1.0f)
            GotoNextPoint();
    }

    void GotoNextPoint()
    {
        Debug.Log("Go to next point called on:" + gameObject.name);
        // Returns if no points have been set up
        if (points.Length == 0)
            return;
        Debug.Log("confirmed more than one point on:" + gameObject.name);
        // Set the agent to go to the currently selected destination.
        agent.destination = points[destPoint].position;
        Debug.Log("new destination set @: " + agent.destination + " on: " + gameObject.name);
        // Choose the next point in the array as the destination,
        // cycling to the start if necessary.
        destPoint = (destPoint + 1) % points.Length;
        Debug.Log("destnation point index is now: " + destPoint + " on:" + gameObject.name);
       // if (destPoint == points.Length)
            ///destPoint = 0;
    }



    


}
