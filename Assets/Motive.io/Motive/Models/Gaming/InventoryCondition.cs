using UnityEngine;
using System.Collections;
using Motive.Core.Scripting;

public class InventoryCondition : AtomicCondition {
    public NumericalConditionOperator Operator { get; set; }

    public CollectibleCount CollectibleCount { get; set; }
}
