using UnityEngine;
using System.Collections;

public class LocationTreasureChest 
{
    public string[] LocationTypes { get; set; }
    public string[] StoryTags { get; set; }
    public double Weight { get; set; }
    public WeightedValuablesCollection[] TreasureChests { get; set; }
}
