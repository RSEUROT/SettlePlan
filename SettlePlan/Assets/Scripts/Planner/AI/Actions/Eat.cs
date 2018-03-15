using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Eat : ActionGoap
{
    private bool isDone = false;
    private float timerToPerform = 0.0f;

    [SerializeField]
    private float workDuration = 2.0f;

    [SerializeField]
    private int breadsConsuption = 1;

    private SupplyHouse targetSupplyPile; // where we get the logs from


    public Eat()
    {
        AddPrecondition("hasBread", true);
        AddEffect("hasBread", false);
        AddEffect("needToEat", true);
    }

    public override void DoReset()
    {
        isDone = false;
        timerToPerform = 0.0f;
    }
    public override bool IsDone()
    {
        return isDone;
    }

    public override bool RequiresInRange()
    {
        return true;
    }
    public override bool CheckProceduralPrecondition(GameObject agent)
    {
        // find the nearest tree that we can chop
        SupplyHouse[] supplyPiles = (SupplyHouse[])UnityEngine.GameObject.FindObjectsOfType(typeof(SupplyHouse));
        SupplyHouse closest = null;
        float closestDist = 0;

        foreach (SupplyHouse supplyPile in supplyPiles)
        {
            if (closest == null)
            {
                // first one, so choose it for now
                closest = supplyPile;
                closestDist = (supplyPile.gameObject.transform.position - agent.transform.position).magnitude;
            }
            else
            {
                // is this one closer than the last?
                float dist = (supplyPile.gameObject.transform.position - agent.transform.position).magnitude;
                if (dist < closestDist)
                {
                    // we found a closer one, use it
                    closest = supplyPile;
                    closestDist = dist;
                }
            }
        }
        if (closest == null)
            return false;

        targetSupplyPile = closest;
        MyTarget = targetSupplyPile.gameObject;

        return closest != null;
    }
    public override bool Perform(GameObject agent)
    {
        timerToPerform += Time.deltaTime;

        if (timerToPerform > workDuration)
        {
            // finished chopping
            PeopleInventory myInventory = (PeopleInventory)agent.GetComponent(typeof(PeopleInventory));
            myInventory.NumBreads -= breadsConsuption;
            GetComponent<AgentGoap>().Eat();
            isDone = true;
        }
        return true;
    }
    public override string GetAnimationName()
    {
        return "Cook";
    }
}
