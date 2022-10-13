using DG.Tweening;
using Dreamteck.Splines;
using UnityEngine;

public class TFTBoyController : MonoBehaviour
{
    [SerializeField] private GameObject laughGameObject, cryingGameObject, angryGameObject;
    [SerializeField] private bool IsSecondCharacter;
    
    private SplineFollower _splineFollower;
    private SplineComputer _splineComputer;
    private Animator _animator;
    private TFTSplineTrigerHelper _splineTrigerHelper;
    
    private static readonly int Idle = Animator.StringToHash("idle");
    private static readonly int Walk = Animator.StringToHash("walk");
    private static readonly int MakeFall = Animator.StringToHash("makefall");
    private static readonly int Angry = Animator.StringToHash("angry");

    private void OnEnable()
    {
        GameEvents.TapToPlay += OnTapToPlay;
        TFTGameEvents.DoneButtonPressed += OnDoneButtonPressed;
    }

    private void OnDisable()
    {
        GameEvents.TapToPlay -= OnTapToPlay;
        TFTGameEvents.DoneButtonPressed -= OnDoneButtonPressed;
        
    }

    
    private void Start()
    {
        _animator = GetComponent<Animator>();
        _splineFollower = GetComponent<SplineFollower>();
        _splineComputer = _splineFollower.spline;
        
        
        SplineFollowStatus(false);

        if (!_splineComputer) return;

        if (!_splineComputer.transform.TryGetComponent(out TFTSplineTrigerHelper trigerHelper)) return;

        _splineTrigerHelper = trigerHelper;
        
        _splineTrigerHelper.AssignBoyController(this);
        
        
    }
    
    public void SplineFollowStatus(bool status) => _splineFollower.follow = status;
    
    private void OnTapToPlay()
    {
        print("tap to play");
        SetTriggerWalk();
       
    }

    public void OnTiggerReached()
    {
        print("boyTrigger");
        SplineFollowStatus(false);
        _animator.SetTrigger(MakeFall);
        
        
        
        DOVirtual.DelayedCall(3f, () =>
        {
            
            if(AudioManager.instance)
                AudioManager.instance.Play("BoyLaugh");
            laughGameObject.SetActive(true);
            DOVirtual.DelayedCall(6f, () => laughGameObject.SetActive(false));
        });
    }

    public void SetTriggerWalk()
    {
        _animator.SetTrigger(Walk);
        SplineFollowStatus(true);
    }
    
    private void OnDoneButtonPressed()
    {
        DOVirtual.DelayedCall(0.5f, () => MakeSecondCharacterWalk());
    }

    private void MakeSecondCharacterWalk()
    {
        if (!IsSecondCharacter) return;
        
        _animator.SetTrigger(Walk);
        SplineFollowStatus(true);
    }

    public void OnSecondCharacterTriggerReached()
    {
        if (!IsSecondCharacter) return;
        
        _animator.SetTrigger(Idle);
        SplineFollowStatus(false);

        DOVirtual.DelayedCall(0.2f, () => BoyAngryAnimation());

    }

    private void BoyAngryAnimation()
    {
        _animator.SetTrigger(Angry);

        DOVirtual.DelayedCall(2.7f, () =>
        {
            angryGameObject.SetActive(true);
            if(AudioManager.instance)
                AudioManager.instance.Play("BoyAngry");

            DOVirtual.DelayedCall(5.5f, () => angryGameObject.SetActive(false));
        });
        
        
        
        DOVirtual.DelayedCall(4.7f, () =>
        {
            TFTGameEvents.InvokeOnCloseLiftDoors();
            GameEvents.InvokeGameWin();
        });
    }
}
