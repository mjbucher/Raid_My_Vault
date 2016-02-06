using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Linq;
using Motive.Core.WebServices;

public class MapHud : MonoBehaviour {

    public Button TasksButton;
    public Button InventoryButton;

	// Use this for initialization
	void Start () {
	    TaskManager.Instance.Updated += TaskManagerUpdated;
        Inventory.Instance.Updated += InventoryUpdated;

        UpdateTaskCount();
        UpdateInventoryCount();
	}

    private void InventoryUpdated(object sender, System.EventArgs e)
    {
        UpdateInventoryCount();
    }

    void UpdateInventoryCount()
    {
        InventoryButton.GetComponentInChildren<Text>().text = string.Format("Inventory ({0})",
            Inventory.Instance.TotalItems);
    }

    void UpdateTaskCount()
    {
        TasksButton.GetComponentInChildren<Text>().text = string.Format("Tasks ({0})",
            TaskManager.Instance.ActiveTaskDrivers.Count());
    }

    private void TaskManagerUpdated(object sender, System.EventArgs e)
    {
        UpdateTaskCount();
    }
	
	// Update is called once per frame
	void Update () {
	}
}
