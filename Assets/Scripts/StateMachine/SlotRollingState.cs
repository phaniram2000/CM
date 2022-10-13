using System.Collections;
using System.Collections.Generic;
using StateMachine;
using UnityEngine;

public class SlotRollingState : InputStateBase
{
	private SlotThief _slotThief;

	private float _interval = 0.5f;
	private float _lockPickSoundTimeDiff = 0.25f;


	public SlotRollingState(SlotThief slotThief)
	{
		_slotThief = slotThief;
	}
	
	public override void OnEnter()
	{
		//Play the Copying anim
		_lockPickSoundTimeDiff = 0.25f;

	}
	

	public override void Execute()
	{
		if (InputExtensions.GetFingerHeld())
		{
			var xVal = InputExtensions.GetInputDelta().x;
			var yVal = InputExtensions.GetInputDelta().y;

			if (xVal != 0f)
			{
				_slotThief.Rotate(xVal);
			}

			if (yVal != 0f)
			{
				_slotThief.Rotate(-yVal);
			}
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
		_slotThief.CheckForCode();
	}
}
