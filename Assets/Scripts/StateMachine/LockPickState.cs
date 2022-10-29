using StateMachine;
using UnityEngine;

public class LockPickState : InputStateBase
{
	private readonly LockerPicking _lockerPicking;

	private float _lockPickSoundTimeDiff = 0.25f;
	
	public LockPickState(LockerPicking lockerPicking)
	{
		_lockerPicking = lockerPicking;
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
				 _lockerPicking.Rotate(xVal);
			 }

			 if (yVal != 0f)
			 {
				 _lockerPicking.Rotate(-yVal);
			 }
			 if (_lockPickSoundTimeDiff <= 0f)
			 {
				 if (AudioManager.instance)
					 AudioManager.instance.Play("LockPick");

				 _lockPickSoundTimeDiff = 0.25f;
				 Vibration.Vibrate(10);
			 }

			 _lockPickSoundTimeDiff -= Time.deltaTime;
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
		_lockerPicking.CheckForKeyCode();

		_lockPickSoundTimeDiff = 0.25f;
		
		if (AudioManager.instance)
			AudioManager.instance.Pause("LockPick");	
	}
}
