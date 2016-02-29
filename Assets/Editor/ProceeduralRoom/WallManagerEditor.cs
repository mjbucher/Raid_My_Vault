using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(WallManager))]
public class WallManagerEditor : Editor 
{
	string buttonNamePG = "[Disable] Perpetual Generation?";
	string buttonNameGizmo = "[Disable] Gizmos?";
	string buttonNameWall = "[Disable] Wall?";
	string buttonNameEnterance = "[Disable] Enterance?";
	public override void OnInspectorGUI ()
	{
		DrawDefaultInspector();
		WallManager myWall = (WallManager) target;

		GUILayout.Box("Quick Controls");

		if (GUILayout.Button("Reset"))
		{
			myWall.Reset();
			Debug.Log("Wall Reset");
		}

		if (GUILayout.Button(buttonNamePG))
		{
			myWall.perpetualGeneration = !myWall.perpetualGeneration;
			myWall.CheckControls();
		}
		buttonNamePG = myWall.perpetualGeneration ? "[Disable] Perpetual Generation?" : "[Enable] Perpetual Generation?";

		if (GUILayout.Button(buttonNameGizmo)) 
		{
			myWall.drawGizmos = !myWall.drawGizmos;
			myWall.CheckControls();
		}
		buttonNameGizmo = myWall.drawGizmos ? "[Disable] Gizmos?" : "[Enable] Gizmos?";

		if (GUILayout.Button(buttonNameWall))
		{
			myWall.wallEnabled = !myWall.wallEnabled;
			myWall.CheckControls();
		}
		buttonNameWall = myWall.wallEnabled ? "[Disable] Wall?" : "[Enable] Wall?";



		if (GUILayout.Button(buttonNameEnterance))
		{
			myWall.enteranceEnabled = !myWall.enteranceEnabled;
			myWall.CheckControls();
		}
		buttonNameEnterance = myWall.enteranceEnabled ? "[Disable] Enterance?" : "[Enable] Enterance?";
	}

}
