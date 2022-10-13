using System;
using DG.Tweening;
using UnityEngine;

public class InterviewBossController : MonoBehaviour
{

    private Animator _anim;
    
    
    [SerializeField] private float desiredYRotateValue,rotateDuration;

    private static readonly int Welcome = Animator.StringToHash("welcome");
    private static readonly int Talk = Animator.StringToHash("talk");
    private static readonly int Give = Animator.StringToHash("give");

    public Animator Anim => _anim;


    private void OnEnable()
    {
        GameEvents.TapToPlay += OnTapToPlay;
    }

    private void OnDisable()
    {
        GameEvents.TapToPlay -= OnTapToPlay;
    }


    private void Start()
    {
        _anim = GetComponent<Animator>();
    }


    private void RotateBoss()
    {
        print("In rotate boss");
        var rotation = Quaternion.Euler(0, desiredYRotateValue, 0);

        transform.DORotateQuaternion(rotation, rotateDuration).SetEase(Ease.OutBack).OnComplete(() =>
        {
            print("In rotate boss done");
            InterviewEvents.InvokeOnBossRotationDone();
        });
    }
    
    private void OnTapToPlay()
    {
        RotateBoss();
    }

    public void SetWelcomeTrigger()
    {
        _anim.SetTrigger(Welcome);
    }

    public void SetTalkingTrigger()
    {
        _anim.SetTrigger(Talk);
    }

    public void SetGiveTrigger()
    {
        _anim.SetTrigger(Give);
        _anim.ResetTrigger(Talk);
    }
}
