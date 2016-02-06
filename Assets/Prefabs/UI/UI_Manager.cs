using UnityEngine;
using System.Collections;

public class UI_Manager : MonoBehaviour 
{
	public enum UIType
	{
		Main,
		Pause,
		Raiding,
		Building,
		Map,
		ScoutReport,
		AfterActionReport

	}
	[SerializeField] public UIGroup group;
	[SerializeField]public UIGroup[] HudGroups; // = new UIGroup();

	[System.Serializable]
	public class UIGroup : UI_Manager
	{
		// associations
		public GameObject HUDObject;
		public UIType UIType;
		// constructor
		public UIGroup(GameObject _HUDObject, UIType _UIType)
		{
			HUDObject = _HUDObject;
			UIType = _UIType;
		}

	}





	public void enableUI (UIGroup _hudGroup)
	{
		_hudGroup.HUDObject.SetActive(true);
	}

	public void diableUI (UIGroup _hudgroup)
	{
		_hudgroup.HUDObject.SetActive(false);
	}



}
