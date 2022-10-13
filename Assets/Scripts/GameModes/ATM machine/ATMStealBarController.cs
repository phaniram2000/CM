using System;
using DG.Tweening;
using UnityEngine;

public class ATMStealBarController : MonoBehaviour
{

    [SerializeField] private Transform arrowHolder;
    [SerializeField] private float outDuration, inDuration,stealBarScale,arrowRotationDuration, rotationInitialPos, rotateEndPos;
    
    private Transform _transform;
    private Tween arrowHolderTween;

    private ATMGameController atmGameController;

    private void OnEnable()
    {
        ATMEvents.SwitchToStealCam += OnSwitchToStealCam;
        ATMEvents.PlayerAtemptToSteal += OnPlayerAtemptToSteal;
        ATMEvents.StealFail += HideStealBar;
        ATMEvents.StealSuccess += HideStealBar;
    }

    private void OnDisable()
    {
        ATMEvents.SwitchToStealCam -= OnSwitchToStealCam;
        ATMEvents.PlayerAtemptToSteal -= OnPlayerAtemptToSteal;
        ATMEvents.StealFail -= HideStealBar;
        ATMEvents.StealSuccess -= HideStealBar;
    }

    private void Start()
    {
        _transform = transform;
        _transform.localScale = Vector3.zero;
        
        var controller = GameObject.FindWithTag("GameController");

       
        if (!controller) return;

        print("controller got it");
        if (!controller.TryGetComponent(out ATMGameController gameController)) return;

        
        print("got it");
        atmGameController = gameController;
    }

    private void HideStealBar()
    {
        _transform.DOScale(Vector3.zero, outDuration).SetEase(Ease.InBack);
       
    }

    private void ShowStealBar()
    {
        _transform.DOScale(Vector3.one * stealBarScale, inDuration).SetEase(Ease.InBack).OnComplete(() =>
        {
            arrowHolder.localRotation = Quaternion.Euler(0, 0, rotationInitialPos);
            arrowHolderTween = arrowHolder.DOLocalRotate(new Vector3(0, 0, rotateEndPos), arrowRotationDuration)
                .SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);
            
            //allow to tap and steal card here
            ATMEvents.InvokeOnAllowPlayerToSteal();
            
            AudioManager.instance.Play("Tick");
            
        });
    }
    
    
    private void OnSwitchToStealCam()
    {
        DOVirtual.DelayedCall(0.5f, () => ShowStealBar());
    }
    
    
    private void OnPlayerAtemptToSteal()
    {
        arrowHolderTween.Kill();
        var arrowValue = arrowHolder.transform.localEulerAngles.z;

        print("damage not done");
        if (!atmGameController) return;
        
        if (arrowValue > 33f)
            arrowValue -= 360f;

        arrowValue = Mathf.Abs(arrowValue);

        var damage = 1 - (Mathf.InverseLerp(0, 33, arrowValue));
        
        AudioManager.instance.Pause("Tick");
        print("damage done");
        atmGameController.CheckIfStealIsSuccessfull(damage);
       
       
    }
    
    

}
