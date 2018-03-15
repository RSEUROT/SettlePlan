using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;


public abstract class People : MonoBehaviour, IGoap
{
    public float moveSpeed = 3;

    private NavMeshAgent agent;
    private bool hasAlreadyADestination;
    private PeopleInventory myInventory;
    

    // Use this for initialization
    void Start ()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = moveSpeed;

        hasAlreadyADestination = false;

        myInventory = GetComponent<PeopleInventory>();
        if(myInventory == null)
        {
            myInventory = this.gameObject.AddComponent<PeopleInventory>();
        }
    }

    public List<KeyValuePair<string, object>> GetWorldState()
    {
        List<KeyValuePair<string, object>> WorldData = new List<KeyValuePair<string, object>>();

        WorldData.Add(new KeyValuePair<string, object>("hasLogs", (myInventory.NumLogs > 0)));
        WorldData.Add(new KeyValuePair<string, object>("hasOres", (myInventory.NumOres > 0)));
        WorldData.Add(new KeyValuePair<string, object>("hasBlee", (myInventory.NumBlee > 0)));
        WorldData.Add(new KeyValuePair<string, object>("hasBread", (myInventory.NumBreads > 0)));

        return WorldData;
    }
    public abstract List<KeyValuePair<string, object>> CreateGoalState();

    public bool MoveAgent(ActionGoap nextAction)
    {
        float remainingDistance = nextAction.MyTarget.GetComponent<ObjectStatic>().GetRemainingDistance();
        Vector3 tempDest = nextAction.MyTarget.transform.position + (Vector3.Normalize(transform.position - nextAction.MyTarget.transform.position) * remainingDistance);
        if (!hasAlreadyADestination)
        {
            NavMeshHit hit;
            bool hasFindResult = true;
            hasFindResult = NavMesh.SamplePosition(tempDest, out hit, 10.0f, NavMesh.AllAreas);
            agent.destination = hit.position;
            hasAlreadyADestination = true;
        }

        if (Vector3.Distance(agent.destination, transform.position) <= 1.5f)
        {
            nextAction.SetInRange(true);
            hasAlreadyADestination = false;
            return true;
        }
        return false;
    }
    public void PlanFailed(List<KeyValuePair<string, object>> failedGoal)
    {
        
    }
    public void PlanFound(List<KeyValuePair<string, object>> goal, List<ActionGoap> actions)
    {
        
    }
    public void ActionsFinished()
    {
        
    }
    public void PlanAborted(ActionGoap aborter)
    {
        
    }

    public abstract void SetDefaultActions();
}
