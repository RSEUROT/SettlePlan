using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Eater))]
public class EaterScriptEditor : Editor
{
    Eater myTarget;

    public override void OnInspectorGUI()
    {
        myTarget = (Eater)target;

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
        if (myTarget.gameObject.GetComponent<Eat>() == null)
            myTarget.gameObject.AddComponent<Eat>();
        if (myTarget.gameObject.GetComponent<PickUpBread>() == null)
            myTarget.gameObject.AddComponent<PickUpBread>();
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
public class Eater : People
{
    public override List<KeyValuePair<string, object>> CreateGoalState()
    {
        List<KeyValuePair<string, object>> goal = new List<KeyValuePair<string, object>>();

        goal.Add(new KeyValuePair<string, object>("needToEat", true));

        return goal;
    }

    public override void SetDefaultActions()
    {
        ActionGoap[] allActions = this.gameObject.GetComponents<ActionGoap>();

        for (int i = 0; i < allActions.Length; i++)
        {
            DestroyImmediate(allActions[i]);
        }

        if (this.gameObject.gameObject.GetComponent<Eat>() == null)
            this.gameObject.gameObject.AddComponent<Eat>();
        if (this.gameObject.gameObject.GetComponent<PickUpBread>() == null)
            this.gameObject.gameObject.AddComponent<PickUpBread>();

    }
}
