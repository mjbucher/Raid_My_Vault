using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RMV;

/// <summary>
/// datebases for all items and their corresponding lists
/// </summary>

namespace RMV
{
    [ExecuteInEditMode]
    public class InventoryDatabase : MonoBehaviour
    {
        public bool generateItemDatabase;
        // Prefab for a generic button
       // public GameObject buttonPrefab;
        // Lists of all assets that can be built (these may be different tabs)
        public List<InventoryItem> weapons;
        public List<InventoryItem> rooms;
        public List<InventoryItem> traps;
        public List<InventoryItem> AI;
        public List<InventoryItem> upgrades;
        //List<GameObject> allItems;

        // smaller lists for weapons
        //public List<InventoryItem> weapons;
        //public List<InventoryItem> rifleWeapons;
        //public List<InventoryItem> bazookaWeapons;
        //public List<InventoryItem> grenadeWeapons;
        //public List<InventoryItem> trapWeapons;

        // smaller lists for mods
        // public List<InventoryItem> statMods;
        // public List<InventoryItem> abilityMods;
        // public List<InventoryItem> passiveMods;

        // smalle lists for room sizes
        //public List<InventoryItem> smallRooms;
        //public List<InventoryItem> mediumRooms;
        //public List<InventoryItem> largeRooms;

        // smaller list for upgrades
        //public List<InventoryItem> upgradeRooms;

        // smaller list for traps
        //public List<InventoryItem> oneUseTraps;
        //public List<InventoryItem> continuousTraps;
        //public List<InventoryItem> hiddenTraps;

        // combination lists
        // public List<List<InventoryItem>> allWeapons;
        // public List<List<InventoryItem>> allRooms; 

        public int numberOfItems;
        int[,] buttonPositions;

        void Update ()
        {
            CheckControls();
        }

        void CheckControls ()
        {
            if (generateItemDatabase)
            {
                SetUpInventoryDatabase();
                Debug.Log("Database Generated");
                generateItemDatabase = false;
            }
        }

        void SetUpInventoryDatabase ()
        {
            #region Set Up Weapons
            // set up weapons
            weapons.Clear();
            // add rifles
            weapons.Add(new InventoryItem("Basic Rifle", 0, "A basic rifle.", WeaponType.Projectile, 0));
            weapons.Add(new InventoryItem("Goo Rifle", 1, "A goo rifle.", WeaponType.Projectile, 0));
            weapons.Add(new InventoryItem("Confusion Rifle", 2, "A consfusion rifle.", WeaponType.Projectile, 0));
            weapons.Add(new InventoryItem("EMP Rifle", 3, "A emp rifle.", WeaponType.Projectile, 0));
            weapons.Add(new InventoryItem("Slow Rifle", 4, "A slow rifle.", WeaponType.Projectile, 0));
            // add bazookas

            // add grenades

            // add throwable traps

            // add player mods

            #endregion

            #region Set Up Rooms
            // set up rooms
            rooms.Clear();
            // set up small rooms

            // set up medium rooms

            // set up large rooms

            #endregion

            #region Set Up Upgrades
            // set up upgrades
            upgrades.Clear();
            // set up Room upgrades

            // set up AI upgrades

            // set up Trap upgrades

            // set up Other upgrades

            #endregion

            #region Set Up AI
            // set up AI
            AI.Clear();
            // set up basic AI

            #endregion

            #region Set Up Traps
            // set up traps
            traps.Clear();
            //set up Use Activated traps

            // set up Continuous traps

            // set up obstacles and barriers

            #endregion
        }

    }

}
