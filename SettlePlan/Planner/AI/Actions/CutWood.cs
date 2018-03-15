using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutWood : ActionGoap
{
    private bool chopped = false;
    private TreeComponent targetTree;

    private float startTimer = 0.0f;

    [SerializeField]
    private float workDuration = 2.0f;

    public CutWood()
    {
        AddPrecondition("hasLogs", false);
        AddEffect("hasLogs", true);
    }

    public override void DoReset()
    {
        chopped = false;
        targetTree = null;
        startTimer = 0.0f;
    }
    public override bool IsDone()
    {
        return chopped;
    }

    public override bool RequiresInRange()
    {
        return true;
    }

    public override bool CheckProceduralPrecondition(GameObject agent)
    {
        // find the nearest tree that we can chop
        TreeComponent[] trees = (TreeComponent[])UnityEngine.GameObject.FindObjectsOfType(typeof(TreeComponent));
        TreeComponent closest = null;
        float closestDist = 0;

        foreach (TreeComponent tree in trees)
        {
            if (closest == null)
            {
                // first one, so choose it for now
                closest = tree;
                closestDist = (tree.gameObject.transform.position - agent.transform.position).magnitude;
            }
            else
            {
                // is this one closer than the last?
                float dist = (tree.gameObject.transform.position - agent.transform.position).magnitude;
                if (dist < closestDist)
                {
                    // we found a closer one, use it
                    closest = tree;
                    closestDist = dist;
                }
            }
        }
        if (closest == null)
            return false;

        targetTree = closest;
        Target = targetTree.gameObject;

        return closest != null;
    }

    public override bool Perform(GameObject agent)
    {

        startTimer += Time.deltaTime;

        if (startTimer > workDuration)
        {
            // finished chopping
            PeopleInventory myInventory = (PeopleInventory)agent.GetComponent(typeof(PeopleInventory));

            myInventory.NumLog += 1;
            chopped = true;
        }
        return true;
    }
}
