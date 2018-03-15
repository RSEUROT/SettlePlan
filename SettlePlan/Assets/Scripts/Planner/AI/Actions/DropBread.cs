using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DropBread : ActionGoap
{
    private bool droppedOffBread = false;
    private SupplyHouse targetSupplyPile; // where we get the logs from

    public DropBread()
    {
        AddPrecondition("hasBread", true); // don't get a logs if we already have one
        AddEffect("hasBread", false); // we now have a logs
        AddEffect("cookFood", true); // we collected logs
    }
    public override void DoReset()
    {
        droppedOffBread = false;
        targetSupplyPile = null;
    }
    public override bool IsDone()
    {
        return droppedOffBread;
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
        PeopleInventory myInventory = (PeopleInventory)agent.GetComponent(typeof(PeopleInventory));
        targetSupplyPile.UpdateNumBread(myInventory.NumBreads);
        droppedOffBread = true;
        myInventory.NumBreads = 0;

        return true;
    }
    public override string GetAnimationName()
    {
        return "";
    }

}
