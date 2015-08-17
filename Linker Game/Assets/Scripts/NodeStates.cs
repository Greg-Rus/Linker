using UnityEngine;
using System.Collections;

public class Idle: FSMState
{
	public Idle(NodeController controller)
	{
		stateID = StateID.Idle;
		nodeController = controller;
	}
	
	public override void Reason()
	{
		if(nodeController.processLinkReq)
		{
			nodeController.nodeFSM.PerformTransition(Transition.processLinkList);
		}
	}
	
	public override void Act ()
	{
	
	}
}

