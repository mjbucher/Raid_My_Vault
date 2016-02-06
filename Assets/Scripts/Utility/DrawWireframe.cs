using UnityEngine;
using System.Collections;

namespace Utility
{
	public class DrawWireframe : MonoBehaviour 
	{	
		public Color drawColor = Color.cyan;


		private Collider _col;
		private Transform _childTransform; 

		private Vector3 _center;
		private Vector3 _size;

		public void OnDrawGizmos ()
		{
			// if it has a collider use that for center and size
			if (GetComponent<Collider>())
			{
				_col = GetComponent<Collider>();
				_center = _col.bounds.center;
				_size = _col.bounds.size;

			}
			// else mind what ever child has a mesh and copy from there
			else
			{
				_childTransform = GetComponentInChildren<MeshFilter>().gameObject.GetComponent<Transform>();
				_center = _childTransform.position;
				_size = _childTransform.lossyScale;
			}
			// draw cube in the color from inspector
			Gizmos.color = drawColor;
			Gizmos.DrawWireCube(_center, _size);
		}
	}
}
