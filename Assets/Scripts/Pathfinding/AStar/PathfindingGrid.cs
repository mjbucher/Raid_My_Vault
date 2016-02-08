/// <summary>
/// This is made using the tutorials found at https://www.youtube.com/playlist?list=PLFt_AvWsXl0cq5Umv3pMC9SPnKjfp9eGW By: Sebastian Lague
/// </summary>

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace AStar
{
	public class PathfindingGrid : MonoBehaviour 
	{
		public bool displayGridGizmos;
		public LayerMask unwalkableMask;
		public Vector2 gridWorldSize;
		public float nodeRadius = 0.5f;
		public bool gridOnNodeCenter = true;
		PathfindingNode[,] grid;
		public bool showGizmos = false;

		float nodeDiameter;
		int gridSizeX, gridSizeY;

		void Awake ()
		{
			nodeDiameter = nodeRadius * 2;
			gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
			gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);

			CreateGrid();
		}
			
		public int MaxSize
		{
			get
			{
				return gridSizeX * gridSizeY;
			}
		}

		void CreateGrid ()
		{
			grid = new PathfindingNode[gridSizeX, gridSizeY];
			Vector3 worldBottomLeft = transform.position - Vector3.right * gridWorldSize.x/2 - Vector3.forward * gridWorldSize.y/2; 

			for (int x = 0; x < gridSizeX; x++)
			{
				for (int y = 0; y < gridSizeY; y++)
				{
					Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.forward * (y * nodeDiameter + nodeRadius);
					// allows for gridnodes to be based  on unity centered coodinates <-- toggleable
					if (gridOnNodeCenter)
					{
						worldPoint = worldPoint + (Vector3.one * nodeRadius); 
					}
					// may need something here to eliminate areas outside of grid from play (past walls
					bool walkable = !(Physics.CheckSphere(worldPoint, nodeRadius, unwalkableMask));
					grid[x,y] = new PathfindingNode(walkable, worldPoint, x, y);
				}
			}

		}

		public List<PathfindingNode> GetNeighbors (PathfindingNode node)
		{
			List<PathfindingNode> neighbors = new List<PathfindingNode>();

			for (int x = -1; x <= 1; x++)
			{
				for (int y = -1; y <= 1; y++)
				{
					if (x ==0 && y == 0)
					{
						continue;
					}
					else
					{
						int checkX = node.gridX + x;
						int checkY = node.gridY + y;

						if (checkX >- 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY)
						{
							neighbors.Add(grid[checkX, checkY]);
						}
					}

				}
			}

			return neighbors;
		}

		public PathfindingNode GetNodeFromWorldPoint (Vector3 worldPosition)
		{	
			// world plane and gird plain do not match up! that is the issue!!!
			float percentX = ((worldPosition.x + (gridWorldSize.x / 2)) / gridWorldSize.x); // + 0.5f;

			float percentY = ((worldPosition.z + (gridWorldSize.y / 2)) / gridWorldSize.y); // + 0.5f;
			percentX = Mathf.Clamp01(percentX);
			percentY = Mathf.Clamp01(percentY);
			Debug.Log("percentX :" + percentX + " percentY: " + percentY);
			int x = Mathf.RoundToInt((gridSizeX - 1 ) * percentX);
			int y = Mathf.RoundToInt((gridSizeY - 1 ) * percentY);
			//x += (Mathf.RoundToInt(gridWorldSize.x) / 2);
			//y += (Mathf.RoundToInt(gridWorldSize.y) / 2);
			Debug.Log("x: " + x + " y: " + y);
			// make sure x isnt negative
			//x = x < 0 ? x + gridSizeX : x;
			//y = y < 0 ? y + gridSizeY : x;

			return grid[x,y];
		}



		void OnDrawGizmos()
		{
			if (showGizmos)
			{
				Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, 1, gridWorldSize.y));
				if (grid != null && displayGridGizmos)
				{
					foreach (PathfindingNode node in grid)
					{
						Gizmos.color = node.walkable ? Color.white : Color.black ;
						Gizmos.DrawCube(node.worldPosition, Vector3.one * (nodeDiameter - 0.25f));
					}
				}
			}
		}



	}	
}

