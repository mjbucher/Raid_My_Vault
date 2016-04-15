using UnityEngine;
using System.Collections;
using System;

public static class GameObjectExtensions
{
   public static void CheckAfterTime (this GameObject _object, bool _switch, Action _action)
    {
        
    }

    public static void InstantiateAsChild (this GameObject _object, GameObject _objectToSpawn, GameObject _parentObject)
    {
        GameObject childObject = GameObject.Instantiate(_objectToSpawn) as GameObject;
        childObject.transform.SetParent(_parentObject.transform);


    }
    public static void InstantiateAsChild(this GameObject _object, GameObject _objectToSpawn)
    {
        GameObject childObject = GameObject.Instantiate(_objectToSpawn) as GameObject;
        childObject.transform.SetParent(_object.transform);
    }

    //public static GameObject InstantiateAsChild(this GameObject _object, GameObject _objectToSpawn, GameObject _parentObject)
    //{
    //    GameObject childObject = GameObject.Instantiate(_objectToSpawn) as GameObject;
    //    childObject.transform.SetParent(_parentObject.transform);
    //    return childObject;

    //}
    //public static GameObject InstantiateAsChild(this GameObject _object, GameObject _objectToSpawn)
    //{
    //    GameObject childObject = GameObject.Instantiate(_objectToSpawn) as GameObject;
    //    childObject.transform.SetParent(_object.transform);
    //    return childObject;
    //}
}
