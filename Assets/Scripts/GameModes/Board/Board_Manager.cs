using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Board_Manager : MonoBehaviour
{
    [Header("Story")] public Animator pranker;
    public Animator othergirl;
    public GameObject girlmessage;
    public GameObject prankermessage;
    public Transform Movetodoor;
    private static readonly int walk = Animator.StringToHash("Walk");
    [Header("Game")]
    public Animator pranker_Game;
    public List<Animator> Classgirls_game;
    public Animator othergirl_in_class;
    private static readonly int Final = Animator.StringToHash("Final");
    private static readonly int Pray = Animator.StringToHash("Pray");
    private static readonly int Dance = Animator.StringToHash("Dance");
    private static readonly int sad = Animator.StringToHash("Sad");
    [Header("UI")] public GameObject Stage1_UI;
    public GameObject Stage2_UI;
    public GameObject RIPprincipal;
    public GameObject Holiday;
    public Transform help, helpmovepoint;
    [Header("Duster")] public GameObject Duster;
    public GameObject Eraseboard;
    [Header("Camera")] public Animator Camera;
    private static readonly int Board = Animator.StringToHash("Board");
    private static readonly int Students = Animator.StringToHash("Student");

    private void OnEnable()
    {
        GameEvents.TapToPlay += OnTapToPlay;
        EraseTarget.Ondoneerasing += Doneerasing;
    }

    private void OnDisable()
    {
        GameEvents.TapToPlay -= OnTapToPlay;
        EraseTarget.Ondoneerasing -= Doneerasing;
    }

    private void Start()
    {
        Vibration.Init();
    }

    private void OnTapToPlay()
    {
        girlmessage.transform.DOScale(Vector3.one, .3f).SetEase(Ease.OutBounce);
        othergirl.SetTrigger("Message");
        DOVirtual.DelayedCall(3, () =>
        {
            prankermessage.transform.DOScale(Vector3.one, .3f).SetEase(Ease.OutBounce);
            pranker.SetTrigger("Message");
        });
        StorySequence();
        // Stage1_UI.transform.DOScale(Vector3.one, .3f).SetEase(Ease.OutBounce);
    }

    public void Onduster()
    {
        Duster.gameObject.SetActive(true);
        help.DOMove(helpmovepoint.position, .3f).SetEase(Ease.OutBounce);
        DOVirtual.DelayedCall(5, () => help.DOScale(Vector3.zero, .3f));
        Eraseboard.tag = "EraseTarget";
        Stage1_UI.transform.DOScale(Vector3.zero, .3f).SetEase(Ease.OutBounce);
        Duster.SetActive(true);
        Duster.transform.DOScale(Vector3.one * 100, .3f).SetEase(Ease.OutBounce);
        if (AudioManager.instance)
        {
            AudioManager.instance.Play("TapToPlay");
        }
        Vibration.Vibrate(100);
    }

    public void OnRIPprincipal()
    {
        RIPprincipal.transform.DOScale(Vector3.one, .3f).SetEase(Ease.OutBounce);
        Stage2_UI.transform.DOScale(Vector3.zero, .3f).SetEase(Ease.OutBounce);
        DOVirtual.DelayedCall(1.5f, () =>
        {
            Camera.SetTrigger(Students);
            pranker_Game.SetTrigger(Final);
            othergirl_in_class.SetTrigger(sad);
            pranker_Game.transform.DORotate(new Vector3(0, -14.922f, 0), .3f);
            for (int i = 0; i < Classgirls_game.Count; i++)
            {
                Classgirls_game[i].SetTrigger(Pray);
            }
        });
        DOVirtual.DelayedCall(3, () =>
        {
            GameEvents.InvokeGameWin();
        });
        if (AudioManager.instance)
        {
            AudioManager.instance.Play("TapToPlay");
            AudioManager.instance.Play("Awe");
        }
        Vibration.Vibrate(100);   
        help.DOScale(Vector3.zero, .3f).SetEase(Ease.InBounce);
    }

    public void OnHoliday()
    {
        Holiday.transform.DOScale(Vector3.one, .3f).SetEase(Ease.OutBounce);
        Stage2_UI.transform.DOScale(Vector3.zero, .3f).SetEase(Ease.OutBounce);
        DOVirtual.DelayedCall(1.5f, () =>
        {
            Camera.SetTrigger(Students); 
            pranker_Game.SetTrigger(Final);
            othergirl_in_class.SetTrigger(sad);
            pranker_Game.transform.DORotate(new Vector3(0, -14.922f, 0), .3f);
            for (int i = 0; i < Classgirls_game.Count; i++)
            {
                Classgirls_game[i].SetTrigger(Dance);
            }
        });
        DOVirtual.DelayedCall(3, () =>
        {
            GameEvents.InvokeGameWin();
        });
        if (AudioManager.instance)
        {
            AudioManager.instance.Play("TapToPlay");
            AudioManager.instance.Play("Holiday");
        }
        Vibration.Vibrate(100);
        
    }

    public void Doneerasing()
    {
        DOVirtual.DelayedCall(.3f, () => Stage2_UI.transform.DOScale(Vector3.one, .3f).SetEase(Ease.OutBounce));
        Duster.SetActive(false);
    }

    //story
    public void StorySequence()
    {
        var seq = DOTween.Sequence();
        seq.AppendInterval(6);
        seq.Append(othergirl.transform.DOLookAt(Movetodoor.position, .8f));
        seq.Append(pranker.transform.DOLookAt(Movetodoor.position, .8f));
        seq.AppendCallback(() =>
        {
            pranker.SetTrigger(walk);
            pranker.transform.DOMove(Movetodoor.position, 3);
            othergirl.SetTrigger(walk);
            othergirl.transform.DOMove(Movetodoor.position, 4);
        });
        seq.AppendInterval(2);
        seq.AppendCallback(() =>
        {
            Camera.SetTrigger(Board);
            //  Stage1_UI.transform.DOScale(Vector3.one, .3f).SetEase(Ease.OutBounce);
            pranker.gameObject.SetActive(false);
            othergirl.gameObject.SetActive(false);
        });
        seq.AppendInterval(1);
        seq.AppendCallback(() => { Stage1_UI.transform.DOScale(Vector3.one, .3f).SetEase(Ease.OutBounce); });
    }
}