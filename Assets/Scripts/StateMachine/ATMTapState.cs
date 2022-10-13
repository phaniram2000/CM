using StateMachine;


public class ATMTapState : InputStateBase
{
    public override void OnEnter()
    {
        base.OnEnter();
    }

    public override void Execute()
    {
        //do whatever you want to do here

        ATMEvents.InvokeOnPlayerAtemptToSteal();
        AInputHandler.AssignNewState(InputState.Idle);
    }

    public override void OnExit()
    {
        base.OnExit();
        Vibration.Vibrate(30);
    }
}
