using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class ATMCanvasController : MonoBehaviour
{
    [SerializeField] private List<GameObject> atmPanels;
    

    /// <summary>
    /// panels order in list.
    /// 0 - welcome,1-atm pin,2-enter pin,3-success pin,4-fail pin
    /// </summary>


    private const int Welcome = 0;

    private const int ShowAtmPin = 1;

    private const int EnterPin = 2;

    private const int SuccessPin = 3;

    private const int FailPin = 4;

    private const int WithdrawlPanel = 5;

    private void Start()
    {
        ActivatePanel(Welcome);
        
    }

    private void OnEnable()
    {
        ATMEvents.GirlCanPeak += OnGirlCanPeak;
        ATMEvents.SwitchToDebitInCam += OnSwitchToDebitInCam;
        ATMEvents.EnterAtmPinGamePlay += OnEnterAtmPinGamePlay;
        ATMEvents.RightAnswerPressed += OnRightAnswerPressed;
        ATMEvents.WorngAnswerPressed += OnWrongAnswerPressed;
    }

    private void OnDisable()
    {
        ATMEvents.GirlCanPeak -= OnGirlCanPeak;
        ATMEvents.SwitchToDebitInCam -= OnSwitchToDebitInCam;
        ATMEvents.EnterAtmPinGamePlay -= OnEnterAtmPinGamePlay;
        ATMEvents.RightAnswerPressed -= OnRightAnswerPressed;
        ATMEvents.WorngAnswerPressed -= OnWrongAnswerPressed;
    }

    

    private void ActivatePanel(int index)
    {
        for (int i = 0; i < atmPanels.Count; i++)
        {
            if (i == index)
            {
                atmPanels[i].SetActive(true);
            }
            else
            {
                atmPanels[i].SetActive(false);
            }
        }
    }
    
    private void OnGirlCanPeak()
    {
       ActivatePanel(ShowAtmPin);
    }

   
    
    private void OnSwitchToDebitInCam()
    {
        ActivatePanel(Welcome);
    }
    
    private void OnEnterAtmPinGamePlay()
    {
        ActivatePanel(EnterPin);
    }
    
    private void OnRightAnswerPressed()
    {
        ActivatePanel(SuccessPin);
        DOVirtual.DelayedCall(1.2f, () =>
        {
            ActivatePanel(WithdrawlPanel);
            ATMEvents.InvokeOnEnableMoneySlider();
        });
    }
    
    private void OnWrongAnswerPressed()
    {
        ActivatePanel(FailPin);
    }
}
