using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Miner))]
public class MinerScriptEditor : Editor
{
    Miner myTarget;

    public override void OnInspectorGUI()
    {
        myTarget = (Miner)target;

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
        if (myTarget.gameObject.GetComponent<MineOre>() == null)
            myTarget.gameObject.AddComponent<MineOre>();
        if (myTarget.gameObject.GetComponent<DropOre>() == null)
            myTarget.gameObject.AddComponent<DropOre>();
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
public class Miner : People
{
    public override List<KeyValuePair<string, object>> CreateGoalState()
    {
        List<KeyValuePair<string, object>> goal = new List<KeyValuePair<string, object>>();

        goal.Add(new KeyValuePair<string, object>("collectOres", true));

        return goal;
    }

    public override void SetDefaultActions()
    {
        ActionGoap[] allActions = this.gameObject.GetComponents<ActionGoap>();

        for (int i = 0; i < allActions.Length; i++)
        {
            DestroyImmediate(allActions[i]);
        }

        if (this.gameObject.gameObject.GetComponent<MineOre>() == null)
            this.gameObject.gameObject.AddComponent<MineOre>();
        if (this.gameObject.gameObject.GetComponent<DropOre>() == null)
            this.gameObject.gameObject.AddComponent<DropOre>();

    }
}
