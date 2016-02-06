using UnityEngine;
using System.Collections;
using Motive.Core.Utilities;
using Motive.Core.Models;
using System.Collections.Generic;
using System.Linq;

public class CharacterDirectory : Singleton<CharacterDirectory> {
    Dictionary<string, Character> m_directory;
    public void Populate(Catalog<Character> catalog)
    {
        m_directory = catalog.ToDictionary(c => c.Id);
    }

    public Character GetCharacter(string id)
    { 
        if (id != null && m_directory.ContainsKey(id))
        {
            return m_directory[id];
        }

        return null;
    }
}
