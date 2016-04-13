using UnityEngine;
using System.Collections;

public static class ResetTransformsExtension
{
	public static void ResetTransforms (this Transform _transform)
	{
		_transform.position = Vector3.zero;
		_transform.localRotation = Quaternion.identity;
		_transform.localScale = Vector3.one;
	}

    public static void ResetTransfroms (this GameObject _gameObject)
    {
        Transform _transform = _gameObject.GetComponent<Transform>();
        _transform.position = Vector3.zero;
        _transform.localRotation = Quaternion.identity;
        _transform.localScale = Vector3.one;
    }
}
