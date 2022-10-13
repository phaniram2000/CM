using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class TBRBoyController : MonoBehaviour
{

    [SerializeField] private int clothsId;
    [SerializeField] private List<GameObject> boyClothsToDissappearList;
    
    private Animator _anim;
    
    private static readonly int SittingAngry = Animator.StringToHash("sittingangry");
    private static readonly int StandUp = Animator.StringToHash("standup");

    private void OnEnable()
    {
        TBREvents.GirlOpenDoorDone += DoAngryAnimation;
        TBREvents.GirlPrankingDoneNowEscape += OnGirlDonePranking;
        TBREvents.ItemsButtonPressed += OnItemsButtonPressed;
    }

    private void OnDisable()
    {
        TBREvents.GirlOpenDoorDone -= DoAngryAnimation;
        TBREvents.GirlPrankingDoneNowEscape -= OnGirlDonePranking;
        TBREvents.ItemsButtonPressed -= OnItemsButtonPressed;
    }

    private void Start()
    {
        _anim = GetComponent<Animator>();
    }

    private void DoAngryAnimation()
    {
        _anim.SetTrigger(SittingAngry);
        
        if (AudioManager.instance)
            DOVirtual.DelayedCall(0.2f, () => AudioManager.instance.Play("ManShout"));
        
        
    }

    
    private void OnGirlDonePranking()
    {
        DOVirtual.DelayedCall(4f, () => MakeBoyStand());
    }

    private void MakeBoyStand()
    {
        _anim.SetTrigger(StandUp);
        
        if(AudioManager.instance)
            AudioManager.instance.Play("ManShout");

        DOVirtual.DelayedCall(1.2f, () => GameEvents.InvokeGameWin());
    }


    private void OnItemsButtonPressed(int obj)
    {
        if(obj != clothsId) return;

        for (int i = 0; i < boyClothsToDissappearList.Count; i++)
        {
            boyClothsToDissappearList[i].SetActive(false);
        }
        
    }



}
