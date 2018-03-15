using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGoap
{
    List<KeyValuePair<string, object>> GetWorldState();
    List<KeyValuePair<string, object>> CreateGoalState();

    void PlanFailed(List<KeyValuePair<string, object>> failedGoal);

    void PlanFound(List<KeyValuePair<string, object>> goal, List<ActionGoap> actions);

    void ActionsFinished();

    void PlanAborted(ActionGoap aborter);

    bool MoveAgent(ActionGoap nextAction);

    void SetDefaultActions();
}
