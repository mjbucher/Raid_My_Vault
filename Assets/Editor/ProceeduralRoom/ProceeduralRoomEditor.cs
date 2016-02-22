using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(RoomProceedural))]
public class ProceeduralRoomEditor : Editor 
{
	string name = "Turn ON Perpetual Generation: [Currently OFF]";

	public override void OnInspectorGUI ()
	{
	
		DrawDefaultInspector();
		RoomProceedural myRoom = (RoomProceedural) target;

		//GUILayout.Toolbar()
		GUILayout.Box("Quick Controls");

		if (GUILayout.Button("Reload"))
		{
			myRoom.Reload();
			Debug.Log("Room Resotred!");
	
		}
		if (GUILayout.Button("Reset"))
		{
			myRoom.ClearAllManagers();
			name = "Turn OFF Perpetual Generation: [Currently ON]";
		}

		if (GUILayout.Button(name))
		{
			myRoom.PerpetualGeneration = !myRoom.PerpetualGeneration;
			name = name == "Turn OFF Perpetual Generation: [Currently ON]" ? "Turn ON Perpetual Generation: [Currently OFF]" : "Turn OFF Perpetual Generation: [Currently ON]";
		}



	}


}
