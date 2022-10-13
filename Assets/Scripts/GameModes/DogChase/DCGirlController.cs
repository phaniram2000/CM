using System;
using DG.Tweening;
using StateMachine;
using UnityEngine;

public class DCGirlController : MonoBehaviour
{


    [SerializeField] private float runDuration,girlSpeedMultiplier;
    [SerializeField] private float playerJumpValue;
    private Transform girlEndPointTransform;

    private const string GirlEndPointName = "GirlRunEndPoint";
    
    private Animator _anim;

    
    private static readonly int Run = Animator.StringToHash("run");
    private static readonly int Jump = Animator.StringToHash("jump");

    private bool hastapped;

    private Rigidbody _rb;

    private Tween _runTween;

    private float _characterYValue;

    private bool _stopRunning;

    private Vector3 _targetVelocity;

    private bool _canJump;

    private void OnEnable()
    {
        GameEvents.TapToPlay += OnTapToPlay;
        DCEvents.GirlCollidedWithObstacle += OnGirlCollidedWithObs;
    }

    private void OnDisable()
    {
        GameEvents.TapToPlay -= OnTapToPlay;
        DCEvents.GirlCollidedWithObstacle -= OnGirlCollidedWithObs;
    }

    

    private void Start()
    {
        _anim = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody>();
        GetEndpointTransform();
        _stopRunning = false;
        _characterYValue = transform.position.y;
    }

    public void SetAnimatiorStatus(bool status) => _anim.enabled = status;

    private void GetEndpointTransform()
    {
       var tempGameObject = GameObject.Find(GirlEndPointName);

       if (!tempGameObject) return;

       girlEndPointTransform = tempGameObject.transform;
    }
    
    private void MakeGirlRun()
    { 
        _anim.SetTrigger(Run);
        _runTween = transform.DOMoveZ(girlEndPointTransform.position.z, runDuration).SetEase(Ease.Linear);


    }
    
    
    private void OnTapToPlay()
    {
        hastapped = true;
        MakeGirlRun();
        
       _canJump = true;
       //transform.DOMoveZ(transform.position.z + 40f, runDuration).SetEase(Ease.Linear);
    }

    public void MakeGirlJump()
    { 
       
        if(!_canJump) return;

        _canJump = false;
        _anim.SetTrigger(Jump);
       transform.DOMoveY(playerJumpValue, 0.5f).SetLoops(2,LoopType.Yoyo).OnComplete(()=>
       {
           _canJump = true;
           _anim.SetTrigger(Run);
       });
    }
    
    private void OnGirlCollidedWithObs()
    {
        SetAnimatiorStatus(false);
        _runTween.Kill();
    }

}
