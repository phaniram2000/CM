using StateMachine;

public class StandingIdle : IdleState
{
	private readonly EatingPerson _eatingPerson;
	private readonly EatingFoodInputHandler _input;

	public StandingIdle(EatingPerson eatingPerson)
	{
		_eatingPerson = eatingPerson;
	}
	
	public override void OnEnter()
	{
		_eatingPerson.StopEating();
	}

	public override void Execute()
	{
		_eatingPerson.DecreaseFillAmount();
		_eatingPerson.MakeSkinPale();

		if (InputExtensions.GetFingerHeld())
		{
			ExitState();
			return;
		}
		
		if(InputExtensions.GetFingerUp())
		{
		}
	}

	public override void OnExit()
	{
		
	}
}
