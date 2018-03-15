using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Builder))]
public class BuilderHouseScriptEditor : Editor
{
    Builder myTarget;

    public override void OnInspectorGUI()
    {
        myTarget = (Builder)target;

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
        if (myTarget.gameObject.GetComponent<Build>() == null)
            myTarget.gameObject.AddComponent<Build>();
        if (myTarget.gameObject.GetComponent<PickUpLog>() == null)
            myTarget.gameObject.AddComponent<PickUpLog>();
        if (myTarget.gameObject.GetComponent<PickUpOre>() == null)
            myTarget.gameObject.AddComponent<PickUpOre>();
        if (myTarget.gameObject.GetComponent<MineOre>() == null)
            myTarget.gameObject.AddComponent<MineOre>();
        if (myTarget.gameObject.GetComponent<DropOre>() == null)
            myTarget.gameObject.AddComponent<DropOre>();
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
public class Builder : People
{
    public override List<KeyValuePair<string, object>> CreateGoalState()
    {
        List<KeyValuePair<string, object>> goal = new List<KeyValuePair<string, object>>();

        goal.Add(new KeyValuePair<string, object>("hasBuildHouse", true));

        return goal;
    }

    public override void SetDefaultActions()
    {
        ActionGoap[] allActions = this.gameObject.GetComponents<ActionGoap>();

        for (int i = 0; i < allActions.Length; i++)
        {
            DestroyImmediate(allActions[i]);
        }

        if (this.gameObject.GetComponent<Build>() == null)
            this.gameObject.AddComponent<Build>();
        if (this.gameObject.GetComponent<PickUpLog>() == null)
            this.gameObject.AddComponent<PickUpLog>();
        if (this.gameObject.GetComponent<PickUpOre>() == null)
            this.gameObject.AddComponent<PickUpOre>();
        if (this.gameObject.GetComponent<MineOre>() == null)
            this.gameObject.AddComponent<MineOre>();
        if (this.gameObject.GetComponent<DropOre>() == null)
            this.gameObject.AddComponent<DropOre>();
        if (this.gameObject.GetComponent<CutWood>() == null)
            this.gameObject.AddComponent<CutWood>();
        if (this.gameObject.GetComponent<DropLog>() == null)
            this.gameObject.AddComponent<DropLog>();
    }
}
