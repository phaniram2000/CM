
using System;
using UnityEngine;

public class ATMBoyAnimationHelper : MonoBehaviour
{

    private ATMBoyController atmBoyController;


    private void Start()
    {
        atmBoyController = GetComponent<ATMBoyController>();
    }


    public void OnAnimationBoyTyingDone()
    {
        //game event to girl can peak
        ATMEvents.InvokeOnGirlCanPeak();
        
    }

    public void OnAnimationBoyInPocket()
    {
        ATMEvents.InvokeOnSwitchToStealCam();
    }

    public void OnAnimationActivateCardHolder()
    {
        if (!atmBoyController) return;
        
        atmBoyController.OnAnimationActivateCardHolder();
    }

    public void OnAnimationBoyAttack()
    {
        ATMEvents.InvokeOnGirlFall();
    }

    public void OnAnimationAngryDone()
    {
        if (!atmBoyController) return;
        
        atmBoyController.MakeBoyWalkOnFail();
    }
}
