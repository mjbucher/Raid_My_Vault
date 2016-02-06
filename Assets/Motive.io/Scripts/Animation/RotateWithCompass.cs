using UnityEngine;
using System.Collections;
using Motive.Core.Utilities;

public class RotateWithCompass : MonoBehaviour {

    public Vector3 Axis = new Vector3(0, 0, 1);
    public float Offset = 0f;
    public bool UseMagneticHeading = false;

    private float m_editorHeading = 0f;

	// Use this for initialization
	void Start () {
        Input.compass.enabled = true;
        gameObject.transform.rotation = Quaternion.AngleAxis(0f, Axis);
	}

    float Heading
    {
        get
        {
            if (Application.isEditor)
            {
                return m_editorHeading;
            }
            else
            {
                if (UseMagneticHeading)
                {
                    return Input.compass.magneticHeading;
                }
                else
                {
                    return Input.compass.trueHeading;
                }
            }
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (Application.isEditor)
        {
            if (Input.GetKey(KeyCode.R))
            {
                m_editorHeading += Time.deltaTime * 360f;
            }

            if (Input.GetKey(KeyCode.L))
            {
                m_editorHeading -= Time.deltaTime * 360f;
            }

            m_editorHeading = MathHelper.GetDegreesInRange(m_editorHeading);
        }

        float currAngle;
        Vector3 currAxis;

        gameObject.transform.rotation.ToAngleAxis(out currAngle, out currAxis);

        float targetAngle = MathHelper.GetDegreesInRange(-(Heading + Offset));

        targetAngle = MathHelper.ApproachAngle(currAngle, targetAngle, 0.1f, Time.deltaTime);

        this.gameObject.transform.rotation = Quaternion.AngleAxis(targetAngle, Axis);
    }
}
