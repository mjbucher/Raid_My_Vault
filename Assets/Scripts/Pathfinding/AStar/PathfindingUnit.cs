using UnityEngine;
using System.Collections;

namespace AStar
{
	public class PathfindingUnit : MonoBehaviour 
	{
		public Transform target;
		float speed = 1.0f;
		Vector3[] path;
		int targetIndex;

		void Start()
		{
			//Debug.Log("ResourceRequest");

		}

		public void Update_Path(Transform _target)
		{
			//Debug.Log("update path called");
			//Debug.Log("Stopping current path");
			StopCoroutine("FindPath");
			target = _target;
			//Debug.Log("requesting new path");
			Debug.Log("current position: " + transform.position);
			Debug.Log("target position: " + target.position);
			PathfindingRequestManager.RequestPath(transform.position, target.position, OnPathFound);
		}

		public void StopMoving()
		{
			Update_Path(transform);
		}

		public void OnPathFound(Vector3[] newPath, bool pathSuccessful)
		{
			Debug.Log("OnPathFound called: " + pathSuccessful );
			if (pathSuccessful)
			{
				path = newPath;
				//StopCoroutine("FollowPath");
				Debug.Log("Follow path stopped");
				//StartCoroutine("FollowPath");
			}
			else
			{
				Debug.Log("Unsucessful");
			}

		}

		IEnumerator FollowPath ()
		{
			//Debug.Log("Follow Path started");
			Vector3 currentWaypoint = path[0];

			while (true)
			{
				if (transform.position == currentWaypoint)
				{
					targetIndex++;
					if (targetIndex >= path.Length)
					{
						targetIndex = 0;
						yield break;
					}
					currentWaypoint = path[targetIndex];
				}
				// add rotation here
				//transform.LookAt(new Vector3(target.position.x, 0.5f, -target.position.z));
				transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, speed * Time.deltaTime);
				yield return null;
			}

		}

		public void OnDrawGizmos()
		{
			if (path != null)
			{
				for(int i = targetIndex; i < path.Length; i++)
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

