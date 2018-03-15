using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ActionGoap : MonoBehaviour
{

    private List<KeyValuePair<string, object>> preconditions;
    private List<KeyValuePair<string, object>> effects;

    private bool inRangeOfTarget = false;

    public float cost = 1.0f;

    private GameObject target;

    public List<KeyValuePair<string, object>> Preconditions
    {
        get
        {
            return preconditions;
        }
    }
    public List<KeyValuePair<string, object>> Effects
    {
        get
        {
            return effects;
        }
    }

    public GameObject Target
    {
        get
        {
            return target;
        }

        set
        {
            target = value;
        }
    }

    public ActionGoap()
    {
        preconditions = new List<KeyValuePair<string, object>>();
        effects = new List<KeyValuePair<string, object>>();
    }
    public void ResetAll()
    {
        inRangeOfTarget = false;
        target = null;
        DoReset();
    }

    /**
 * Reset any variables that need to be reset before planning happens again.
 */
    public abstract void DoReset();
    /**
	 * Is the action done?
	 */
    public abstract bool IsDone();
    /**
	 * Procedurally check if this action can run. Not all actions
	 * will need this, but some might.
	 */
    public abstract bool CheckProceduralPrecondition(GameObject agent);
    /**
	 * Run the action.
	 * Returns True if the action performed successfully or false
	 * if something happened and it can no longer perform. In this case
	 * the action queue should clear out and the goal cannot be reached.
	 */
    public abstract bool Perform(GameObject agent);
    /**
	 * Does this action need to be within range of a target game object?
	 * If not then the moveTo state will not need to run for this action.
	 */
    public abstract bool RequiresInRange();

    /**
	 * Are we in range of the target?
	 * The MoveTo state will set this and it gets reset each time this action is performed.
	 */
    public bool IsInRange()
    {
        return inRangeOfTarget;
    }
    public void SetInRange(bool inRange)
    {
        this.inRangeOfTarget = inRange;
    }

    public void AddPrecondition(string _key, object _value)
    {
        preconditions.Add(new KeyValuePair<string, object>(_key, _value));
    }
    public bool RemovePrecondition(string _key)
    {
        KeyValuePair<string, object> tempToRemove = default(KeyValuePair<string, object>);

        foreach(KeyValuePair<string, object> tempCurrent in preconditions)
        {
            if(tempCurrent.Key == _key)
            {
                tempToRemove = tempCurrent;
            }
        }

        if (preconditions.Contains(tempToRemove))
        {
            preconditions.Remove(tempToRemove);
            return true;
        }
        return false;
    }

    public void AddEffect(string _key, object _value)
    {
        effects.Add(new KeyValuePair<string, object>(_key, _value));
    }
    public bool RemoveEffect(string _key)
    {
        KeyValuePair<string, object> tempToRemove = default(KeyValuePair<string, object>);

        foreach (KeyValuePair<string, object> tempCurrent in effects)
        {
            if (tempCurrent.Key == _key)
            {
                tempToRemove = tempCurrent;
            }
        }

        if (effects.Contains(tempToRemove))
        {
            effects.Remove(tempToRemove);
            return true;
        }
        return false;
    }

}
