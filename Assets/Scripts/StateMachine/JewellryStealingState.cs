using StateMachine;

public class JewellryStealingState : InputStateBase
{
	private readonly JewelleryThief _jewelleryThief;
	
	public JewellryStealingState(JewelleryThief jewelleryThief)
	{
		_jewelleryThief = jewelleryThief;
	}
	
	public override void OnEnter()
	{
		//Play the Copying anim
		_jewelleryThief.ResetInterval();
		_jewelleryThief.SlideDownAndSteal();
		_jewelleryThief.ResetLaughTimer();
	}
	

	public override void Execute()
	{
		//if(IsExitingCurrentState) return;
		if (InputExtensions.GetFingerHeld())
		{
			_jewelleryThief.Steal();
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
		_jewelleryThief.StopStealing();
		if(!_jewelleryThief.isCaught)
			_jewelleryThief.SlideUpAndHide();
		_jewelleryThief.ResetInterval();
		_jewelleryThief.ResetLaughTimer();
	}
}
