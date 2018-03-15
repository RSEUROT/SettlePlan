using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PickUpOre : ActionGoap
{
    private bool hasOre = false;
    private SupplyHouse targetSupplyPile; // where we get the logs from
    public int numOresNeeded = 1;

    public PickUpOre()
    {
        AddEffect("hasOres", true); // we now have a logs
    }
    public void SetNumOresNeeded(int _numOresNeeded)
    {
        numOresNeeded = _numOresNeeded;
    }
    public override void DoReset()
    {
        hasOre = false;
        targetSupplyPile = null;
    }
    public override bool IsDone()
    {
        return hasOre;
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
            if (supplyPile.GetNumOres() > numOresNeeded)
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
        Debug.Log("Perform Pickup Ore");

        PeopleInventory myInventory = (PeopleInventory)agent.GetComponent(typeof(PeopleInventory));
        if (targetSupplyPile.GetNumOres() >= numOresNeeded)
        {
            targetSupplyPile.UpdateNumOres(-numOresNeeded);
            hasOre = true;
            myInventory.NumOres += numOresNeeded;

            return true;
        }
        return false;
    }
    public override string GetAnimationName()
    {
        return "";
    }

}
