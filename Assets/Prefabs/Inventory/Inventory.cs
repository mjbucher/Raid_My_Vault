using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RMV;

namespace RMV
{
    [ExecuteInEditMode]
    public class Inventory : MonoBehaviour
    {
        public List<InventoryItem> inventory = new List<InventoryItem>();
        private InventoryDatabase database;

        void Awake ()
        {
           database = GetComponent<InventoryDatabase>();
           //inventory.Add(database.rifleWeapons[0]);
           //inventory.Add(database.rifleWeapons[1]);
           //inventory.Add(new InventoryItem("test", 0, "testing", new GameObject(), InventoryCategory.Weapon, 0.0f));
        }

        public bool CheckInventoryFor (InventoryItem _item)
        {
            bool tempBool;
            tempBool = inventory.Contains(_item) ? true : false;
            return tempBool;
        }

        public void AddToInventory (InventoryItem _item)
        {
            inventory.Add(_item);
        }

        public void SubractFromInventory (InventoryItem _item)
        {
            inventory.Remove(_item);
        }

        void OnGUI()
        {
            for (int i = 0; i < inventory.Count; i++)
            {
                GUI.Label(new Rect(10, i * 20, 200, 50), inventory[i].itemName);
            }
        }


    }
}


