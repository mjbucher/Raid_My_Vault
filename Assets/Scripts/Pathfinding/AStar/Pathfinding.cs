/// <summary>
/// This is made using the tutorials found at https://www.youtube.com/playlist?list=PLFt_AvWsXl0cq5Umv3pMC9SPnKjfp9eGW By: Sebastian Lague
/// </summary>

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System;
using UD = UnityEngine.Debug;

namespace AStar
{
	public class Pathfinding : MonoBehaviour 
	{
		PathfindingRequestManager requestManager;
		PathfindingGrid grid;
		public int diagonalWeight = 1;
		public bool useSimplifyPath = true;

		void Awake ()
		{
			grid = GetComponent<PathfindingGrid>();
			requestManager = GetComponent<PathfindingRequestManager>();
		}
			
			
		public void StartFindPath (Vector3 startPos, Vector3 targetPos)
		{
			//UnityEngine.Debug.Log("Coroutine StartFindPath called");
			StartCoroutine(FindPath(startPos, targetPos));
		}


		IEnumerator FindPath (Vector3 startPos, Vector3 targetPos)
		{
			//UnityEngine.Debug.Log("starting stopwatch in FindPath");
			Stopwatch sw = new Stopwatch();
			sw.Start();

			Vector3[] waypoints = new Vector3[0];
			bool pathSuccess = false;

			PathfindingNode startNode = grid.GetNodeFromWorldPoint(startPos);
			PathfindingNode targetNode = grid.GetNodeFromWorldPoint(targetPos);			

			//UnityEngine.Debug.Log("if statement starting");
			UD.Log("target.walkable: " + targetNode.walkable + " ::: start.walkable = " + startNode.walkable);
			if (startNode.walkable && targetNode.walkable)
			{
				//UD.Log("both start and target are walkable");
				PathfindingHeap<PathfindingNode> openSet = new PathfindingHeap<PathfindingNode>(grid.MaxSize);
				HashSet<PathfindingNode> closedSet = new HashSet<PathfindingNode>(); 
				openSet.Add(startNode);
				while (openSet.Count > 0)
				{
					PathfindingNode currentNode = openSet.RemoveFirst();

					closedSet.Add(currentNode); 
					if (currentNode == targetNode)
					{
						sw.Stop();
						print ("Path Found: " + sw.ElapsedMilliseconds + " ms");

						pathSuccess = true;
						break;
					}

					foreach(PathfindingNode neighbor in grid.GetNeighbors(currentNode))
					{
						if (!neighbor.walkable || closedSet.Contains(neighbor))
						{
							continue;
						}

						int newMovementCostToNeighbor = currentNode.gCost + GetDistance(currentNode, neighbor);
						if (newMovementCostToNeighbor < neighbor.gCost || !openSet.Contains(neighbor))
						{
							neighbor.gCost = newMovementCostToNeighbor;
							neighbor.hCost = GetDistance(neighbor, targetNode);
							neighbor.parent = currentNode;
							//UnityEngine.Debug.Log(currentNode);

							if (!openSet.Contains(neighbor))
							{
								openSet.Add(neighbor);
							}
							else
							{
								openSet.UpdateItem(neighbor);	
							}
						}
					}
				}
				yield return null;
				if (pathSuccess)
				{
					waypoints = RetracePath(startNode, targetNode);
				}
				//UnityEngine.Debug.Log("calling to request manager");
				requestManager.FinishedProcessingPath(waypoints, pathSuccess);
			}
		}

		Vector3[] RetracePath(PathfindingNode startNode, PathfindingNode endNode)
		{
			List<PathfindingNode> path = new List<PathfindingNode>();
			PathfindingNode currentNode = endNode;

			while (currentNode != startNode)
			{
				path.Add(currentNode);
				currentNode = currentNode.parent;
			}

			Vector3[] waypoints = SimplifyPath(path);
			
			Array.Reverse(waypoints);
			return waypoints;
		}

		Vector3[] SimplifyPath(List<PathfindingNode> path)
		{
			List<Vector3> waypoints = new List<Vector3>();
			Vector2 directionOld = Vector2.zero;

			for (int i = 1; i < path.Count; i++)
			{
				Vector2 directionNew = new Vector2(path[i-1].gridX - path[i].gridX, path[i-1].gridY - path[i].gridY);
				if (directionNew != directionOld)
				{
					waypoints.Add(path[i].worldPosition);
				}
				directionOld = directionNew;
			}
			return waypoints.ToArray();
		}
			
		public int GetDistance (PathfindingNode nodeA, PathfindingNode nodeB) 
		{
			int distX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
			int distY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

			int diagonal  = 14; // make this more than double strait if you want to eliminate diagonals
			int strait = 10;
			int diagonalWeighting  = diagonalWeight;

			if (distX > distY)
			{
				return ((diagonal * distY * diagonalWeighting) + (strait * (distX - distY)));
			}
			else
			{
				return ((diagonal * distX * diagonalWeighting) + (strait * (distY - distX)));
			}
		}

	}
}

