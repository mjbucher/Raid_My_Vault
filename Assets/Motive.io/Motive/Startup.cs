using UnityEngine;
using System.Collections;

public class Startup : MonoBehaviour {

	// Use this for initialization
	void Start () {
        ScriptExtensions.Initialize(SystemPositionService.Instance.LocationManager);

        SystemPositionService.Instance.Initialize();

        WebServices.Instance.Initialize();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
