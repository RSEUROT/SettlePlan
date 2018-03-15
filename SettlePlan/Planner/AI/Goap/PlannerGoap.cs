using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlannerGoap
{
    /**
	 * Plan what sequence of actions can fulfill the goal.
	 * Returns null if a plan could not be found, or a list of the actions
	 * that must be performed, in order, to fulfill the goal.
	 */
    public List<ActionGoap> Plan(GameObject agent, List<ActionGoap> availableActions, List<KeyValuePair<string, object>> worldState, List<KeyValuePair<string, object>> goal)
    {
        // reset the actions so we can start fresh with them
        foreach (ActionGoap a in availableActions)
        {
            a.ResetAll();
        }

        // check what actions can run using their checkProceduralPrecondition
        List<ActionGoap> usableActions = new List<ActionGoap>();
        foreach (ActionGoap a in availableActions)
        {
            if (a.CheckProceduralPrecondition(agent))
                usableActions.Add(a);
        }

        // we now have all actions that can run, stored in usableActions

        // build up the tree and record the leaf nodes that provide a solution to the goal.
        List<Node> leaves = new List<Node>();

        // build graph
        Node start = new Node(null, 0, worldState, null);
        bool success = BuildGraph(start, leaves, usableActions, goal);

        if (!success)
        {
            // oh no, we didn't get a plan
            Debug.Log("NO PLAN");
            return null;
        }

        // get the cheapest leaf
        Node cheapest = null;
        foreach (Node leaf in leaves)
        {
            if (cheapest == null)
                cheapest = leaf;
            else
            {
                if (leaf.runningCost < cheapest.runningCost)
                    cheapest = leaf;
            }
        }


        // get its node and work back through the parents
        List<ActionGoap> result = new List<ActionGoap>();
        Node n = cheapest;
        while (n != null)
        {
            if (n.Action != null)
            {
                result.Insert(0, n.Action); // insert the action in the front
            }
            n = n.parent;
        }

        // we now have this action list in correct order

        return result;
    }

    /**
 * Returns true if at least one solution was found.
 * The possible paths are stored in the leaves list. Each leaf has a
 * 'runningCost' value where the lowest cost will be the best action
 * sequence.
 */
    private bool BuildGraph(Node parent, List<Node> leaves, List<ActionGoap> usableActions, List<KeyValuePair<string, object>> goal)
    {
        bool foundOne = false;

        // go through each action available at this node and see if we can use it here
        foreach (ActionGoap action in usableActions)
        {

            // if the parent state has the conditions for this action's preconditions, we can use it here
            if (InState(action.Preconditions, parent.state))
            {

                // apply the action's effects to the parent state
                List<KeyValuePair<string, object>> currentState = PopulateState(parent.state, action.Effects);
                //Debug.Log(GoapAgent.prettyPrint(currentState));
                Node node = new Node(parent, parent.runningCost + action.cost, currentState, action);

                if (InState(goal, currentState))
                {
                    // we found a solution!
                    leaves.Add(node);
                    foundOne = true;
                }
                else
                {
                    // not at a solution yet, so test all the remaining actions and branch out the tree
                    List<ActionGoap> subset = ActionSubset(usableActions, action);
                    bool found = BuildGraph(node, leaves, subset, goal);
                    if (found)
                        foundOne = true;
                }
            }
        }

        return foundOne;
    }

    /**
	 * Create a subset of the actions excluding the removeMe one. Creates a new set.
	 */
    private List<ActionGoap> ActionSubset(List<ActionGoap> actions, ActionGoap removeMe)
    {
        List<ActionGoap> subset = new List<ActionGoap>();
        foreach (ActionGoap a in actions)
        {
            if (!a.Equals(removeMe))
                subset.Add(a);
        }
        return subset;
    }

    /**
	 * Check that all items in 'test' are in 'state'. If just one does not match or is not there
	 * then this returns false.
	 */
    private bool InState(List<KeyValuePair<string, object>> test, List<KeyValuePair<string, object>> state)
    {
        bool allMatch = true;
        foreach (KeyValuePair<string, object> t in test)
        {
            bool match = false;
            foreach (KeyValuePair<string, object> s in state)
            {
                if (s.Equals(t))
                {
                    match = true;
                    break;
                }
            }
            if (!match)
            {
                allMatch = false;
                break;
            }
        }
        return allMatch;
    }

    /**
	 * Apply the stateChange to the currentState
	 */
    private List<KeyValuePair<string, object>> PopulateState(List<KeyValuePair<string, object>> currentState, List<KeyValuePair<string, object>> stateChange)
    {
        List<KeyValuePair<string, object>> state = new List<KeyValuePair<string, object>>();
        // copy the KVPs over as new objects
        foreach (KeyValuePair<string, object> s in currentState)
        {
            state.Add(new KeyValuePair<string, object>(s.Key, s.Value));
        }

        foreach (KeyValuePair<string, object> change in stateChange)
        {
            // if the key exists in the current state, update the Value
            bool exists = false;

            foreach (KeyValuePair<string, object> s in state)
            {
                if (s.Equals(change))
                {
                    exists = true;
                    break;
                }
            }

            if (exists)
            {
                KeyValuePair<string, object> toRemove = new KeyValuePair<string, object>();
                foreach (KeyValuePair<string, object> temp in state)
                {
                    if(temp.Key == change.Key)
                    {
                        toRemove = temp;
                    }
                }
                if(state.Contains(toRemove))
                    state.Remove(toRemove);


                KeyValuePair<string, object> updated = new KeyValuePair<string, object>(change.Key, change.Value);
                state.Add(updated);
            }
            // if it does not exist in the current state, add it
            else
            {
                state.Add(new KeyValuePair<string, object>(change.Key, change.Value));
            }
        }
        return state;
    }


    /**
 * Used for building up the graph and holding the running costs of actions.
 */
    private class Node
    {
        public Node parent;
        public float runningCost;
        public List<KeyValuePair<string, object>> state;
        private ActionGoap action;
        public string actionName;

        public ActionGoap Action
        {
            get
            {
                return action;
            }

            set
            {
                action = value;
                if (action != null)
                    actionName = action.name;
            }
        }

        public Node(Node parent, float runningCost, List<KeyValuePair<string, object>> state, ActionGoap action)
        {
            this.parent = parent;
            this.runningCost = runningCost;
            this.state = state;
            this.Action = action;
        }
    }

}
