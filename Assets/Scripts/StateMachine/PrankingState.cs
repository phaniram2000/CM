using StateMachine;

public class PrankingState : InputStateBase
{
	private Prankster _prankster;
	private ShampooInputHandler _shampooInputHandler;

	public PrankingState(Prankster prankster, ShampooInputHandler shampooInputHandler)
	{
		_prankster = prankster;
		_shampooInputHandler = shampooInputHandler;
	}
	
	public override void OnEnter()
	{
		//Play the Copying anim
		ShowerPrankEvents.InvokeStartPrank();
	}

	public override void Execute()
	{
		//if(IsExitingCurrentState) return;
		
		if(InputExtensions.GetFingerUp())
		{
			ExitState();
			return;
		}
		
		if (InputExtensions.GetFingerHeld())
		{
			//Check of player touch
			_prankster.prankCanvas.FillTheImage();
		}
	}

	public override void OnExit()
	{
		//. . . 
		//if required amount of pics taken then move to blackmailState
		ShowerPrankEvents.InvokeEndPrank();
	}
}
