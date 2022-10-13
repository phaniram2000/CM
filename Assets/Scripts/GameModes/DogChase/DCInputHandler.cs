using DG.Tweening;
using StateMachine;
using UnityEngine;

public class DCInputHandler : AInputHandler
{
    
    private static readonly DCTapState  DcTapState= new DCTapState();

    private Transform girlTransform;
    public static DCGirlController DcGirlController;

    private bool hasTappedToPlay;

    private const string GirlTag = "Player";

    protected override void OnEnable()
    {
        base.OnEnable();
        GameEvents.TapToPlay += OnTapToPlay;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        GameEvents.TapToPlay -= OnTapToPlay;
    }

    protected override void InitialiseDerivedState()
    {
        girlTransform = GameObject.FindWithTag(GirlTag).transform;

        if (!girlTransform) return;

        if (!girlTransform.TryGetComponent(out DCGirlController girlController)) return;

        DcGirlController = girlController;

    }

    protected override InputStateBase HandleInput()
    {

        if (!hasTappedToPlay) return CurrentInputState;
       
        if (InputExtensions.GetFingerUp())
            return DcTapState;
        
        return CurrentInputState;
    }
    
    private void OnTapToPlay()
    {
         DOVirtual.DelayedCall(0.5f,()=>hasTappedToPlay = true);
    }
}
