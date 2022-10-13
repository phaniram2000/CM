using StateMachine;


public class DCTapState : InputStateBase
{
    public override void OnEnter()
    {
        base.OnEnter();
       
    }

    public override void Execute()
    {
        base.Execute();
        DCInputHandler.DcGirlController.MakeGirlJump();
        AInputHandler.AssignNewState(InputState.Idle);
    }

    public override void OnExit()
    {
        base.OnExit();
    }
}
