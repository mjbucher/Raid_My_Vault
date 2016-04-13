using UnityEngine;
using System.Collections;


public class UILogicManager : MonoBehaviour
{
    
    public UIGroup group;

    public UIGroup[] HudGroups; // = new UIGroup();

    public class UIGroup : UIManager
    {
        // associations
        public GameObject HUDObject;
        public UIState state;
        //constructor
        public UIGroup(GameObject _HUDObject, UIState _state)
        {
        HUDObject = _HUDObject;
        state = _state;
        }
    }

    public void enableUI(UIGroup _hudGroup)
    {
        _hudGroup.HUDObject.SetActive(true);
    }

    public void diableUI(UIGroup _hudgroup)
    {
        _hudgroup.HUDObject.SetActive(false);
    }



 }


