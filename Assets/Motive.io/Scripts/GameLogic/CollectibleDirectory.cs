using UnityEngine;
using System.Collections;
using Motive.Core.Utilities;
using System.Collections.Generic;
using Motive.Core.Models;
using System.Linq;

public class CollectibleSet
{
    public Collectible Collectible { get; set; }
    public int Count { get; set; }
}

public class CollectibleDirectory : Singleton<CollectibleDirectory>
{
    public Dictionary<string, Collectible> m_allCollectibles;

    public CollectibleDirectory()
    {
        m_allCollectibles = new Dictionary<string, Collectible>();
    }

    public void PopulateResources(Catalog<Collectible> catalog)
    {
        foreach (var c in catalog)
        {
            m_allCollectibles[c.Id] = c;
        }
    }

    public Collectible GetCollectible(string id)
    {
        if (m_allCollectibles != null && m_allCollectibles.ContainsKey(id))
        {
            return m_allCollectibles[id];
        }

        return null;
    }

    public IEnumerable<CollectibleSet> GetCollectibleSets(ValuablesCollection valuables)
    {
        if (valuables == null || valuables.CollectibleCounts == null)
        {
            return null;
        }

        List<CollectibleSet> sets = new List<CollectibleSet>();

        foreach (var cc in valuables.CollectibleCounts)
        {
            var collectible = GetCollectible(cc.CollectibleId);

            if (collectible != null)
            {
                sets.Add(new CollectibleSet { Collectible = collectible, Count = cc.Count });
            }
        }

        return sets;
    }

    internal void PopulateBonusCards(Catalog<Collectible> catalog)
    {
        foreach (var c in catalog)
        {
            m_allCollectibles[c.Id] = c;
        }

    }
}
