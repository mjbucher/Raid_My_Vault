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

    public Camera orthographicCamera;
    public Camera topDownCamera;
    private Camera activeCamera;

    public bool isLive = true;
    public bool orthographicLive;

	void UpdateCamera ()
	{

        if (orthographicLive)
            activeCamera = orthographicCamera;
        else
            activeCamera = topDownCamera;
        activeCamera.orthographicSize = distanceFromPlayer;
        activeCamera.backgroundColor = backGroundColor;
        transform.position = targetPos.position;

    }

    void LateUpdate ()
    {
        if (isLive)
            UpdateCamera();
    }

    public void RotateCamera (string _direction)
    {
        string direction = _direction.ToLower();

        if (direction == "left")
            gameObject.transform.Rotate(0, 0, -45);
        else if (direction == "right")
            gameObject.transform.Rotate(0, 0, 45);
        else
            Debug.Log("invalid direction selected");

    }

}
