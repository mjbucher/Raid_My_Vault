using UnityEngine;
using System.Collections;
using Motive.Unity.Scripting;

public class InventoryCollectiblesProcessor : ThreadSafeScriptResourceProcessor<InventoryCollectibles> {
    public override void ActivateResource(Motive.Core.Scripting.ResourceActivationContext context, InventoryCollectibles resource)
    {
        if (!context.IsClosed)
        {
            // Add items to player's inventory
            if (resource.StartAtZero)
            {
                Inventory.Instance.Set(resource.CollectibleCounts);
            }
            else
            {
                Inventory.Instance.Add(resource.CollectibleCounts);
            }

            context.Close();
        }
    }
}
