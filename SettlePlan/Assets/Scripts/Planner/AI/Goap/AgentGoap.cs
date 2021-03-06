﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AgentGoap : MonoBehaviour
{
    public string currentState = "";

    private FSM stateMachine;

    private FSMState idleState; // finds something to do
    private FSMState moveToState; // moves to a target
    private FSMState performActionState; // performs an action

    private List<ActionGoap> availableActions;
    private List<ActionGoap> currentActions;
    private PlannerGoap myPlanner;

    private bool hasABehaviour;

    private string PeopleDatasName;
    public string previousPeopleDatasName = "";

    private IGoap PeopleDatas;

    private NavMeshAgent agent;
    private Animator anim;

    [SerializeField]
    private float currentHungryRate;
    [SerializeField]
    private float maxHungryRate = 100.0f;

    public float ratio;

    void Start()
    {
        hasABehaviour = false;
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponentInChildren<Animator>();

        stateMachine = new FSM();

        availableActions = new List<ActionGoap>();
        currentActions = new List<ActionGoap>();

        myPlanner = new PlannerGoap();

        CheakForJobs();

        EconomicBrainGoap.Instance.AgentInactive(this);
    }

    private bool FindPeopleDatas()
    {
        hasABehaviour = false;
        foreach (Component comp in gameObject.GetComponents(typeof(Component)))
        {
            if (typeof(IGoap).IsAssignableFrom(comp.GetType()))
            {
                PeopleDatas = (IGoap)comp;
                previousPeopleDatasName = PeopleDatasName;
                PeopleDatasName = PeopleDatas.ToString();
                hasABehaviour = true;
                break;
            }
        }

        return hasABehaviour;
    }
    public void RemovePeopleDatas()
    {
        
        foreach (Component comp in gameObject.GetComponents(typeof(Component)))
        {
            if (typeof(IGoap).IsAssignableFrom(comp.GetType()))
            {
                //PeopleDatas = null;
                Destroy(comp);
                hasABehaviour = false;
                availableActions.Clear();
                break;
            }
        }
        if (currentActions != null && currentActions.Count >= 1)
        {
            ActionGoap action = currentActions[0];
            if (action.GetAnimationName() != string.Empty)
                anim.SetBool(action.GetAnimationName(), false);
        }
    }

    public void Eat()
    {
        currentHungryRate = maxHungryRate;
        RemovePeopleDatas();

        if (previousPeopleDatasName.Contains("Logger"))
        {
            this.gameObject.AddComponent<Logger>();
            GetComponent<Logger>().SetDefaultActions();
        }
        else if (previousPeopleDatasName.Contains("Blacksmith"))
        {
            this.gameObject.AddComponent<Blacksmith>();
            GetComponent<Blacksmith>().SetDefaultActions();
        }
        else if (previousPeopleDatasName.Contains("Cooker"))
        {
            this.gameObject.AddComponent<Cooker>();
            GetComponent<Cooker>().SetDefaultActions();
        }
        else if (previousPeopleDatasName.Contains("Eater"))
        {
            this.gameObject.AddComponent<Eater>();
            GetComponent<Eater>().SetDefaultActions();
        }
        else if (previousPeopleDatasName.Contains("Farmer"))
        {
            this.gameObject.AddComponent<Farmer>();
            GetComponent<Farmer>().SetDefaultActions();
        }
        else if (previousPeopleDatasName.Contains("Miner"))
        {
            this.gameObject.AddComponent<Miner>();
            GetComponent<Miner>().SetDefaultActions();
        }


    }

    private void IdleState(FSM fsm, GameObject gameObj)
    {
        currentState = "idle";
        List<KeyValuePair<string, object>> worldState = PeopleDatas.GetWorldState();
        List<KeyValuePair<string, object>> goal = PeopleDatas.CreateGoalState();

        agent.destination = transform.position;
        // Plan
        List<ActionGoap> plan = myPlanner.Plan(gameObject, availableActions, worldState, goal);
        if (plan != null)
        {
            // we have a plan, hooray!
            currentActions = plan;
            PeopleDatas.PlanFound(goal, plan);

            fsm.popState(); // move to PerformAction state
            fsm.pushState(performActionState);

        }
        else
        {
            // ugh, we couldn't get a plan
            //Debug.Log("<color=orange>Failed Plan:</color>" + prettyPrint(goal));
            PeopleDatas.PlanFailed(goal);
            fsm.popState(); // move back to IdleAction state
            fsm.pushState(idleState);

            CheakForJobs();
        }

    }
    private void MoveToState(FSM fsm, GameObject gameObj)
    {
        currentState = "moveto";
        // move the game object
        ActionGoap action = currentActions[0];
        if (action.RequiresInRange() && action.MyTarget == null)
        {
            //Debug.Log("<color=red>Fatal error:</color> Action requires a target but has none. Planning failed. You did not assign the target in your Action.checkProceduralPrecondition()");
            fsm.popState(); // move
            fsm.popState(); // perform
            fsm.pushState(idleState);
            return;
        }

        // get the agent to move itself
        if (PeopleDatas.MoveAgent(action))
        {
            fsm.popState();
        }
    }
    private void PerformAction(FSM fsm, GameObject gameObj)
    {
        currentState = "perform";
        // perform the action
        agent.destination = transform.position;
        if (!HasActionPlan())
        {
            // no actions to perform
            Debug.Log("<color=red>Done actions</color>");
            fsm.popState();
            fsm.pushState(idleState);
            PeopleDatas.ActionsFinished();
            return;
        }

        ActionGoap action = currentActions[0];
        
        if (action.IsDone())
        {
            if(action.GetAnimationName() != string.Empty)
                anim.SetBool(action.GetAnimationName(), false);
            currentActions.RemoveAt(0);
        }

        if (HasActionPlan())
        {
            // perform the next action
            action = currentActions[0];
            string actionAnimName = action.GetAnimationName();

            bool inRange = action.RequiresInRange() ? action.IsInRange() : true;

            if (inRange)
            {
                // we are in range, so perform the action
                bool success = action.Perform(gameObj);
                if (actionAnimName != string.Empty)
                    anim.SetBool(action.GetAnimationName(), true);

                if (!success)
                {
                    if (actionAnimName != string.Empty)
                        anim.SetBool(action.GetAnimationName(), false);
                    // action failed, we need to plan again
                    fsm.popState();
                    fsm.pushState(idleState);
                    PeopleDatas.PlanAborted(action);
                }
            }
            else
            {
                // we need to move there first
                // push moveTo state
                if (actionAnimName != string.Empty)
                    anim.SetBool(action.GetAnimationName(), false);
                fsm.pushState(moveToState);
            }

        }
        else
        {
            // no actions left, move to Plan state
            fsm.popState();
            fsm.pushState(idleState);
            PeopleDatas.ActionsFinished();

            EconomicBrainGoap.Instance.AgentInactive(this);
        }
    }
    
    // Update is called once per frame
    void Update ()
    {
        anim.SetFloat("speed", Vector3.Magnitude(agent.velocity));
        if(hasABehaviour)
            stateMachine.Update(this.gameObject);

        UpdateHungry();
        if (!hasABehaviour)
        {
            CheakForJobs();
        }
    }
    private void UpdateHungry()
    {
        currentHungryRate -= (Time.deltaTime * maxHungryRate * 0.0001f);
        ratio = currentHungryRate / maxHungryRate;
        currentHungryRate = (currentHungryRate >= 0.0f) ? currentHungryRate : 0.0f;
    }

    public void CheakForJobs()
    {
        if (FindPeopleDatas())
        {
            idleState = IdleState;
            moveToState = MoveToState;
            performActionState = PerformAction;

            stateMachine.pushState(idleState);
            LoadActions();
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
        if(availableActions.Contains(action))
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
