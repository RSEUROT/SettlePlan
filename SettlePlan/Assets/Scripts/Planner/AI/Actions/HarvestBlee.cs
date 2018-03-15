using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HarvestBlee : ActionGoap
{
    private bool isDone = false;
    private float timerToPerform = 0.0f;

    [SerializeField]
    private float workDuration = 2.0f;

    [SerializeField]
    private int bleeGain = 7;
    [SerializeField]
    private int randomAroundGains = 1;


    public HarvestBlee()
    {
        AddPrecondition("hasBlee", false);
        AddEffect("hasBlee", true);
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
        FieldComponent[] blees = (FieldComponent[])UnityEngine.GameObject.FindObjectsOfType(typeof(FieldComponent));
        FieldComponent closest = null;
        float closestDist = 0;

        foreach (FieldComponent blee in blees)
        {
            if (closest == null)
            {
                // first one, so choose it for now
                closest = blee;
                closestDist = (blee.gameObject.transform.position - agent.transform.position).magnitude;
            }
            else
            {
                // is this one closer than the last?
                float dist = (blee.gameObject.transform.position - agent.transform.position).magnitude;
                if (dist < closestDist)
                {
                    // we found a closer one, use it
                    closest = blee;
                    closestDist = dist;
                }
            }
        }
        if (closest == null)
            return false;

        MyTarget = closest.gameObject;

        return closest != null;
    }
    public override bool Perform(GameObject agent)
    {
        timerToPerform += Time.deltaTime;
        
        if (timerToPerform > workDuration)
        {
            // finished chopping
            PeopleInventory myInventory = (PeopleInventory)agent.GetComponent(typeof(PeopleInventory));

            int gain = bleeGain + Random.Range(-randomAroundGains, randomAroundGains);
            gain = (gain < 0) ? 0 : gain;
            myInventory.NumBlee += gain;
            isDone = true;
        }
        return true;
    }
    public override string GetAnimationName()
    {
        return "Farmer";
    }
}
