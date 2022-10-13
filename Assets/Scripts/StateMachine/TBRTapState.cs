using StateMachine;


public class TBRTapState : InputStateBase
{
    public override void OnEnter()
    {
        base.OnEnter();
    }

    public override void Execute()
    {
        //do whatever you want to do here

       //invoke event for lock door.
       TBREvents.InvokeOnTapToLock();
       AInputHandler.AssignNewState(InputState.Idle);
    }

    public override void OnExit()
    {
        base.OnExit();
        Vibration.Vibrate(30);
    }
    
}
