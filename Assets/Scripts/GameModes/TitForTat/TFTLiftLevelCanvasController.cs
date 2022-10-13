using System;
using DG.Tweening;
using UnityEngine;

public class TFTLiftLevelCanvasController : MonoBehaviour
{
    [SerializeField] private GameObject firstLiftLevelController;
    [SerializeField] private float firstLevelEndValue,firstLevelDuration;

    private void OnEnable()
    {
        TFTGameEvents.ActivateNextScene += OnActivateNextScene;
    }

    private void OnDisable()
    {
        TFTGameEvents.ActivateNextScene -= OnActivateNextScene;
    }


    private void OnActivateNextScene()
    {
        DOVirtual.DelayedCall(1f, () => ChangeLiftLevel());
    }


    private void ChangeLiftLevel()
    {
        firstLiftLevelController.GetComponent<RectTransform>().DOAnchorPosY(firstLevelEndValue, firstLevelDuration)
            .SetEase(Ease.Linear).OnComplete(
                () =>
                {
                    DOVirtual.DelayedCall(0.5f, () => TFTGameEvents.InvokeOnOpenLiftDoors()); 
                });
    }
}
