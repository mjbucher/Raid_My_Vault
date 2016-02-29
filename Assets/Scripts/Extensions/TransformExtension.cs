using UnityEngine;
using System.Collections;

public static class TransformExtension {

	public static void ResetTransforms (this Transform _transform)
	{
		_transform.position = Vector3.zero;
		_transform.localRotation = Quaternion.identity;
		_transform.localScale = Vector3.one;
	}
}
