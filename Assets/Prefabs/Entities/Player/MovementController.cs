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
       
    }

    public void Start ()
    {
        agent.autoBraking = false;
        if (points.Length > 0)
            transform.position = points[0].position;
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
        // Returns if no points have been set up
        if (points.Length == 0)
            return;

        // Set the agent to go to the currently selected destination.
        agent.destination = points[destPoint].position;
        Debug.Log(agent.destination);
        // Choose the next point in the array as the destination,
        // cycling to the start if necessary.
        destPoint = (destPoint + 1) % points.Length;
        if (destPoint == points.Length)
            destPoint = 0;
    }



    


}
