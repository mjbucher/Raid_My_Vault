using UnityEngine;
using System.Collections;

public class DisableOnStart : MonoBehaviour
{
    public Component[] components;

    void Start ()
    {
        foreach (Component _component in components)
        {
            if (_component is Behaviour)
                (_component as Behaviour).enabled = false;
            else if (_component is Collider)
                (_component as Collider).enabled = false;
            else if (_component is Renderer)
                (_component as Renderer).enabled = false;
        }
    }
}
