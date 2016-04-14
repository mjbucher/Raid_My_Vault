using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class CameraController : MonoBehaviour 
{
	public float distanceFromPlayer = 5.0f;
	public float nearPlane = 0.3f;
	public float farPlane = 1000.0f;
	public float downwardAngle;

    public Transform targetPos;
	public Color backGroundColor = Color.black;

    public bool isLive = true;

	void UpdateCamera ()
	{
		Camera.main.orthographicSize = distanceFromPlayer;
		Camera.main.backgroundColor = backGroundColor;
        transform.position = targetPos.position;
	}

    void LateUpdate ()
    {
        if (isLive)
            UpdateCamera();
    }

}
