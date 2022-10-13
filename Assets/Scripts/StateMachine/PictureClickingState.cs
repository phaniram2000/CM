using DG.Tweening;
using StateMachine;

public class PictureClickingState : InputStateBase
{
	private readonly Blackmailer _blackmailer;
	private readonly BlackmailingInputHandler _input;
	private bool _toTakePicture;

	
	public PictureClickingState(Blackmailer blackmailer, BlackmailingInputHandler input)
	{
		_blackmailer = blackmailer;
		_input = input;
	}
	
	public override void OnEnter()
	{
		_input.ResetInterval();
		//Play the Copying anim
		BlackmailingEvents.InvokeStartTakingPictures();
		_toTakePicture = true;
		if (AudioManager.instance)
			AudioManager.instance.Play("Turn");
		Vibration.Vibrate(10);
	}

	public override void Execute()
	{
		if (InputExtensions.GetFingerHeld())
		{
			//Check of player touch 
			_input.StartInterval();
			if (_input.tempIntervalBetweenShots <= 0f)
			{
				BlackmailingEvents.InvokeTakePicture();
				_input.ResetInterval();
				_blackmailer.Flash();
				DOVirtual.DelayedCall(0.25f, _blackmailer.StopFlash);
				if (AudioManager.instance)
					AudioManager.instance.Play("Camera");
				Vibration.Vibrate(10);
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
		//if required amount of pics taken then move to blackmailState
		BlackmailingEvents.InvokeStopTakingPictures();
	}
}
