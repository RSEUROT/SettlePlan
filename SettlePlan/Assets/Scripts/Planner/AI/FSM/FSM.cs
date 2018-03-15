using System.Collections.Generic;
using UnityEngine;
using System.Collections;

/**
 * Stack-based Finite State Machine.
 * Push and pop states to the FSM.
 * 
 * States should push other states onto the stack 
 * and pop themselves off.
 */
using System;

public delegate void FSMState (FSM fsm, GameObject gameObject);

public class FSM
{
	private Stack<FSMState> stateStack = new Stack<FSMState> (); // pile d'action	

	public void Update (GameObject gameObject)
    {
		if (stateStack.Peek() != null)
			stateStack.Peek().Invoke (this, gameObject);
	}

	public void pushState(FSMState state)
    {
		stateStack.Push(state);
	}
	public void popState()
    {
		stateStack.Pop ();
	}
}
