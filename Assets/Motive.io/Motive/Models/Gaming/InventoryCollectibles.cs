using UnityEngine;
using System.Collections;
using Motive.Core.Scripting;

public class InventoryCollectibles : ScriptObject {

    public CollectibleCount[] CollectibleCounts { get; set; }
    public bool StartAtZero { get; set; }
}
