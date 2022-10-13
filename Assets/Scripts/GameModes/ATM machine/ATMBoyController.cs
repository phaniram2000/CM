using System;
using DG.Tweening;
using UnityEngine;

public class ATMBoyController : MonoBehaviour
{
     private Animator _anim;

     [SerializeField] private GameObject cardHolder;
     [SerializeField] private float walkDuration;

     private Transform walkEndPoint;

     private const string EndPointName = "BoyWalkEndPoint";
     
     private static readonly int Idle = Animator.StringToHash("idle");
     private static readonly int Typing = Animator.StringToHash("typing");
     private static readonly int InPocket = Animator.StringToHash("inpocket");
     private static readonly int StealFail = Animator.StringToHash("stealfail");
     private static readonly int Walk = Animator.StringToHash("walk");
     private static readonly int WalkWithMoney = Animator.StringToHash("walkwithmoney");
     

     private void OnEnable()
     {
          GameEvents.TapToPlay += OnTapToPlay;
          ATMEvents.SwitchToDebitInCam += OnSwitchToDebitInCam;
          ATMEvents.StealFail += OnStealFail;
          ATMEvents.StealSuccess += OnStealSuccess;
          ATMEvents.DisableBoysPocketAtmCard += DisableCardHolderInPocket;

     }

     private void OnDisable()
     {
          GameEvents.TapToPlay -= OnTapToPlay;
          ATMEvents.SwitchToDebitInCam -= OnSwitchToDebitInCam;
          ATMEvents.StealFail -= OnStealFail;
          ATMEvents.StealSuccess -= OnStealSuccess;
          ATMEvents.DisableBoysPocketAtmCard -= DisableCardHolderInPocket;
        
     }

     private void Start()
     {
          _anim = GetComponent<Animator>();
          DisableCardHolderInPocket();
          walkEndPoint = GameObject.Find(EndPointName).transform;

     }

     private void StartTyping()
     {
          _anim.SetTrigger(Typing);
          
          AudioManager.instance.Play("Type");
     }
     
     private void OnTapToPlay()
     {
          DOVirtual.DelayedCall(0.8f, () => StartTyping());
     }
     
     private void OnSwitchToDebitInCam()
     {
          DOVirtual.DelayedCall(0.6f, () => _anim.SetTrigger(InPocket));

     }


     private void EnableCardHolderInPocket()
     {
          cardHolder.SetActive(true);
     }

     private void DisableCardHolderInPocket()
     {
          cardHolder.SetActive(false);
     }

     public void OnAnimationActivateCardHolder()
     {
          EnableCardHolderInPocket();
     }
     
     private void OnStealFail()
     {
          _anim.SetTrigger(StealFail);
     }

     public void MakeBoyWalkOnFail()
     {
          _anim.SetTrigger(Walk);
          BoyWalkAnimation();
          GameEvents.InvokeGameLose(-1);
     }
     
     public void MakeBoyWalkOnSuccess()
     {
          _anim.SetTrigger(WalkWithMoney);
          BoyWalkAnimation();
     }
     
     

     private void BoyWalkAnimation()
     {
         
          transform.DOLookAt(walkEndPoint.position, 0.2f).SetEase(Ease.Linear).OnComplete(() =>
          {
               transform.DOMove(walkEndPoint.position, walkDuration).SetEase(Ease.Linear);
          });
     }
     
     private void OnStealSuccess()
     {
          DOVirtual.DelayedCall(2.5f, () =>
          {
               MakeBoyWalkOnSuccess();
               DOVirtual.DelayedCall(0.4f, () => ATMEvents.InvokeOnMakeGirlMoveTowardsAtm());
          });
     }


}
