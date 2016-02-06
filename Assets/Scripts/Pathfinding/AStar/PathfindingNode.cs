/// <summary>
/// This is made using the tutorials found at https://www.youtube.com/playlist?list=PLFt_AvWsXl0cq5Umv3pMC9SPnKjfp9eGW By: Sebastian Lague
/// </summary>

using UnityEngine;
using System.Collections;
using System;

namespace AStar
{
	public class PathfindingNode : IHeapItem<PathfindingNode>
	{
		public bool walkable;
		public Vector3 worldPosition;
		public int gridX, gridY;

		public int gCost;
		public int hCost;
		public PathfindingNode parent;
		int heapindex;
		

		// constructor
		public PathfindingNode(bool _walkable, Vector3 _worldPos, int _gridX, int _gridY)
		{
			walkable = _walkable;
			worldPosition = _worldPos;
			gridX = _gridX;
			gridY = _gridY;
		}

		public int fCost 
		{
			get 
			{
				return gCost + hCost;	
			}
		}

		public int HeapIndex
		{
			get 
			{
				return heapindex;
			}
			set
			{
				heapindex = value;
			}
		}

		public int CompareTo(PathfindingNode nodeToCompare)
		{
			int compare = fCost.CompareTo(nodeToCompare.fCost);
			if (compare == 0)
			{
				compare = hCost.CompareTo(nodeToCompare.hCost);
			}
			return -compare;
		}


	}
}

