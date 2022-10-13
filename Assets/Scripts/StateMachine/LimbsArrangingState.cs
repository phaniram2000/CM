using System.Collections;
using System.Collections.Generic;
using StateMachine;
using UnityEngine;

public class LimbsArrangingState : InputStateBase
{
	private readonly SlidingDownBurglar _burglar;
	
	public LimbsArrangingState(SlidingDownBurglar burglar)
	{
		_burglar = burglar;
	}
	
	public override void OnEnter()
	{
		//Play the Copying anim
		_burglar.StartDragging();
		Vibration.Vibrate(20);
	}
	

	public override void Execute()
	{
		//if(IsExitingCurrentState) return;
		if (InputExtensions.GetFingerHeld())
		{
			_burglar.MoveTheSelectedLimb();
		}
		
		if(InputExtensions.GetFingerUp())
		{
			//_cheatingStudent.PlayCoveringAnim();
			ExitState();
			return;
		}
	}

	public override void OnExit()
	{
		//. . . 
		_burglar.ResetHitObj();
		Vibration.Vibrate(20);
	}
}
