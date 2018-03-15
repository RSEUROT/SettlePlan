using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Cook : ActionGoap
{
    private bool isDone = false;
    private float timerToPerform = 0.0f;

    [SerializeField]
    private float workDuration = 2.0f;

    [SerializeField]
    private int breadsGain = 5;
    [SerializeField]
    private int randomAroundGains = 1;
    
    public Cook()
    {
        AddPrecondition("hasBlee", true);
        AddPrecondition("hasBread", false);
        AddEffect("hasBread", true);
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
        WillComponent[] wills = (WillComponent[])UnityEngine.GameObject.FindObjectsOfType(typeof(WillComponent));
        WillComponent closest = null;
        float closestDist = 0;

        foreach (WillComponent will in wills)
        {
            if (closest == null)
            {
                // first one, so choose it for now
                closest = will;
                closestDist = (will.gameObject.transform.position - agent.transform.position).magnitude;
            }
            else
            {
                // is this one closer than the last?
                float dist = (will.gameObject.transform.position - agent.transform.position).magnitude;
                if (dist < closestDist)
                {
                    // we found a closer one, use it
                    closest = will;
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
            if (myInventory.NumBlee > 0)
            {
                int gain = breadsGain + Random.Range(-randomAroundGains, randomAroundGains);
                gain = (gain < 0) ? 0 : gain;
                myInventory.NumBreads += gain;
                myInventory.NumBlee -= 1;
                isDone = true;
            }
        }
        return true;
    }
    public override string GetAnimationName()
    {
        return "Cook";
    }
}
