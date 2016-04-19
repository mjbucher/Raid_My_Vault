using UnityEngine;
using System.Collections;

public class AccelatePlayer : MonoBehaviour
{

    public DebugPlayerLocation dpl;
    public float speedMultiplier = 5.0f;
    private float speed;
    private bool goingFast = false;


    void Start ()
    {
        speed = dpl.PlayerSpeed;
    }

    void Update ()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            goingFast = !goingFast;
            if (goingFast)
                dpl.PlayerSpeed = speed * speedMultiplier;
            else
                dpl.PlayerSpeed = speed;

        }
    }
}
