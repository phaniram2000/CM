using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public enum StealNRun_PlayerAction
{
    None,
    RunToWards,
    PlayerControl,
    PlayerEnd, 
    PlayerCaught
}

public class StealNRun_PlayerController : SingletonInstance<StealNRun_PlayerController>
{
    public StealNRun_PlayerAction playerAction;
    
    [Range(0f, 8f)] public float zSpeed;
    [Range(0f, 8f)] public float xSpeed;
    public GameObject suiteCase;
    public Animator playerAnimator;
    public Vector3 targetPos, endPos;
    public GameObject helpCanvas;
    
    private static readonly int Rob = Animator.StringToHash("Rob");
    private static readonly int Run = Animator.StringToHash("Run");
    private static readonly int Fall = Animator.StringToHash("Fall");
    private static readonly int Jump = Animator.StringToHash("Jump");
    private static readonly int RunTowards = Animator.StringToHash("RunTowards");

    protected override void Awake()
    {
        base.Awake();
        playerAnimator = GetComponentInChildren<Animator>();
    }

    public void OnTapToPlay()
    {
        playerAction = StealNRun_PlayerAction.RunToWards;
        StealNRun_SuiteGuard.WalkWithBriefCase();
        RunTowardsGuard();
        AudioManager.instance.Play("Run");
        DOVirtual.DelayedCall(2f, ()=>helpCanvas.SetActive(false));
    }

    private void Update()
    {
        PlayerActionHandler();
    }

    private void PlayerActionHandler()
    {
        switch (playerAction)
        {
            case StealNRun_PlayerAction.None:
                break;
            case StealNRun_PlayerAction.RunToWards:
                break;
            case StealNRun_PlayerAction.PlayerControl:
                PlayerMovement();
                break;
            case StealNRun_PlayerAction.PlayerEnd:
                break;
            case StealNRun_PlayerAction.PlayerCaught:
                break;
            default:
                break;
        }
    }

    private void RunTowardsGuard()
    {
        playerAnimator.SetTrigger(RunTowards);
        transform.DOMove(targetPos, 0.65f).SetEase(Ease.Flash).OnComplete(() =>
        {
            StealNRun_SuiteGuard.GetRobbedNow();
            playerAnimator.SetTrigger(Rob);
            DOVirtual.DelayedCall(0.15f, () =>
            {
                transform.DOMove(targetPos, 0.15f).SetEase(Ease.Flash).OnComplete(() =>
                {
                    zSpeed += 5f;
                    playerAnimator.SetTrigger(Run);
                    playerAction = StealNRun_PlayerAction.PlayerControl; 
                });
            });
        });
    }

    private void PlayerMovement()
    {
        transform.position += Vector3.forward * (Time.deltaTime * zSpeed);

        if (!GetMouseHeld()) return;

        var position = transform.position;

        position += new Vector3(GetDeltaMousePos().x, 0, 0) * (Time.deltaTime * xSpeed);
        var x = Mathf.Clamp(position.x, -3.5f, 3.5f);
        position = new Vector3(x, position.y, position.z);
        transform.position = position;
        transform.Rotate(new Vector3(0,GetDeltaMousePos().x ,0) * (50 * Time.deltaTime));
        transform.eulerAngles = new Vector3(0, MyHelpers.ClampAngleTo(transform.eulerAngles.y, -5, 5), 0);
    }

    private void OnEnd()
    {
        transform.eulerAngles = Vector3.zero;
        AudioManager.instance.Pause("Run");
        transform.DOMoveX(0f, 0.15f).SetEase(Ease.Flash);
        transform.DOMoveZ(endPos.z, 0.5f).SetEase(Ease.Flash).OnComplete(() =>
        {
            StealNRun_Manager.instance.ActivateManHole(() =>
            {
                playerAnimator.SetTrigger(Jump);
                StealNRun_CameraController.instance.OnEnd();
                transform.DOMoveZ(endPos.z+ 2, 0.075f).SetEase(Ease.Flash).OnComplete(() =>
                {
                    AudioManager.instance.Play("Swipe");
                    transform.position = new Vector3(transform.position.x, 3f, transform.position.z);
                    transform.DOMoveY(-5f, 0.85f).SetEase(Ease.Flash).OnComplete(() =>
                    {
                        StealNRun_Manager.instance.DeactivateManHole();
                        
                        GameCanvas.game.MakeGameResult(0,0);
                    });
                   // DOVirtual.DelayedCall(0.2f, () => gameObject.SetActive(false));
                });
            });
        });
    }

    public void PlayerCaught(float z)
    {
        AudioManager.instance.Pause("Run");
        playerAction = StealNRun_PlayerAction.PlayerCaught;
        playerAnimator.SetTrigger(Fall);
        transform.DOMoveZ(z, 0.2f).SetEase(Ease.Flash);
        GameCanvas.game.MakeGameResult(1,1);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Finish"))
        {
            StealNRun_Manager.instance.DeactivateAll();
            playerAction = StealNRun_PlayerAction.PlayerEnd;
            OnEnd();
        }
        else if (other.CompareTag("Target"))
        {
            AudioManager.instance.Pause("Run");
            playerAction = StealNRun_PlayerAction.PlayerCaught;
            playerAnimator.SetTrigger(Fall);
            transform.DOMoveZ(transform.position.z - 0.2f, 0.2f).SetEase(Ease.Flash);
            GameCanvas.game.MakeGameResult(1,1);
            StealNRun_Manager.instance.PoliceMenAttackPlayer();
        }
    }

    private Vector2 GetDeltaMousePos() => InputExtensions.GetInputDelta();
    private bool GetMouseHeld() => InputExtensions.GetFingerHeld();
}