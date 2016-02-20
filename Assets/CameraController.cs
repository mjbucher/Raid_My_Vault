using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour 
{
	public float distanceFromPlayer = 5.0f;
	public float nearPlane = 0.3f;
	public float farPlane = 1000.0f;
	public float downwardAngle;

	public Color backGroundColor = Color.black;


	void UpdateCamera ()
	{
		Camera.main.orthographicSize = distanceFromPlayer;
		Camera.main.backgroundColor = backGroundColor;
	}


}
