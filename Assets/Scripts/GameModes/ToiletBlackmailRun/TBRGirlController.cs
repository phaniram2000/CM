using System;
using DG.Tweening;
using UnityEngine;

public class TBRGirlController : MonoBehaviour
{

    [SerializeField] private Transform itemsHolder;
    
    private Animator _anim;
    private Transform girlWalkEndPoint,girlWalkBackwardEndPoint,girlRunEndPoint;


    private const string StartingWalkEndPoint = "GirlWalkEndPoint";
    private const string BackwardWalkEndPoint = "GirlBackWalkEndPoint";
    private const string GirlRunEndPointName = "GirlRunEndPoint";
    
    private static readonly int Idle = Animator.StringToHash("idle");
    private static readonly int Walk = Animator.StringToHash("walk");
    private static readonly int OpenDoor = Animator.StringToHash("opendoor");
    private static readonly int CloseDoor = Animator.StringToHash("closedoor");
    private static readonly int LockDoor = Animator.StringToHash("lockdoor");
    private static readonly int BackWalk = Animator.StringToHash("backwalk");
    private static readonly int PickUp = Animator.StringToHash("pickup");
    private static readonly int LookAround = Animator.StringToHash("lookaround");
    private static readonly int Run = Animator.StringToHash("run");


    private GameObject currentItemPicked;
    
    
    [SerializeField] private float walkDuration;


    private void OnEnable()
    {
        GameEvents.TapToPlay += OnTapToPlay;
        TBREvents.BoyAngryDone += OnCloseDoor;
        TBREvents.OnTapToLock += OnTapToLock;
        TBREvents.ItemReadyToPick += OnItemReadyToPick;
        TBREvents.GirlPrankingDoneNowEscape += OnGirlPrankingDone;
    }

    private void OnDisable()
    {
        GameEvents.TapToPlay -= OnTapToPlay;
        TBREvents.BoyAngryDone -= OnCloseDoor;
        TBREvents.OnTapToLock -= OnTapToLock;
        TBREvents.ItemReadyToPick -= OnItemReadyToPick;
        TBREvents.GirlPrankingDoneNowEscape -= OnGirlPrankingDone;
    }

    private void Start()
    {
        _anim = GetComponent<Animator>();
        girlWalkEndPoint = GameObject.Find(StartingWalkEndPoint).transform;
        girlWalkBackwardEndPoint = GameObject.Find(BackwardWalkEndPoint).transform;
        girlRunEndPoint = GameObject.Find(GirlRunEndPointName).transform;

    }

    private void MakeGirlWalk()
    {
        _anim.SetTrigger(Walk);
        GirlWalkAnimation();
    }

    private void GirlWalkAnimation()
    {
        
        if (AudioManager.instance)
             AudioManager.instance.Play("GirlWalk");
        
        transform.DOLookAt(girlWalkEndPoint.position, 0.1f).SetEase(Ease.Linear).OnComplete(() =>
        {
            transform.DOMove(girlWalkEndPoint.position, walkDuration).SetEase(Ease.Linear).OnComplete(()=>
            {
                _anim.SetTrigger(Idle);
                _anim.SetTrigger(OpenDoor);
                
                if (AudioManager.instance)
                    AudioManager.instance.Pause("GirlWalk");
                
            });
        });
    }


    private void OnCloseDoor()
    {
        _anim.SetTrigger(CloseDoor);
    }

    private void OnTapToPlay()
    {
        MakeGirlWalk();
    }
    
    private void OnTapToLock()
    {
         DOVirtual.DelayedCall(0.4f,()=>  _anim.SetTrigger(LockDoor));
    }

    public void MakeGirlWalkBackWard()
    {
        _anim.SetTrigger(BackWalk);
        GirlWalkBackAnimation();
    }
    
    private void GirlWalkBackAnimation()
    {
        transform.DOMove(girlWalkBackwardEndPoint.position, 0.6f).SetEase(Ease.Linear).OnComplete(()=>
            {
                _anim.SetTrigger(Idle);
                TBREvents.InvokeOnGirlDoorLockingDone();
                
            });
    }
    
    private void OnItemReadyToPick(GameObject obj)
    {
        currentItemPicked = obj;
        _anim.SetTrigger(PickUp);
    }

    public void OnItemPickedUpStart()
    {
        currentItemPicked.transform.parent = itemsHolder.transform;
        currentItemPicked.transform.localPosition = Vector3.zero;
        //currentItemPicked.transform.localScale = Vector3.one;
        currentItemPicked.transform.rotation = Quaternion.Euler(0,0,0);
    }

    public void OnItemPickedUpEnd()
    {
        currentItemPicked.SetActive(false);
        
        TBREvents.InvokeOnItemPickedUpByGirl();
    }

    
    private void OnGirlPrankingDone()
    {
        DOVirtual.DelayedCall(0.5f, () => MakeGirlRun());
    }

    private void MakeGirlRun()
    {
        _anim.SetTrigger(LookAround);
        DOVirtual.DelayedCall(3f, () =>
        {
            _anim.SetTrigger(Run);
            GirlRunAnimation();
        });
    }

    private void GirlRunAnimation()
    {
        
        if(AudioManager.instance)
            AudioManager.instance.Play("GirlWalk");
        
        transform.DOLookAt(girlRunEndPoint.position, 0.1f).SetEase(Ease.Linear).OnComplete(() =>
        {
            transform.DOMove(girlRunEndPoint.position, walkDuration).SetEase(Ease.Linear).OnComplete(()=>
            {
                if(AudioManager.instance)
                    AudioManager.instance.Pause("GirlWalk");
            });
        });
    }

}
