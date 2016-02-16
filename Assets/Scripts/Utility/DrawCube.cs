using UnityEngine;
using System.Collections;

namespace Utility
{
	public class DrawCube : MonoBehaviour 
	{
		public Color _color = Color.magenta;

		public float cubeSize = 0.2f; 

		public void OnDrawGizmos ()
		{
			Gizmos.color = _color;
			Gizmos.DrawCube(transform.position, Vector3.one * cubeSize);
			Gizmos.color = Color.black;
			Gizmos.DrawSphere (transform.position, cubeSize / 4);
		}
	}
}

