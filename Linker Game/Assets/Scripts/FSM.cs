using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum Transition{
	//For global state ProcessingLink
	processLinkList, continueLinkProcessing, forwardLinkList, waitForUploadInd, startDownloading, trimLinkList, 
	forwardTrimmedLinkList, waitForNextLinkListForward, onForwardLinkList, finishLinkProcessing

}

public enum StateID{
	Idle, ProcessingLink, SendingUploadInd, ForwardingLinkList, WaitingForUploadInd, Downloading, TrimmingLinkList, 
	WaitingForNextLinkListForward, Sourcing
}

//public class FSM : MonoBehaviour {}

public abstract class FSMState
{
	protected Dictionary<Transition, StateID> map = new Dictionary<Transition, StateID> ();
	protected StateID stateID;
	protected NodeController nodeController;
	public StateID ID { get { return stateID; } }

	public void AddTransition (Transition newTransition, StateID newId)
	{
		if (map.ContainsKey (newTransition)) 
		{
			Debug.LogError("Error: Transition " + newTransition + " already in map.");
			return;
		}
		map.Add (newTransition, newId);
		Debug.Log ("Added transition: " + newTransition);
	}

	public void DeleteTransition (Transition oldTransition)
	{
		if (map.ContainsKey (oldTransition)) 
		{
			map.Remove(oldTransition);
			return;
		}
		Debug.LogError("Error: Transition " + oldTransition + " not present in map.");
	}

	public StateID GetNextState(Transition trans)
	{
		return map[trans];
	}

	public abstract void Reason();

	public abstract void Act ();
	
	public virtual void DoBeforeEntering() { }
	
	public virtual void DoBeforeExiting() { }
}

public class FSMSystem
{
	private List<FSMState> states;

	private StateID currentStateID;
	public StateID CurrentStateID { get { return currentStateID; } }

	private FSMState currentState;
	public FSMState CurrentState {get {return currentState;}}

	public FSMSystem()
	{
		states = new List<FSMState> ();
	}

	public void AddState(FSMState newState)
	{
		if (states.Count == 0) {
			states.Add(newState);
			currentState = newState;
			currentStateID = newState.ID;
			return;
		}

		foreach (FSMState knownState in states)
		{
			if(knownState.ID == newState.ID)
			{
				Debug.LogError("Error: State "+ newState.ID + " already in FSM");
			}
		}
		states.Add(newState);
	}

	public void DeleteState(StateID id)
	{
		foreach (FSMState knowStaste in states) 
		{
			if(knowStaste.ID == id)
			{
				states.Remove(knowStaste);
				return;
			}
		}
		Debug.LogError ("No state with ID: " + id + " present on state list");
	}

	public void PerformTransition (Transition trans)
	{
		StateID id = currentState.GetNextState (trans);
		currentStateID = id;

		foreach (FSMState state in states) 
		{
			if(state.ID == currentStateID)
			{
				currentState.DoBeforeExiting();
				currentState  = state;
				currentState.DoBeforeEntering();
				break;
			}
		}
	}

}