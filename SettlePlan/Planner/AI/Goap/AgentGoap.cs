using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AgentGoap : MonoBehaviour
{
    private FSM stateMachine;

    private FSM.FSMState idleState; // finds something to do
    private FSM.FSMState moveToState; // moves to a target
    private FSM.FSMState performActionState; // performs an action

    private List<ActionGoap> availableActions;
    private List<ActionGoap> currentActions;
    private PlannerGoap myPlanner;

    private InterfaceGoap kindOfPeopleDatas;

    private NavMeshAgent agent;

    private void Start()
    {
        stateMachine = new FSM();

        availableActions = new List<ActionGoap>();
        currentActions = new List<ActionGoap>();
        myPlanner = new PlannerGoap();

        FindkindOfPeople();

        RecordStates();

        stateMachine.pushState(idleState);

        LoadActions();

        agent = GetComponent<NavMeshAgent>();
    }
    private void Update()
    {
        stateMachine.Update(this.gameObject);
    }

    public void ChangeKindOfPeople()
    {

    }
    private void FindkindOfPeople()
    {
        foreach (Component comp in gameObject.GetComponents(typeof(Component)))
        {
            if (typeof(InterfaceGoap).IsAssignableFrom(comp.GetType()))
            {
                kindOfPeopleDatas = (InterfaceGoap)comp;
                return;
            }
        }
    }

    private void RecordStates()
    {
        idleState = IdleState;
        moveToState = MoveToState;
        performActionState = PerformAction;
    }
    private void IdleState(FSM fsm, GameObject gameObj)
    {
        Debug.Log("Idle");
        List<KeyValuePair<string, object>> worldState = kindOfPeopleDatas.GetWorldState();
        List<KeyValuePair<string, object>> goal = kindOfPeopleDatas.CreateGoalState();

        //agent.destination = transform.position;
        // Plan
        List<ActionGoap> plan = myPlanner.Plan(gameObject, availableActions, worldState, goal);
        if (plan != null)
        {
            // we have a plan, hooray!
            currentActions = plan;
            kindOfPeopleDatas.PlanFound(goal, plan);

            fsm.popState(); // move to PerformAction state
            fsm.pushState(performActionState);

        }
        else
        {
            // ugh, we couldn't get a plan
            //Debug.Log("<color=orange>Failed Plan:</color>" + prettyPrint(goal));
            kindOfPeopleDatas.PlanFailed(goal);
            fsm.popState(); // move back to IdleAction state
            fsm.pushState(idleState);
        }
    }
    private void MoveToState(FSM fsm, GameObject gameObj)
    {
        // move the game object
        Debug.Log("MoveTo ");
        ActionGoap action = currentActions[0];
        if (action.RequiresInRange() && action.Target == null)
        {
            //Debug.Log("<color=red>Fatal error:</color> Action requires a target but has none. Planning failed. You did not assign the target in your Action.checkProceduralPrecondition()");
            fsm.popState(); // move
            fsm.popState(); // perform
            fsm.pushState(idleState);
            return;
        }

        // get the agent to move itself
        if (kindOfPeopleDatas.MoveAgent(action))
        {
            fsm.popState();
        }
    }
    private void PerformAction(FSM fsm, GameObject gameObj)
    {
        // perform the action
        Debug.Log("PerformAction");
        //agent.destination = transform.position;
        if (!HasActionPlan())
        {
            // no actions to perform
            Debug.Log("<color=red>Done actions</color>");
            fsm.popState();
            fsm.pushState(idleState);
            kindOfPeopleDatas.ActionsFinished();
            return;
        }

        ActionGoap action = currentActions[0];
        if (action.IsDone())
        {
            currentActions.RemoveAt(0);
        }

        if (HasActionPlan())
        {
            // perform the next action
            action = currentActions[0];
            bool inRange = action.RequiresInRange() ? action.IsInRange() : true;

            if (inRange)
            {
                // we are in range, so perform the action
                bool success = action.Perform(gameObj);

                if (!success)
                {
                    // action failed, we need to plan again
                    fsm.popState();
                    fsm.pushState(idleState);
                    kindOfPeopleDatas.PlanAborted(action);
                }
            }
            else
            {
                // we need to move there first
                // push moveTo state
                fsm.pushState(moveToState);
            }

        }
        else
        {
            // no actions left, move to Plan state
            fsm.popState();
            fsm.pushState(idleState);
            kindOfPeopleDatas.ActionsFinished();
        }
    }

    public void AddAction(ActionGoap a)
    {
        availableActions.Add(a);
    }
    public ActionGoap GetAction(Type action)
    {
        foreach (ActionGoap g in availableActions)
        {
            if (g.GetType().Equals(action))
                return g;
        }
        return null;
    }
    public void RemoveAction(ActionGoap action)
    {
        availableActions.Remove(action);
    }
    private bool HasActionPlan()
    {
        return currentActions.Count > 0;
    }

    private void LoadActions()
    {
        ActionGoap[] actions = gameObject.GetComponents<ActionGoap>();
        foreach (ActionGoap a in actions)
        {
            availableActions.Add(a);
        }
    }

}
