using UnityEngine;
using System.Collections;

public static class GizmosExtensions
{
	public enum AxisDirection
	{
		PositiveX,
		PositiveY,
		PositiveZ,
		NegativeX,
		NegativeY,
		NegativeZ,
		None
	}

	public static void DrawCollider (this Gizmos _gizmo, Collider _col)
	{
		Gizmos.DrawCube(_col.bounds.center, _col.bounds.size);
	}

	/// <summary>
	/// Draws A debug Arrow
	/// </summary>
	/// <param name="_obj">Object.</param>
	/// <param name="_startPos">Start position.</param>
	/// <param name="_endPos">End position.</param>
	/// <param name="_headOffset">Head offset.</param>
	/// <param name="_axis">Axis.</param>
	public static void DrawArrow (this Gizmos _gizmo, Transform _startPos, Transform _endPos, float _headOffset)
	{
		Vector3 startPos = _startPos.position;
		Vector3 endPos = _endPos.position;
		float directionAngle = Vector3.Angle(startPos, endPos);
		Vector3 directionOffset = (endPos - startPos);// / (endPos - startPos); // end up with directional vector, though assums you are drawing it on a plane
		Vector3 topEndPos = endPos + _endPos.up * (1 + _headOffset) + directionOffset;
		Vector3 bottomEndPos = endPos - _endPos.up * (1 - _headOffset) + directionOffset;
		Debug.DrawLine(startPos, endPos);
		Debug.DrawLine(endPos, topEndPos);
		Debug.DrawLine(endPos, bottomEndPos);
	}

}


