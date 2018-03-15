using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PickUpBlee : ActionGoap
{
    private bool hasBlee = false;
    private SupplyHouse targetSupplyPile; // where we get the logs from
    public int numBleeNeeded = 1;

    public PickUpBlee()
    {
        AddEffect("hasBlee", true); // we now have a logs
    }
    public void SetNumBleeNeeded(int _numBleeNeeded)
    {
        numBleeNeeded = _numBleeNeeded;
    }
    public override void DoReset()
    {
        hasBlee = false;
        targetSupplyPile = null;
    }
    public override bool IsDone()
    {
        return hasBlee;
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
            if (supplyPile.GetNumBlee() > numBleeNeeded)
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
        Debug.Log("Perform Pickup blee");
        PeopleInventory myInventory = (PeopleInventory)agent.GetComponent(typeof(PeopleInventory));
        if (targetSupplyPile.GetNumBlee() >= numBleeNeeded)
        {
            targetSupplyPile.UpdateNumBlee(-numBleeNeeded);
            hasBlee = true;
            myInventory.NumBlee += numBleeNeeded;

            return true;
        }
        return false;
    }
    public override string GetAnimationName()
    {
        return "";
    }

}
