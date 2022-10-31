using StateMachine;

public class ATMInputHandler : AInputHandler
{

    private static readonly ATMTapState AtmTapState = new ATMTapState();

    private bool _shouldEnterTapState;

    protected override void OnEnable()
    {
        base.OnEnable();
        ATMEvents.AllowPlayerToSteal += OnAllowPlayerToSteal;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        ATMEvents.AllowPlayerToSteal -= OnAllowPlayerToSteal;
    }

    
    protected override void InitialiseDerivedState()
    {
       
    }

    protected override InputStateBase HandleInput()
    {
        if (!_shouldEnterTapState) return CurrentInputState;


        if (InputExtensions.GetFingerUp())
        {
            _shouldEnterTapState = false;
            return AtmTapState;
        }

        return CurrentInputState;
    }
    
    private void OnAllowPlayerToSteal()
    {
        _shouldEnterTapState = true;
    }
    
    
}
