using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BuildHouse : ActionGoap
{
    private bool isDone = false;
    private float timerToPerform = 0.0f;

    [SerializeField]
    private float workDuration = 20.0f;

    public BuildHouse()
    {
        AddPrecondition("hasLogs", true);
        AddPrecondition("hasOres", true);
        AddPrecondition("hasBuildHouse", false);
        AddEffect("hasBuildHouse", true);
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
        PeopleInventory myInventory = (PeopleInventory)agent.GetComponent(typeof(PeopleInventory));
        bool toReturn = true;

        if(myInventory.NumLogs < 15 || myInventory.NumOres < 20)
        {
            toReturn = false;
        }

        return toReturn;
    }
    public override bool Perform(GameObject agent)
    {
        timerToPerform += Time.deltaTime;
        
        if (timerToPerform > workDuration)
        {
            // finished chopping
            PeopleInventory myInventory = (PeopleInventory)agent.GetComponent(typeof(PeopleInventory));

            myInventory.NumLogs -= 15;
            myInventory.NumOres -= 20;
            isDone = true;
        }
        return true;
    }
    public override string GetAnimationName()
    {
        return "Mine";
    }
}
