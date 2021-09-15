using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MainController))]
[CanEditMultipleObjects]
public class MainControllerEditor : Editor
{
    //SerializedProperty mainController;

    //public Object source;
    //public ScriptController scriptController;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        /*
        EditorGUILayout.BeginVertical();
        source = EditorGUILayout.ObjectField(source, typeof(Object), true);
        scriptController = (ScriptController) EditorGUILayout.ObjectField(scriptController, typeof(ScriptController), true);
        EditorGUILayout.EndVertical();
        */
    }

}
