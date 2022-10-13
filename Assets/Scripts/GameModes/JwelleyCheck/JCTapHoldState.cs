using StateMachine;


public class JCTapHoldState : InputStateBase
{
    public override void OnEnter()
    {
        base.OnEnter();
        JCInputHandler.JcMeterController.RotateTo();
        
    }

    public override void Execute()
    {
        //do whatever you want to do here
        
        if (InputExtensions.GetFingerUp())
        {
            //reset meter rot
            JCInputHandler.JcMeterController.RotateBack();
            AInputHandler.AssignNewState(InputState.Idle);
        }


    }

    public override void OnExit()
    {
        base.OnExit();
        
    }
}
