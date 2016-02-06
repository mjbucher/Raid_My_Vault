using UnityEngine;
using System.Collections;

public class LocationTaskItem : PlayerTaskItem<LocationTask> {

    // Use this for initialization
	void Start () {
	
	}

    public void ShowLocation()
    {
        MapController.Instance.SelectTaskAnnotation(Driver.Task);
        PanelManager.Instance.HideAll();
    }
}
