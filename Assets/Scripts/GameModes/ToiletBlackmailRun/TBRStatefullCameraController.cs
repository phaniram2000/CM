using System;
using DG.Tweening;
using UnityEngine;

public class TBRStatefullCameraController : MonoBehaviour
{
    private Animator _anim;
    
    
    private static readonly int Idle = Animator.StringToHash("idle");
    private static readonly int LockDoor = Animator.StringToHash("lockdoor");


    private void OnEnable()
    {
        TBREvents.OnTapToLock += LockDoorCam;
    }

    private void OnDisable()
    {
        TBREvents.OnTapToLock -= LockDoorCam;
    }


    private void Start()
    {
        _anim = GetComponent<Animator>();
    }

    private void LockDoorCam()
    {
        _anim.SetTrigger(LockDoor);
        DOVirtual.DelayedCall(1.8f, () => _anim.SetTrigger(Idle));
    }
    
    
}
