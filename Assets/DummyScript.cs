using UnityEngine;
using System.Collections;
using UnityEditor;

public class DummyScript : MonoBehaviour
{
    public int number = 0;
    public char letter = 'a';
    public string word = "Hiya!";
    public float subtleNumber = 0.00001f;


    [CustomEditor(typeof(DummyScript))]
    public class DummyScriptEditor : Editor
    {
        public override void OnInspectorGUI()
        {
          
        }
    }

}
