using DG.Tweening;
using StateMachine;
using UnityEngine;
using UnityEngine.EventSystems;

public class JCInputHandler : AInputHandler
{
    
    private static readonly JCTapHoldState JcTapHoldState =  new JCTapHoldState();
    
    public static JCMeterController JcMeterController;

    private bool canCheck;
    
    protected override void OnEnable()
    {
        base.OnEnable();
        JCEvents.AllowToCheckJwellery += OnAllowToCheckJwellery;
        JCEvents.FakeSelected += DisableCanCheck;
        JCEvents.RealSelected += DisableCanCheck;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        JCEvents.AllowToCheckJwellery -= OnAllowToCheckJwellery;
        JCEvents.FakeSelected -= DisableCanCheck;
        JCEvents.RealSelected -= DisableCanCheck;
        
    }

    protected override void InitialiseDerivedState()
    {
        var meterGameObject = GameObject.FindWithTag("JCMeter");

        if (!meterGameObject) return;

        if (!meterGameObject.TryGetComponent(out JCMeterController meterController)) return;

        JcMeterController = meterController;
    }

    protected override InputStateBase HandleInput()
    {

        if (!canCheck) return CurrentInputState;
        
        if (InputExtensions.GetFingerHeld() && !EventSystem.current.IsPointerOverGameObject(InputExtensions.GetPointerId()))
            return JcTapHoldState;
       
        return CurrentInputState;
    }
    
    private void OnAllowToCheckJwellery()
    {
        DOVirtual.DelayedCall(0.1f, () => canCheck = true);
    }


    private void DisableCanCheck()
    {
        canCheck = false;
    }
}
