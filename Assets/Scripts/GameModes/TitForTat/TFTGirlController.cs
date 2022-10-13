using System;
using DG.Tweening;
using Dreamteck.Splines;
using UnityEngine;

public class TFTGirlController : MonoBehaviour
{
    [SerializeField] private GameObject girlThinkingCanvas,emojiGameObject,laughGameObject,cryingGameObject,angryGameObject;
    [SerializeField] private bool IsSecondCharacter;
    private SplineFollower _splineFollower;
    private SplineComputer _splineComputer;
    private Animator _animator;
    private TFTSplineTrigerHelper _splineTrigerHelper;
    
    
    private static readonly int Idle = Animator.StringToHash("idle");
    private static readonly int Walk = Animator.StringToHash("walk");
    private static readonly int Fall = Animator.StringToHash("fall");
    private static readonly int Think = Animator.StringToHash("think");
    private static readonly int Dance = Animator.StringToHash("dance");

    private void OnEnable()
    {
        GameEvents.TapToPlay += OnTapToPlay;
        TFTGameEvents.LiftOpenDoorsDone += OnLiftDoorsOpenDone;
        TFTGameEvents.DoneButtonPressed += OnDoneButtonPressed;
        GameEvents.GameWin += OnGameWin;
    }

    private void OnDisable()
    {
        GameEvents.TapToPlay -= OnTapToPlay;
        TFTGameEvents.LiftOpenDoorsDone -= OnLiftDoorsOpenDone;
        TFTGameEvents.DoneButtonPressed -= OnDoneButtonPressed;
        GameEvents.GameWin -= OnGameWin;
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
        
        _splineTrigerHelper.AssignGrilController(this);

        if (!girlThinkingCanvas) return;
        
        girlThinkingCanvas.SetActive(false);

    }
    
    public void SplineFollowStatus(bool status) => _splineFollower.follow = status;
    
    private void OnTapToPlay()
    {
        print("tap to play");
        _animator.SetTrigger(Walk);
        SplineFollowStatus(true);
       
    }

    public void OnTiggerReached()
    {
        print("girl Trigger");
        SplineFollowStatus(false);
        _animator.SetTrigger(Fall);

        DOVirtual.DelayedCall(0.3f, () =>
        {
            if(AudioManager.instance)
                AudioManager.instance.Play("Falling");

            DOVirtual.DelayedCall(5f, () =>
            {
                if (AudioManager.instance)
                    AudioManager.instance.Play("GirlCrying");
            });
        });
        
        
        DOVirtual.DelayedCall(4f, () =>
        {
            cryingGameObject.SetActive(true);
            DOVirtual.DelayedCall(6f, () => cryingGameObject.SetActive(false));
        });
    }

    public void GirlThinking()
    {
        _animator.SetTrigger(Think);
        
        if(!girlThinkingCanvas) return;
        
        girlThinkingCanvas.SetActive(true);

        DOVirtual.DelayedCall(5, () =>
        {
            TFTGameEvents.InvokeOnShowGameplayScreen();
            girlThinkingCanvas.SetActive(false);
        });

    }
    
    private void OnLiftDoorsOpenDone(float obj)
    {
        DOVirtual.DelayedCall(obj + 0.6f, () => GirlThinking());
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
       
    }
    
    private void OnGameWin()
    {
        _animator.SetTrigger(Dance);
        
        
        if(!laughGameObject) return;
        laughGameObject.SetActive(true);
        
        if(AudioManager.instance)
            AudioManager.instance.Play("GirlLaugh");

        DOVirtual.DelayedCall(3, () => laughGameObject.SetActive(false));
    }

}
