using StateMachine;

public class RPSInputHandler : AInputHandler
{
    private static readonly RPSTapState RpsTapState = new RPSTapState();

    private bool _shouldEnterTapState;
    
    
    protected override void OnEnable()
    {
        base.OnEnable();
        
        RPSGameEvents.AllowPlayerToSlap += EnableTapState;
        RPSGameEvents.GameLose += OnGameLose;
        RPSGameEvents.GameWin += OnGameWin;
        RPSGameEvents.PowerSlapGiven += OnPowerSlapGiven;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        
        RPSGameEvents.AllowPlayerToSlap -= EnableTapState;
        RPSGameEvents.GameLose -= OnGameLose;
        RPSGameEvents.GameWin -= OnGameWin;
        RPSGameEvents.PowerSlapGiven -= OnPowerSlapGiven;
    }
   

    protected override void InitialiseDerivedState()
    {
        
    }

    protected override StateMachine.InputStateBase HandleInput()
    {
        if (!_shouldEnterTapState) return CurrentInputState;


        if (InputExtensions.GetFingerUp())
        {
            _shouldEnterTapState = false;
            return RpsTapState;
        }

        return CurrentInputState;
    }
    
    public void EnableTapState() => _shouldEnterTapState = true;

    private void OnGameWin()
    {
        AssignNewState(InputState.Disabled);
    }

    private void OnGameLose()
    {
        AssignNewState(InputState.Disabled);
    }

    private void OnPowerSlapGiven()
    {
        AssignNewState(InputState.Disabled);
    }
}
