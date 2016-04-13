using UnityEngine;
using System.Collections;

public class TimeOut : MonoBehaviour
{
    public GameObject text;
    public float timeoutTimer;
	
   
    // Use this for initialization
	void Start ()
    {
        StartCoroutine("WaitForTimeout");
    }

    IEnumerator WaitForTimeout()
    {
        yield return new WaitForSeconds(timeoutTimer);
        text.SetActive(true);
    }

}
