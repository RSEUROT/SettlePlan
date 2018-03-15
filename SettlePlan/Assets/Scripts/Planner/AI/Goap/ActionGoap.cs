using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class ActionGoap : MonoBehaviour
{
    private List<KeyValuePair<string, object>> preconditions;
    private List<KeyValuePair<string, object>> effects;
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
    
    [SerializeField]
    private float cost;
    public float Cost
    {
        get
        {
            return cost;
        }

        set
        {
            cost = value;
        }
    }

    private GameObject myTarget;
    public GameObject MyTarget
    {
        get
        {
            return myTarget;
        }

        set
        {
            myTarget = value;
        }
    }
    protected bool needToBeInRangeOfTarget = false;

    public ActionGoap()
    {
        preconditions = new List<KeyValuePair<string, object>>();
        effects = new List<KeyValuePair<string, object>>();
    }

    public bool IsInRange()
    {
        return needToBeInRangeOfTarget;
    }
    public void SetInRange(bool inRange)
    {
        this.needToBeInRangeOfTarget = inRange;
    }

    public void Reset()
    {
        needToBeInRangeOfTarget = false;
        myTarget = null;
        DoReset();
    }

    public void AddPrecondition(string _key, object _value)
    {
        preconditions.Add(new KeyValuePair<string, object>(_key, _value));
    }
    public bool RemovePrecondition(string _key)
    {
        KeyValuePair<string, object> tempToRemove = default(KeyValuePair<string, object>);

        foreach (KeyValuePair<string, object> tempCurrent in preconditions)
        {
            if (tempCurrent.Key == _key)
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
    

    public abstract void DoReset();
    public abstract bool IsDone();
    public abstract bool CheckProceduralPrecondition(GameObject agent);
    public abstract bool Perform(GameObject agent);
    public abstract bool RequiresInRange();
    public abstract string GetAnimationName();
}
