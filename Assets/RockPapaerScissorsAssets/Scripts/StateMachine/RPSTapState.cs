using StateMachine;
using InputState = StateMachine.InputState;
using InputStateBase = StateMachine.InputStateBase;

public class RPSTapState : InputStateBase
	{
		public override void OnEnter()
		{
			base.OnEnter();
		}

		public override void Execute()
		{
			//do whaterever you want to do here

			RPSGameEvents.InvokeOnPlayerStartGiveSlap();
			AInputHandler.AssignNewState(InputState.Idle);
		}

		public override void OnExit()
		{
			base.OnExit();
			Vibration.Vibrate(30);
		}
	}


