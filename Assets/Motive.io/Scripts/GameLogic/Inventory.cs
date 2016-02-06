using UnityEngine;
using System.Collections;
using Motive.Core.Utilities;
using System.Collections.Generic;
using System;
using System.Linq;
using Motive.Core.Scripting;
using Motive.Core.Json;

public class InventoryItemState
{
    public string CollectibleId { get; set; }
    public int Count { get; set; }
}

public class InventoryItem
{
    public Collectible Collectible { get; set; }
    public int Count { get; set; }
}

public class Inventory : Singleton<Inventory> {

    private Dictionary<string, int> m_inventoryItems;

    public event EventHandler Updated;

    private string m_stateFile;

    public IEnumerable<InventoryItem> AllItems
    {
        get
        {
            List<InventoryItem> items = new List<InventoryItem>();

            foreach (var kv in m_inventoryItems)
            {
                var collectible = CollectibleDirectory.Instance.GetCollectible(kv.Key);

                if (collectible != null)
                {
                    items.Add(new InventoryItem { Collectible = collectible, Count = kv.Value });
                }
            }

            return items.OrderBy(i => i.Collectible.InventoryOrder);
        }
    }

    public Inventory()
    {
        m_stateFile = StorageManager.GetGameFileName("inventory.json");

        m_inventoryItems = new Dictionary<string, int>();

        var state = JsonHelper.Deserialize<InventoryItemState[]>(m_stateFile);

        if (state != null)
        {
            foreach (var cc in state)
            {
                m_inventoryItems[cc.CollectibleId] = cc.Count;
            }
        }

        ScriptManager.Instance.ScriptsReset += Instance_ScriptsReset;
    }

    void Instance_ScriptsReset(object sender, EventArgs e)
    {
        m_inventoryItems.Clear();

        if (Updated != null)
        {
            Updated(this, EventArgs.Empty);
        }
    }

    public void Save()
    {
        var iis = m_inventoryItems.Select(kv => new InventoryItemState { CollectibleId = kv.Key, Count = kv.Value });

        JsonHelper.Serialize(m_stateFile, iis.ToArray());
    }

    public int GetCount(string collectibleId)
    {
        if (m_inventoryItems.ContainsKey(collectibleId))
        {
            return m_inventoryItems[collectibleId];
        }

        return 0;
    }

    public void Remove(IEnumerable<CollectibleCount> collectibleCounts)
    {
        if (collectibleCounts != null)
        {
            foreach (var cc in collectibleCounts)
            {
                Remove(cc, false);
            }
        }

        Save();
    }

    void Remove(CollectibleCount collectibleCount, bool commit)
    {
        if (m_inventoryItems.ContainsKey(collectibleCount.CollectibleId))
        {
            var curr = m_inventoryItems[collectibleCount.CollectibleId];

            var toTake = Math.Min(curr, collectibleCount.Count);

            SetCount(
                collectibleCount.CollectibleId, 
                m_inventoryItems[collectibleCount.CollectibleId] - toTake,
                commit);
        }
    }

    public void Remove(CollectibleCount collectibleCount)
    {
        Remove(collectibleCount, true);
    }

    private void Add(CollectibleCount collectibleCount, bool commit)
    {
        if (!m_inventoryItems.ContainsKey(collectibleCount.CollectibleId))
        {
            SetCount(collectibleCount.CollectibleId, collectibleCount.Count, commit);
        }
        else
        {
            SetCount(
                collectibleCount.CollectibleId, 
                m_inventoryItems[collectibleCount.CollectibleId] +  collectibleCount.Count, 
                commit);
        }
    }

    private void Set(CollectibleCount collectibleCount, bool commit)
    {
        SetCount(collectibleCount.CollectibleId, collectibleCount.Count, commit);
    }

    private void SetCount(string collectibleId, int count, bool commit)
    {
        if (count == 0)
        {
            m_inventoryItems.Remove(collectibleId);
        }
        else
        {
            m_inventoryItems[collectibleId] = count;
        }

        if (Updated != null)
        {
            Updated(this, EventArgs.Empty);
        }

        if (commit)
        {
            Save();
        }
    }

    private void Set(CollectibleCount collectibleCount)
    {
        Set(collectibleCount, true);
    }

    private void Add(CollectibleCount collectibleCount)
    {
        Add(collectibleCount, true);
    }

    public void Add(IEnumerable<CollectibleCount> collectibleCounts)
    {
        if (collectibleCounts != null)
        {
            foreach (var cc in collectibleCounts)
            {
                Add(cc);
            }
        }
    }

    public void Set(IEnumerable<CollectibleCount> collectibleCounts)
    {
        if (collectibleCounts != null)
        {
            foreach (var cc in collectibleCounts)
            {
                Set(cc);
            }
        }
    }
    public int TotalItems
    {
        get
        {
            return m_inventoryItems.Values.Sum();
        }
    }
}
