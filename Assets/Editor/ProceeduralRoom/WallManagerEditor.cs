using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(WallManager))]
[CanEditMultipleObjects]
public class WallManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        WallManager myWall = (WallManager)target;
        EditorGUILayout.LabelField(myWall.wallDirection.ToString() + " Wall", EditorStyles.boldLabel);
        DrawDefaultInspector();
    }
}
