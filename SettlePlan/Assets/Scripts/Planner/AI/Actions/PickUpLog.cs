using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PickUpLog : ActionGoap
{
    private bool hasLog = false;
    private SupplyHouse targetSupplyPile; // where we get the logs from
    public int numLogsNeeded = 1;

    public PickUpLog()
    {
        AddEffect("hasLogs", true); // we now have a logs
    }
    public void SetNumLogsNeeded(int _numLogsNeeded)
    {
        numLogsNeeded = _numLogsNeeded;
    }
    public override void DoReset()
    {
        hasLog = false;
        targetSupplyPile = null;
    }
    public override bool IsDone()
    {
        return hasLog;
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
            if (supplyPile.GetNumLogs() > numLogsNeeded)
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
        }
        if (closest == null)
            return false;

        targetSupplyPile = closest;
        MyTarget = targetSupplyPile.gameObject;

        return closest != null;
    }

    public override bool Perform(GameObject agent)
    {
        Debug.Log("Perform Pickup log");
        PeopleInventory myInventory = (PeopleInventory)agent.GetComponent(typeof(PeopleInventory));
        if (targetSupplyPile.GetNumLogs() >= numLogsNeeded)
        {
            targetSupplyPile.UpdateNumLogs(-numLogsNeeded);
            hasLog = true;
            myInventory.NumLogs += numLogsNeeded;

            return true;
        }
        return false;
    }
    public override string GetAnimationName()
    {
        return "";
    }

}
