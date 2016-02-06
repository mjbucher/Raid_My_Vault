using UnityEngine;
using System.Collections;

public class UIManager : MonoBehaviour {

    public void ShowTasks()
    {
        PanelManager.Instance.Show<TaskPanel>();
    }

    public void ShowInventory()
    {
        PanelManager.Instance.Show<InventoryPanel>();
    }

    public void ShowAccount()
    {
        PanelManager.Instance.Show<AccountPanel>();
    }
}
