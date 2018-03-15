using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Farmer))]
public class FarmerScriptEditor : Editor
{
    Farmer myTarget;

    public override void OnInspectorGUI()
    {
        myTarget = (Farmer)target;

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
        if (myTarget.gameObject.GetComponent<HarvestBlee>() == null)
            myTarget.gameObject.AddComponent<HarvestBlee>();
        if (myTarget.gameObject.GetComponent<DropBlee>() == null)
            myTarget.gameObject.AddComponent<DropBlee>();
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
public class Farmer : People
{
    public override List<KeyValuePair<string, object>> CreateGoalState()
    {
        List<KeyValuePair<string, object>> goal = new List<KeyValuePair<string, object>>();

        goal.Add(new KeyValuePair<string, object>("collectBlee", true));

        return goal;
    }

    public override void SetDefaultActions()
    {
        ActionGoap[] allActions = this.gameObject.GetComponents<ActionGoap>();

        for (int i = 0; i < allActions.Length; i++)
        {
            DestroyImmediate(allActions[i]);
        }

        if (this.gameObject.gameObject.GetComponent<DropBlee>() == null)
            this.gameObject.gameObject.AddComponent<DropBlee>();
        if (this.gameObject.gameObject.GetComponent<HarvestBlee>() == null)
            this.gameObject.gameObject.AddComponent<HarvestBlee>();

    }
}
