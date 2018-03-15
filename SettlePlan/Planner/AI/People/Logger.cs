using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Logger : People
{
    public override List<KeyValuePair<string, object>> CreateGoalState()
    {
        List<KeyValuePair<string, object>> goal = new List<KeyValuePair<string, object>>();

        goal.Add(new KeyValuePair<string, object>("collectLogs", true));

        return goal;
    }
}
