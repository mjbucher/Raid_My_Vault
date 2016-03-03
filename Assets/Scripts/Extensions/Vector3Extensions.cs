using UnityEngine;
using System.Collections;

public static class Vector3Extensions 
{
	public static Transform AsTransform(this Vector3 _pos)
	{
		GameObject dummy = new GameObject();
		Transform dummyT = dummy.GetComponent<Transform>();
		dummyT.rotation = Quaternion.identity;
		dummyT.localScale = Vector3.one;
		dummyT.position = _pos;
		GameObject.DestroyImmediate(dummy);
		return dummyT;
	}
}