﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DropOre : ActionGoap
{
    private bool droppedOffOres = false;
    private SupplyHouse targetSupplyPile; // where we get the logs from

    public DropOre()
    {
        AddPrecondition("hasOres", true); // don't get a logs if we already have one
        AddEffect("hasOres", false); // we now have a logs
        AddEffect("collectOres", true); // we collected logs
    }
    public override void DoReset()
    {
        droppedOffOres = false;
        targetSupplyPile = null;
    }
    public override bool IsDone()
    {
        return droppedOffOres;
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
        targetSupplyPile.NumLogs += myInventory.NumLogs;
        droppedOffOres = true;
        myInventory.NumLogs = 0;

        return true;
    }
    public override string GetAnimationName()
    {
        return "";
    }

}
