using DG.Tweening;
using UnityEngine;

public class InterviewStatefullCamController : MonoBehaviour
{
    private Animator _anim;

    private static readonly int Offer = Animator.StringToHash("offer"); 
    private static readonly int Sign = Animator.StringToHash("sign"); 
    private static readonly int Scene = Animator.StringToHash("scene"); 
    

    private void OnEnable()
    {
        InterviewEvents.SwitchToOfferCam += OnSwitchToOfferCam;
        InterviewEvents.DoneButtonPressed += OnDoneButtonPressed;
    }

    private void OnDisable()
    {
        InterviewEvents.SwitchToOfferCam -= OnSwitchToOfferCam;
        InterviewEvents.DoneButtonPressed -= OnDoneButtonPressed;
    }

    
    private void Start()
    {
        _anim = GetComponent<Animator>();
    }
    
    
    private void OnSwitchToOfferCam()
    {
        _anim.SetTrigger(Offer);

        DOVirtual.DelayedCall(2, () => SwitchToSignCam());
    }

    private void SwitchToSignCam()
    {
        _anim.SetTrigger(Sign);
    }

    private void OnDoneButtonPressed()
    {
        _anim.SetTrigger(Scene);
    }


}
