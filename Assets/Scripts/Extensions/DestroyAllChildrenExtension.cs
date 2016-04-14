using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class DestroyAllChildrenExtension 
{
    #region Destroy All Children Now
    public static void DestroyAllChildrenNow (this GameObject _parent)
    {
        //GameObject _parent = _manager.geometrySpawner;
        Transform[] childrenT = _parent.GetComponentsInChildren<Transform>();
        List<GameObject> childrenG = new List<GameObject>();
        foreach (Transform child in childrenT)
        {
            childrenG.Add(child.gameObject);
        }
        // remove parent of list (manager)
        childrenG.Remove(_parent);
        foreach (GameObject child in childrenG)
        {
           GameObject.DestroyImmediate(child);
        }
    }

    public static void DestroyAllChildrenNow(this GameObject _me, GameObject _parent)
    {
        //GameObject _parent = _manager.geometrySpawner;
        Transform[] childrenT = _parent.GetComponentsInChildren<Transform>();
        List<GameObject> childrenG = new List<GameObject>();
        foreach (Transform child in childrenT)
        {
            childrenG.Add(child.gameObject);
        }
        // remove parent of list (manager)
        childrenG.Remove(_parent);
        foreach (GameObject child in childrenG)
        {
            GameObject.DestroyImmediate(child);
        }
    }
    #endregion
}
