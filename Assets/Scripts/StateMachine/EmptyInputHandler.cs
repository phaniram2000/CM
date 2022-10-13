using StateMachine;

public class EmptyInputHandler : AInputHandler
{
	protected override void InitialiseDerivedState() { }

	protected override InputStateBase HandleInput() => CurrentInputState;
}