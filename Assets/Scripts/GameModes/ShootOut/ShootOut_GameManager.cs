using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

public class ShootOut_GameManager : SingletonInstance<ShootOut_GameManager>
{
    public bool startGame;
    public int policeGroupIndex, policeIndex;
    public ShootOut_SubLevels[] subLevels;
    public  ShootOut_Player player ;
    public GameObject currencyStack;
    public ParticleSystem confetti;
    protected override void Awake()
    {
        base.Awake();
    }

    private void OnEnable()
    {
        GameEvents.TapToPlay += OnTapToPlay;
    }

    private void OnDisable()
    {
        GameEvents.TapToPlay -= OnTapToPlay;
    }

    public bool CheckThisGroupPolices()
    {
        return subLevels[policeGroupIndex].CheckThisSubLevel();
    }

    public bool CheckAllPoliceGroups()
    {
        return policeGroupIndex >= subLevels.Length-1 && CheckThisGroupPolices();
    }

    private void OnTapToPlay()
    {
        DOVirtual.DelayedCall(0.25f, () =>
        {
            startGame = true;
            player.myState = ShootOut_PlayerState.Aim;
        });
    }

    public void Vibrate(long milliSec)
    {
        Vibration.Vibrate(milliSec);
    }

}
