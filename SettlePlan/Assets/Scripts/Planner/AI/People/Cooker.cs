using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Cooker))]
public class CookerScriptEditor : Editor
{
    Cooker myTarget;

    public override void OnInspectorGUI()
    {
        myTarget = (Cooker)target;

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
        if (myTarget.gameObject.GetComponent<Cook>() == null)
            myTarget.gameObject.AddComponent<Cook>();
        if (myTarget.gameObject.GetComponent<DropBread>() == null)
            myTarget.gameObject.AddComponent<DropBread>();
        if (myTarget.gameObject.GetComponent<PickUpBlee>() == null)
            myTarget.gameObject.AddComponent<PickUpBlee>();
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
public class Cooker : People
{
    public override List<KeyValuePair<string, object>> CreateGoalState()
    {
        List<KeyValuePair<string, object>> goal = new List<KeyValuePair<string, object>>();

        goal.Add(new KeyValuePair<string, object>("cookFood", true));

        return goal;
    }

    public override void SetDefaultActions()
    {
        ActionGoap[] allActions = this.gameObject.GetComponents<ActionGoap>();

        for (int i = 0; i < allActions.Length; i++)
        {
            DestroyImmediate(allActions[i]);
        }

        if (this.gameObject.GetComponent<Cook>() == null)
            this.gameObject.AddComponent<Cook>();
        if (this.gameObject.GetComponent<DropBread>() == null)
            this.gameObject.AddComponent<DropBread>();
        if (this.gameObject.GetComponent<PickUpBlee>() == null)
            this.gameObject.AddComponent<PickUpBlee>();

    }
}
