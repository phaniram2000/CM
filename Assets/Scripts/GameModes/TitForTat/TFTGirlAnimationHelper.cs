using System;
using DG.Tweening;
using UnityEngine;

public class TFTGirlAnimationHelper : MonoBehaviour
{

    private TFTGameController gameController;
    
    private void Start()
    {
        gameController = GameObject.FindWithTag("GameController").GetComponent<TFTGameController>();
    }

    public void OnCryingAnimationDone()
    {
        if (!gameController) return;

        DOVirtual.DelayedCall(gameController.WaitDurationBeforeSceneTransition,
            () => TFTGameEvents.InvokeOnDoSceneTransition());

    }
}
