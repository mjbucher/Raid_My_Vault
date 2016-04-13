/// <summary>
/// This is made using the tutorials found at https://www.youtube.com/playlist?list=PLFt_AvWsXl0cq5Umv3pMC9SPnKjfp9eGW By: Sebastian Lague
/// </summary>

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace AStar
{
    [ExecuteInEditMode]
	public class PathfindingGrid : MonoBehaviour 
	{
		public bool liveEdit;
		public LayerMask unwalkableMask;
		public Vector2 gridWorldSize;
		public float nodeRadius = 0.5f;
		public bool gridOnNodeCenter = true;
		PathfindingNode[,] grid;
		public bool showGizmos = false;
        public bool showNodes = false;
        public bool generateGrid = false;
        public int size;
        public bool liveResize = false;
        public Transform bgQuad;

		float nodeDiameter;
		int gridSizeX, gridSizeY;

        public int MaxSize
        {
            get { return gridSizeX * gridSizeY; }
        }

        void Awake ()
		{
			nodeDiameter = nodeRadius * 2;
			gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
			gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);
            //bgQuad = GetComponentInChildren<Transform>();
            liveEdit = false;
		}
		
        void Start()
        {
            CreateGrid();
        }

        public void Update ()
        {
            if (liveEdit)
            {
                if (generateGrid)
                {
                    CreateGrid();
                    Debug.Log("Grid Generated");
                    generateGrid = false;
                }
                if (liveResize)
                {
                    ResizeGrid();
                }
            }
          
        }

		void CreateGrid ()
		{
			grid = new PathfindingNode[gridSizeX, gridSizeY];
			Vector3 worldBottomLeft = transform.position - (Vector3.right * gridWorldSize.x/2) - (Vector3.forward * gridWorldSize.y/2); // center - left - bottom  [(0,0,0)-(-1,0,0)-(0,0,-1) = (-1,0,-1)]

			for (int x = 0; x < gridSizeX; x++)
			{
				for (int y = 0; y < gridSizeY; y++)
				{
					//Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * nodeDiameter /*+ nodeRadius*/) + Vector3.forward * (y * nodeDiameter /*+ nodeRadius*/); // bottome left + current x + current y, current x = x  for iterator * node diameter for diatacne between+ node ra
                    Vector3 worldPoint = worldBottomLeft + new Vector3(x * nodeDiameter /*+ nodeRadius*/ , 0,  y * nodeDiameter /*+ nodeRadius*/);
                    // allows for gridnodes to be based  on unity centered coodinates <-- toggleable
                    if (gridOnNodeCenter)
					{
                        //worldPoint = worldPoint + (Vector3.one * nodeRadius); 
                        worldPoint.x += nodeRadius;
                        worldPoint.z += nodeRadius;
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

		public PathfindingNode GetNodeFromWorldPoint (Vector3 _worldPosition)
		{	
			// world plane and gird plain do not match up! that is the issue!!!
			float percentX = ((_worldPosition.x + (gridWorldSize.x / 2)) / gridWorldSize.x) + transform.position.x; // + 0.5f;
			float percentY = ((_worldPosition.z + (gridWorldSize.y / 2)) / gridWorldSize.y) + transform.position.z; // + 0.5f;
			/// *** Rounding does not take into account the maximum (fine for start, but not good for final) In fact will never pick the final properly
			percentX = Mathf.Clamp01(percentX);
			percentY = Mathf.Clamp01(percentY);
			Debug.Log("percentX :" + percentX + " percentY: " + percentY);
			int x = Mathf.RoundToInt((gridSizeX - 1 ) * percentX);
			int y = Mathf.RoundToInt((gridSizeY - 1 ) * percentY);
			//x += (Mathf.RoundToInt(gridWorldSize.x) / 2);
			//y += (Mathf.RoundToInt(gridWorldSize.y) / 2);
			Debug.Log("x: " + Mathf.RoundToInt(_worldPosition.x) + " y: " + Mathf.RoundToInt(_worldPosition.z));
            // make sure x isnt negative
            //x = x < 0 ? x + gridSizeX : x;
            //y = y < 0 ? y + gridSizeY : x;
            return grid[Mathf.RoundToInt(_worldPosition.x), Mathf.RoundToInt(_worldPosition.z)];
			//return grid[x,y];
		}
        

        public void ResizeGrid ()
        {
            if (gridWorldSize.x != size && gridWorldSize.y != size)
            {
                gridWorldSize = new Vector2(size, size);
                transform.position = new Vector3(size / 2.0f, 1, size / 2.0f);
                bgQuad.localScale = new Vector3(size, size, 1);
            }
        }

		void OnDrawGizmos()
		{
			if (showGizmos)
			{
				Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, 1, gridWorldSize.y));
				if (grid != null && showNodes)
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

