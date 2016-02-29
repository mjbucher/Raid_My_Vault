using UnityEngine;
using System.Collections;


namespace Utility
{
	public class DrawWireframe : MonoBehaviour 
	{	
		public Color drawColor = Color.cyan;
		Collider[] _cols;
		Vector3 _center;
		Vector3 _size;

		void OnDrawGizmos ()
		{
			if (GetComponents<Collider>().Length > 0)
			{
				_cols = GetComponents<Collider>();
				foreach (Collider _col in _cols)
				{
					_center = _col.bounds.center;
					_size = _col.bounds.size;
					Gizmos.color = drawColor;
					Gizmos.DrawWireCube(_center, _size);
				}
			}
		}
	}
}
