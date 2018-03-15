using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface InterfaceGoap
{
    /**
	 * The starting state of the Agent and the world.
	 * Supply what states are needed for actions to run.
	 */
    List<KeyValuePair<string, object>> GetWorldState();

    /**
	 * Give the planner a new goal so it can figure out 
	 * the actions needed to fulfill it.
	 */
    List<KeyValuePair<string, object>> CreateGoalState();

    /**
	 * No sequence of actions could be found for the supplied goal.
	 * You will need to try another goal
	 */
    void PlanFailed(List<KeyValuePair<string, object>> failedGoal);

    /**
	 * A plan was found for the supplied goal.
	 * These are the actions the Agent will perform, in order.
	 */
    void PlanFound(List<KeyValuePair<string, object>> goal, List<ActionGoap> actions);

    /**
	 * All actions are complete and the goal was reached. Hooray!
	 */
    void ActionsFinished();

    /**
	 * One of the actions caused the plan to abort.
	 * That action is returned.
	 */
    void PlanAborted(ActionGoap aborter);

    /**
	 * Called during Update. Move the agent towards the target in order
	 * for the next action to be able to perform.
	 * Return true if the Agent is at the target and the next action can perform.
	 * False if it is not there yet.
	 */
    bool MoveAgent(ActionGoap nextAction);
}
