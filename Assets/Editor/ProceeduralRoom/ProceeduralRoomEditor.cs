using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(RoomProceedural))]
public class ProceeduralRoomEditor : Editor 
{
	string buttonName = "Turn [ON] Perpetual Generation: [Mode: OFF]";

	public override void OnInspectorGUI ()
	{

		DrawDefaultInspector();
		RoomProceedural myRoom = (RoomProceedural) target;

		//GUILayout.Toolbar()
		GUILayout.Box("Quick Controls");

		if (GUILayout.Button("Reload"))
		{
			Debug.Log("Room Resotred!");
			myRoom.Reload();
			Debug.Log("Room Resotred!");
	
		}

		if (GUILayout.Button("Reset"))
		{
			myRoom.ClearAllManagers();
			Debug.Log("Room Reset");
		}
			
		if (GUILayout.Button(buttonName))
		{
			myRoom.PerpetualGeneration = !myRoom.PerpetualGeneration;
		}

		buttonName = myRoom.PerpetualGeneration ? "Turn [OFF] Perpetual Generation: [Mode: ON]" : "Turn [ON] Perpetual Generation: [Mode: OFF]";

	}


}
