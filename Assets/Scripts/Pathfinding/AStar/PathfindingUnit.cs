using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace AStar
{
	public class PathfindingUnit : MonoBehaviour 
	{
		public Transform target;
		float speed = 1.0f;
        Entity unit;
        Player player;
        Enemy enemy;
        //Vector3[] path;
        public List<Vector3> path = new List<Vector3>();
		int targetIndex;
        public Vector3 targetPos;


        void Awake ()
        {
            unit = GetComponent<Entity>();
            
        }

		void Start()
		{
            //Debug.Log("ResourceRequest");
            if (GetComponent<Enemy>() == true)
            {
                enemy = GetComponent<Enemy>();
                speed = enemy.speed;
            }
            else if (GetComponent<Player>() == true)
            {
                player = GetComponent<Player>();
                speed = player.speed;
            }
        }

		public void Update_Path(Transform _target)
		{
			//Debug.Log("update path called");
			//Debug.Log("Stopping current path");
			//StopCoroutine("FindPath"); //*** figure out another way to stop unit? Maybe diable movement script?
			target = _target;
			//Debug.Log("requesting new path");
			//Debug.Log("current position: " + transform.position);
			//Debug.Log("target position: " + target.position);
			PathfindingRequestManager.RequestPath(transform.position, target.position, OnPathFound);
		}
        public void Update_Path(Vector3 _target)
        {
            //Debug.Log("update path called");
            //Debug.Log("Stopping current path");
            //StopCoroutine("FindPath"); //*** figure out another way to stop unit? Maybe diable movement script?
            targetPos = _target;
            //Debug.Log("requesting new path");
            Debug.Log("From Update_Path = current position: " + transform.position + " ::::: target position: " + targetPos);
            //Debug.Log("target position: " + target);
            PathfindingRequestManager.RequestPath(transform.position, targetPos, OnPathFound);
        }

        public void StopMoving()
		{
			Update_Path(transform.position);
		}

		public void OnPathFound(List<Vector3> newPath, bool pathSuccessful)
		{
			Debug.Log("OnPathFound called: " + pathSuccessful );
			if (pathSuccessful)
			{
				path = newPath;
				//StopCoroutine("FollowPath");
				Debug.Log("Follow path stopped");
                FollowPath();
			}
			else
			{
				Debug.Log("Unsucessful");
			}

		}

		void FollowPath ()
		{
			Debug.Log("Follow Path started");
            /// *** issue is in follow path, and is an index error
            Debug.Log("First point of path: " + path[0]);
			Vector3 currentWaypoint = path[0];
            Debug.Log("Last point of path: " + path[path.Count - 1]);
			while (true)
			{
                //if (Input.GetKeyDown(KeyCode.Space) == true)
                //{
                if (transform.position == currentWaypoint)
                {
                    targetIndex++;
                Debug.Log("Target index is now: " + targetIndex);
                    if (targetIndex >= path.Count - 1)
                    {
                    targetIndex = 0;
                    //yield break;
                    break;  
                    }
                    currentWaypoint = path[targetIndex];
                }
                // add rotation here
                transform.LookAt(currentWaypoint);
                //transform.LookAt(new Vector3(target.position.x, 0.5f, -target.position.z));
                float step = speed * Time.deltaTime;
                transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, step);
                //transform.Translate(currentWaypoint);   
                //yield return null;
               Debug.Log("Moved to: " + transform.position);
                //}
			}
            Debug.Log("pathfinding complete");
		}

		public void OnDrawGizmos()
		{
			if (path != null)
			{
				for(int i = targetIndex; i < path.Count; i++)
				{
					Gizmos.color = Color.black;
					Gizmos.DrawCube(path[i], Vector3.one);

					if (i == targetIndex)
					{
						Gizmos.DrawLine(transform.position, path[i]);
					}
					else
					{
						Gizmos.DrawLine(path[i-1], path[i]);
					}
				}
			}
		}
	}
}

