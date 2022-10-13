
using System;
using DG.Tweening;
using UnityEngine;

public class ATMHandHelpSliderController : MonoBehaviour
{

    private RectTransform rectTransform;

    private Tween handTween;
    
    private void OnEnable()
    {
        ATMEvents.EnableMoneySlider += OnEnableMoneySlider;
        ATMEvents.WithDrawlButtonPressed += OnWithDrawlButtonPressed;
    }

    private void OnDisable()
    {
        ATMEvents.EnableMoneySlider -= OnEnableMoneySlider;
        ATMEvents.WithDrawlButtonPressed -= OnWithDrawlButtonPressed;
    }


    private void Start()
    {
        transform.localScale = Vector3.zero;
        rectTransform = GetComponent<RectTransform>();
    }
    
    private void OnEnableMoneySlider()
    {
        transform.localScale = Vector3.one;
        handTween=rectTransform.DOAnchorPosX(300, 0.6f).SetEase(Ease.Linear).SetLoops(-1,LoopType.Yoyo);
    }
    
    
    private void OnWithDrawlButtonPressed()
    {
        handTween.Kill();
        transform.localScale = Vector3.zero;
    }
    
    
    
}
