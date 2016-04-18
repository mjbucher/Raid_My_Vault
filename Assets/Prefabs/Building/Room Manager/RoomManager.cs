using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using RMV;

public class RoomManager : MonoBehaviour 
{
	[Header("AI")]
	public bool enableAI = true;
	public int maxAI = 2;
	bool maxAIReached = false;
	[Header("Traps")]
	// maybe use state machine instead?
	public bool trapsEnabled = true;
	public int maxTraps = 4;
	public List<GameObject> totalTraps;
	List<GameObject> activeTraps;
	List<GameObject> usedTraps;
	List<GameObject> diabledTraps;
	[Header("Item Spawn points")]
	public List<GameObject> spawners;

    public List<GameObject> keySpawns;
    public List<GameObject> powerCellSpawn;
    public bool showPaths;
    public List<Path> AIPaths;

    #region Editors
    //[CustomEditor(typeof(RoomManager))]
    //public class RoomManagerEditor : Editor
    //{
    //    Editor _editor;

    //    public override void OnInspectorGUI()
    //    {
    //        RoomManager manager = (RoomManager)target;
    //        DrawDefaultInspector();
    //        if (manager.showPaths)
    //        {
    //            int counter = 0;
    //            foreach (Path _path in manager.AIPaths)
    //            {
    //                EditorGUILayout.Separator();
    //                CreateCachedEditor(_path, null, ref _editor);
    //                EditorGUILayout.LabelField(_path.gameObject.name + ": #" + counter, EditorStyles.boldLabel);
    //                _editor.OnInspectorGUI();
    //                counter++;
    //            }
    //        }
    //    }
    //}
    //[CustomEditor(typeof(Path))]
    //public class PathEditor : Editor
    //{
    //    public override void OnInspectorGUI()
    //    {
    //        Path path = (Path)target;
    //        DrawDefaultInspector();
    //    }
    //}
    #endregion
}
