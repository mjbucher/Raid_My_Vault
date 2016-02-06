using UnityEngine;
using System.Collections;

public class TablePanel : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Clear()
    {
        transform.DetachChildren();
    }

    public T AddItem<T>(T prefab) where T : Component
    {
        var obj = Instantiate<T>(prefab);

        obj.transform.SetParent(transform, false);

        return obj;
    }
}
