using DG.Tweening;
using UnityEngine;

public class ATMStatefullCameraController : MonoBehaviour
{
    private Animator _anim;
    private ATMGameController atmGameController;
    	
    	
    private static readonly int AtmZoom = Animator.StringToHash("atmzoom");
    private static readonly int Snatch = Animator.StringToHash("snatch");
    private static readonly int Gameover = Animator.StringToHash("gameover");
    private static readonly int DebitIn = Animator.StringToHash("debitin");
    private static readonly int StealSuccess = Animator.StringToHash("stealsuccess");
    
    
    
    private void OnEnable()
    {
        ATMEvents.GirlCanPeak += OnGirlCanPeak;
        ATMEvents.SwitchToDebitInCam += OnSwitchToDebitInCam;
        ATMEvents.SwitchToStealCam += OnSwitchToStealCam;
        ATMEvents.StealFail += OnStealFail;
        ATMEvents.EnterAtmPinGamePlay += OnEnterAtmPinGamePlay;
        ATMEvents.WithDrawlButtonPressed += OnWithDrawlButtonPressed;
    }
    
    private void OnDisable()
    {
        ATMEvents.GirlCanPeak -= OnGirlCanPeak;
        ATMEvents.SwitchToDebitInCam -= OnSwitchToDebitInCam;
        ATMEvents.SwitchToStealCam -= OnSwitchToStealCam;
        ATMEvents.StealFail -= OnStealFail;
        ATMEvents.EnterAtmPinGamePlay -= OnEnterAtmPinGamePlay;
        ATMEvents.WithDrawlButtonPressed -= OnWithDrawlButtonPressed;
    }

    private void Start()
    {
        _anim = GetComponent<Animator>();
        var controller = GameObject.FindWithTag("GameController");

        if (!controller) return;

        if (!controller.TryGetComponent(out ATMGameController atmController)) return;

        atmGameController = atmController;
    }
    
    private void OnGirlCanPeak()
    {
        
        AudioManager.instance.Pause("Type");
        DOVirtual.DelayedCall(1.2f, ()=>_anim.SetTrigger(AtmZoom)).OnComplete(() =>
        {
            
            if(atmGameController)
                DOVirtual.DelayedCall(atmGameController.UserPinSeeTime, () => ATMEvents.InvokeOnSwitchToDebitInCam());
            else
                DOVirtual.DelayedCall(3f, () => ATMEvents.InvokeOnSwitchToDebitInCam());
        });
    }
    
    
    private void OnSwitchToDebitInCam()
    {
        _anim.SetTrigger(DebitIn);
    }
    
    
    private void OnSwitchToStealCam()
    {
        _anim.SetTrigger(Snatch);
        
    }
    
    private void OnStealFail()
    {
        _anim.SetTrigger(Gameover);
    }
    
    private void OnEnterAtmPinGamePlay()
    {
        _anim.SetTrigger(StealSuccess);
    }
    
    private void OnWithDrawlButtonPressed()
    {
        _anim.SetTrigger(Gameover);
    }
    
    
}
