using StateMachine;


public class TBRInputHandler : AInputHandler
{
    
    private static readonly TBRTapState TbrTapState = new TBRTapState();
    private static readonly TBRSwipeState SwipeState = new TBRSwipeState();

    private bool _shouldEnterTapState,_canSwipe;
    
    protected override void OnEnable()
    {
        base.OnEnable();
        TBREvents.GirlCloseDoorDone += OnAllowPlayerToLockDoors;

    }

    protected override void OnDisable()
    {
        base.OnDisable();
        TBREvents.GirlCloseDoorDone -= OnAllowPlayerToLockDoors;
        
    }
    
    protected override void InitialiseDerivedState()
    {
        
    }

    protected override InputStateBase HandleInput()
    {
        if (!_shouldEnterTapState) return CurrentInputState;

        //if (!_canSwipe) return CurrentInputState;

        /*if (InputExtensions.GetFingerHeld())
        {
            _shouldEnterTapState = false;
            return TbrTapState;
        }*/
        
        if (InputExtensions.GetInputDelta().x > 1)
        {
            _shouldEnterTapState = false;
            return TbrTapState;
        }



        return CurrentInputState;
    }

    private void OnAllowPlayerToLockDoors()
    {
        _shouldEnterTapState = true;
        _canSwipe = true;
    }
}
