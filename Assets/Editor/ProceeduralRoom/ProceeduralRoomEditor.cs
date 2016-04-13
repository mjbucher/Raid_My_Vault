using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(RoomProceedural))]
public class ProceeduralRoomEditor : Editor
{
    Editor _editor;

    public override void OnInspectorGUI()
    {
        // defaults
        DrawDefaultInspector();
        RoomProceedural myRoom = (RoomProceedural)target;

        // serialize editors for walls
        GUILayout.Space(10.0f);
        CreateCachedEditor(myRoom.northWallManager, null, ref _editor);
        _editor.OnInspectorGUI();

        GUILayout.Space(10.0f);
        CreateCachedEditor(myRoom.eastWallManager, null, ref _editor);
        _editor.OnInspectorGUI();

        GUILayout.Space(10.0f);
        CreateCachedEditor(myRoom.southWallManger, null, ref _editor);
        _editor.OnInspectorGUI();

        GUILayout.Space(10.0f);
        CreateCachedEditor(myRoom.westWallManager, null, ref _editor);
        _editor.OnInspectorGUI();


    }

}
