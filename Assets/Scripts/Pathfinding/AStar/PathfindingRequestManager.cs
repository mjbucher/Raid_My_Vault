/// <summary>
/// This is made using the tutorials found at https://www.youtube.com/playlist?list=PLFt_AvWsXl0cq5Umv3pMC9SPnKjfp9eGW By: Sebastian Lague
/// </summary>

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System; 

namespace AStar
{
	public class PathfindingRequestManager : MonoBehaviour 
	{
		Queue<PathRequest> pathRequestQueue = new Queue<PathRequest>();
		PathRequest currentPathRequest;

		static PathfindingRequestManager instance;
		Pathfinding pathfinding;

		bool isProcessingPath;

		void Awake ()
		{
			instance = this;
			pathfinding = GetComponent<Pathfinding>();
		}



		public static void RequestPath (Vector3 pathStart, Vector3 pathEnd, Action <List<Vector3>, bool> callback)
		{
			PathRequest newRequest = new PathRequest(pathStart, pathEnd, callback);
			instance.pathRequestQueue.Enqueue(newRequest);
			instance.TryProcessNext();
		}

		void TryProcessNext()
		{
			//Debug.Log("TryProcessNext called");
			if(!isProcessingPath && pathRequestQueue.Count > 0)
			{
				
				currentPathRequest = pathRequestQueue.Dequeue();
				isProcessingPath = true;
				//Debug.Log("calling start find path");
				pathfinding.StartFindPath(currentPathRequest.pathStart, currentPathRequest.pathEnd);
			}
		}

		public void FinishedProcessingPath(List<Vector3> path, bool success)
		{
			//UnityEngine.Debug.Log("FinishedProcessingPath called success");
			//UnityEngine.Debug.Log("FinishedProcessingPath: path = " + path + ":: success = " + success);
			currentPathRequest.callback(path, success);
			isProcessingPath = false;
			TryProcessNext();
		}
			

		struct PathRequest
		{
			public Vector3 pathStart;
			public Vector3 pathEnd;
			public Action<List<Vector3>, bool> callback;

			public PathRequest (Vector3 _start, Vector3 _end, Action <List<Vector3>, bool> _callback)
			{
				pathStart = _start;
				pathEnd = _end;
				callback = _callback;
			}
		}

	}

}
