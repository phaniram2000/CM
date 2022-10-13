using StateMachine;
using UnityEngine;

public class KissingState : InputStateBase
{
	private readonly KissingChildren _kissingChildren;

	private float _kissingInterval = 1f;
	
	public KissingState(KissingChildren student)
	{
		_kissingChildren = student;
		_kissingInterval = 1f;
	}
	
	public override void OnEnter()
	{
		//Play the Copying anim
		KissingEvents.InvokeStartKissing();
		
		Vibration.Vibrate(15);
	}

	public override void Execute()
	{
		if (InputExtensions.GetFingerHeld())
		{
			//Check of player touch 
			_kissingChildren.canvas.StartFillingTheHeartText();
			_kissingInterval -= Time.deltaTime;
			if (_kissingInterval <= 0)
			{
				if(AudioManager.instance)
					AudioManager.instance.Play("Kiss");
				_kissingInterval = 1f;
			}

		}
		
		if(InputExtensions.GetFingerUp())
		{
			//_cheatingStudent.PlayCoveringAnim();
			KissingEvents.InvokeStopKissing();
			ExitState();
			return;
		}
		
		
	}

	public override void OnExit()
	{
		//. . . 
		KissingEvents.InvokeStopKissing();
		
		Vibration.Vibrate(15);
	}
}
