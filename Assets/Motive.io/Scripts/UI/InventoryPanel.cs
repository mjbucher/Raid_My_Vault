using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Linq;
using System.Collections.Generic;

public class InventoryPanel : Panel {

    public InventoryTableItem InventoryItem;
    public GameObject ItemsPanel;
    public TablePanel TablePanel;

    void Awake()
    {
        if (!TablePanel)
        {
            TablePanel = GetComponentInChildren<TablePanel>();
        }
    }

    public override void DidShow(object data)
    {
        TablePanel.Clear();

        var items = Inventory.Instance.AllItems;

        foreach (var item in items)
        {
            var obj = TablePanel.AddItem<InventoryTableItem>(InventoryItem);

            obj.Title.text = string.Format("{0}: {1}", item.Collectible.Title, item.Count);

            var imageUrl = item.Collectible.ImageUrl;

            if (imageUrl != null)
            {
                StartCoroutine(ImageLoader.LoadImage(imageUrl, obj.Image));
            }
        }

        base.DidShow(data);
    }
}
