using UnityEngine;
using System.Collections;
using System;

public class Panel<T> : Panel
{
    public T Data { get; private set; }
    public virtual void DidShow(T data)
    {

    }

    public override void DidShow(object data)
    {
        if (data is T)
        {
            Data = (T)data;
            DidShow((T)data);
        }
    } 
}

public class Panel : MonoBehaviour {
    public bool PrePositioned;
    public bool WaitForUserHide;

    public Action OnClose { get; set; }
    public virtual void DidShow(object data)
    {

    }

    public virtual void DidHide()
    {

    }

    public virtual void Back()
    {
        PanelManager.Instance.Hide(this);
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
