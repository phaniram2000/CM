using System;
using DG.Tweening;
using UnityEngine;

public class ATMGirlController : MonoBehaviour
{
    private Animator _anim;

    private static readonly int Idle = Animator.StringToHash("idle");
    private static readonly int Peak = Animator.StringToHash("peak");
    private static readonly int Snatch = Animator.StringToHash("snatch");
    private static readonly int Fall = Animator.StringToHash("fall");
    private static readonly int StealFail = Animator.StringToHash("stealfail");
    private static readonly int Walk = Animator.StringToHash("walk");
    private static readonly int Win = Animator.StringToHash("win");

    private const string EndPointName = "GirlWalkEndPoint";

    private Transform girlWalkEndPoint;
    [SerializeField] private float walkDuration;
    [SerializeField] private GameObject cardHolder;

    private void Start()
    {
        _anim = GetComponent<Animator>();
        girlWalkEndPoint = GameObject.Find(EndPointName).transform;
        DisableCardHolder();
    }


    private void OnEnable()
    {
        ATMEvents.GirlCanPeak += OnGirlCanPeak;
        ATMEvents.SwitchToDebitInCam += OnSwitchToDebitInCam;
        ATMEvents.StealFail += OnStealFail;
        ATMEvents.GirlFall += OnGirlFall;
        ATMEvents.StealSuccess += OnStealSuccess;
        ATMEvents.MakeGirlMoveTowardsATM += OnMakeGirlWalkTowardsATM;
        ATMEvents.WithDrawlButtonPressed += OnWithDrawl;
        ATMEvents.DisableBoysPocketAtmCard += EnableCardHolder;
    }

    private void OnDisable()
    {
        ATMEvents.GirlCanPeak -= OnGirlCanPeak;
        ATMEvents.SwitchToDebitInCam -= OnSwitchToDebitInCam;
        ATMEvents.StealFail -= OnStealFail;
        ATMEvents.GirlFall -= OnGirlFall;
        ATMEvents.StealSuccess -= OnStealSuccess;
        ATMEvents.MakeGirlMoveTowardsATM -= OnMakeGirlWalkTowardsATM;
        ATMEvents.WithDrawlButtonPressed -= OnWithDrawl;
        ATMEvents.DisableBoysPocketAtmCard -= EnableCardHolder;
    }

    
    private void OnGirlCanPeak()
    {
        _anim.SetTrigger(Peak);
    }
    
    private void OnSwitchToDebitInCam()
    {
       _anim.SetTrigger(Idle);
    }
    
    private void OnStealFail()
    {
       _anim.SetTrigger(StealFail);
    }
    
    private void OnGirlFall()
    {
        _anim.SetTrigger(Fall);
        
    }
    
    private void OnStealSuccess()
    {
        _anim.SetTrigger(Snatch);
    }
    
    private void GirlWalkAnimation()
    {
        transform.DOLookAt(girlWalkEndPoint.position, 0.1f).SetEase(Ease.Linear).OnComplete(() =>
        {
            transform.DOMove(girlWalkEndPoint.position, walkDuration).SetEase(Ease.Linear).OnComplete(()=>
            {
                _anim.SetTrigger(Idle);
                ATMEvents.InvokeOnEnterAtmPinGamePlay();
            });
        });
    }
    
    private void OnMakeGirlWalkTowardsATM()
    {
        _anim.SetTrigger(Walk);
        GirlWalkAnimation();
    }
    
    private void OnWithDrawl()
    { 
        DisableCardHolder();
       DOVirtual.DelayedCall(0.5f,()=> _anim.SetTrigger(Win));
    }
    
    private void EnableCardHolder()
    {
        cardHolder.SetActive(true);
        
        AudioManager.instance.Play("Snatch");
    }

    private void DisableCardHolder()
    {
        cardHolder.SetActive(false);
    }
}
