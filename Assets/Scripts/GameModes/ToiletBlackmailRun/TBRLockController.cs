using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class TBRLockController : MonoBehaviour
{
    [SerializeField] private Transform lockHandle;
    [SerializeField] private float handleLockValue,lockhandleDuration;

    private void OnEnable()
    {
        TBREvents.GirlStartingToLockDoor += LockDoor;
    }

    private void OnDisable()
    {
        TBREvents.GirlStartingToLockDoor -= LockDoor;
    }

    private void LockDoor()
    {
        lockHandle.DOLocalMoveX(handleLockValue, lockhandleDuration).SetEase(Ease.Linear);
    }
}
