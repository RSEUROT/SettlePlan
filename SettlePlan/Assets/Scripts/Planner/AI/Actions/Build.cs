using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Build : ActionGoap
{
    public int logResourcesNeeded;
    public int OreResourcesNeeded;

    public GameObject ObjectToBuildPrefab;
    public Vector3 newBuildingPosition;

    private SupplyHouse targetSupplyPile; // where we get the logs from

    private bool isDone = false;
    private float timerToPerform = 0.0f;

    [SerializeField]
    private float workDuration = 20.0f;

    public Build()
    {
        AddPrecondition("hasLogs", true);
        AddPrecondition("hasOres", true);
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
        bool toReturn = true;

        if(ObjectToBuildPrefab == null)
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

            myInventory.NumLogs -= logResourcesNeeded;
            myInventory.NumOres -= OreResourcesNeeded;
            isDone = true;
        }
        return true;
    }
    public override string GetAnimationName()
    {
        return "Mine";
    }
}
