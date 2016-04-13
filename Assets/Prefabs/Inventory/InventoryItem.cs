using UnityEngine;
using System.Collections;
using RMV;

/// <summary>
/// and item that can be displayed by inventory gui
/// to be added to part of the inventory database
/// </summary>
namespace RMV
{
    [System.Serializable]
    public class InventoryItem
    {
        public string itemName;
        public int itemID;
        public string itemDescription;
        public Texture2D itemIcon;
        public GameObject itemPrefab;
        public float itemPrice;
        public int numberAvailable;
        public InventoryCategory category;// = InventoryCategory.None;
 

        public InventoryItem (string _itemName, int _itemID, string _itemDescription, GameObject _itemPrefab, InventoryCategory _category, float _itemPrice)
        {
            itemName = _itemName;
            itemID = _itemID;
            itemDescription = _itemDescription;
            string filename = _itemName.Replace(" ", "") + "Icon";
            Debug.Log(filename);
            itemIcon = Resources.Load<Texture2D>("ItemIcons/" + filename) ? Resources.Load<Texture2D>("ItemIcons/" + filename): Resources.Load<Texture2D>("ItemIcons/" + "DefaultIcon");
            itemPrefab =_itemPrefab;
            category = _category;
            itemPrice = _itemPrice;


        }

        public InventoryItem(string _itemName, int _itemID, string _itemDescription, WeaponType _weaponType, float _itemPrice)
        {
            itemName = _itemName;
            itemID = _itemID;
            itemDescription = _itemDescription;
            itemIcon = (bool)Resources.Load<Texture2D>("ItemIcons/" + itemName) ? Resources.Load<Texture2D>("ItemIcons/" + itemName) : Resources.Load<Texture2D>("ItemIcons/" + "DefaultIcon");
            // itemPrefab = _itemPrefab;
            category = InventoryCategory.Weapon;
            itemPrice = _itemPrice;


        }

        public void UseItem()
        {

        }

        public void SubtractItemCount ()
        {
            numberAvailable--;
        }

        public void AddItemCount()
        {
            numberAvailable++;
        }


    }
}


