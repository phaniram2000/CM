using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

public class StealNRun_Manager : SingletonInstance<StealNRun_Manager>
{
    public GameObject manHoleCover, currencyStack, policeMenStack;
    public StealNRun_PoliceMan[] policeMen;

    public StealNRun_Currency[] totalCurrency;

    protected override void Awake()
    {
        base.Awake();
        GetData();
    }
    
    private void GetData()
    {
        totalCurrency = new StealNRun_Currency[currencyStack.transform.childCount-1];
        for (int i = 0; i < currencyStack.transform.childCount-1; i++)
        {
            totalCurrency[i] = currencyStack.transform.GetChild(i).GetComponent<StealNRun_Currency>();
        }
        
        policeMen = new StealNRun_PoliceMan[policeMenStack.transform.childCount];
        for (int i = 0; i < policeMenStack.transform.childCount; i++)
        {
                policeMen[i] = policeMenStack.transform.GetChild(i).GetComponent<StealNRun_PoliceMan>();
        }
    }

    private void OnEnable()
    {
        GameEvents.TapToPlay += OnTapToPlay;
    }

    private void OnDisable()
    {
        GameEvents.TapToPlay -= OnTapToPlay;
    }

    private static void OnTapToPlay()
    {
        StealNRun_CameraController.instance.OnTapToPlay();
        StealNRun_PlayerController.instance.OnTapToPlay();
    }

    public void PoliceMenAttackPlayer()
    {
        for (int i = 0; i < policeMen.Length; i++)
        {
            policeMen[i].AttackPlayer();
        }
    }

    public void DeactivateAll()
    {
        foreach (var t in policeMen)
        {
            t.DeactiveMe();
        }

        for (int i = 0; i < totalCurrency.Length; i++)
        {
            totalCurrency[i].gameObject.SetActive(false);
        }
    }

    public void ActivateManHole(UnityAction callBack)
    {
        manHoleCover.transform.DORotate(new Vector3(70, 0, 0), 0.2f).SetEase(Ease.Flash).OnComplete(() =>
        {
            callBack?.Invoke();
        });
    }
    
    public void DeactivateManHole()
    {
        manHoleCover.transform.DORotate(new Vector3(0, 0, 0), 0.2f).SetEase(Ease.InOutBounce);
    }
}
