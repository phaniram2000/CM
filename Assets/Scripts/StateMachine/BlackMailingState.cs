using StateMachine;

public class BlackMailingState : InputStateBase
{
	private readonly Blackmailer _blackmailer;
	
	public BlackMailingState(Blackmailer blackmailer)
	{
		_blackmailer = blackmailer;
	}
	
	public override void OnEnter()
	{
		//Play the Copying anim
	}

	public override void Execute()
	{
		if (InputExtensions.GetFingerHeld())
		{
			//Check of player touch 
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
	}
}
