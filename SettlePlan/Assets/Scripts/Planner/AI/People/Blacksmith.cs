using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Blacksmith))]
public class BlacksmithScriptEditor : Editor
{
    Blacksmith myTarget;

    public override void OnInspectorGUI()
    {
        myTarget = (Blacksmith)target;

        base.DrawDefaultInspector();

        EditorGUI.indentLevel = 0;
        EditorGUILayout.LabelField("Fast actions:");
        EditorGUI.indentLevel = 1;

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Add default actions"))
        {
            AddAction();
        }
        else if (GUILayout.Button("Remove all actions"))
        {
            RemoveAction();
        }
        EditorGUILayout.EndHorizontal();

        EditorGUI.indentLevel = 0;
    }
    void AddAction()
    {
        if (myTarget.gameObject.GetComponent<CutWood>() == null)
            myTarget.gameObject.AddComponent<CutWood>();
        if (myTarget.gameObject.GetComponent<DropLog>() == null)
            myTarget.gameObject.AddComponent<DropLog>();
    }
    void RemoveAction()
    {
        ActionGoap[] allActions = myTarget.GetComponents<ActionGoap>();

        for (int i = 0; i < allActions.Length; i++)
        {
            DestroyImmediate(allActions[i]);            
        }
    }
}
public class Blacksmith : People
{
    public override List<KeyValuePair<string, object>> CreateGoalState()
    {
        List<KeyValuePair<string, object>> goal = new List<KeyValuePair<string, object>>();

        goal.Add(new KeyValuePair<string, object>("createTools", true));

        return goal;
    }

    public override void SetDefaultActions()
    {
        ActionGoap[] allActions = this.gameObject.GetComponents<ActionGoap>();

        for (int i = 0; i < allActions.Length; i++)
        {
            DestroyImmediate(allActions[i]);
        }

        if (this.gameObject.gameObject.GetComponent<CutWood>() == null)
            this.gameObject.gameObject.AddComponent<CutWood>();
        if (this.gameObject.gameObject.GetComponent<DropLog>() == null)
            this.gameObject.gameObject.AddComponent<DropLog>();

    }
}
