using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using RPS;
using UnityEngine;

public class Cartheft_Barmeter : MonoBehaviour
{
    private Transform _transform;
    [SerializeField] private Transform slapBarArrow, arrowHolder;
    [SerializeField] private float arrowRotationDuration, rotationInitialPos, rotateEndPos, scale;
    private Tween arrowHolderTween;

    public static event Action Onmeater;
    // Start is called before the first frame update
    void Start()
    {
        _transform = transform;

        _transform.localScale = Vector3.zero;


        if (!_transform.root.TryGetComponent(out CharacterRefBank refBank)) return;

        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void meater()
    {
        Onmeater?.Invoke();
        DOVirtual.DelayedCall(3.2f, () =>
        {
            _transform.DOScale(Vector3.one * scale, 0.7f).SetEase(Ease.OutBack).OnComplete(() =>
            {
                arrowHolder.localRotation = Quaternion.Euler(0, 0, rotationInitialPos);
                arrowHolderTween = arrowHolder.DOLocalRotate(new Vector3(0, 0, rotateEndPos), arrowRotationDuration)
                    .SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);
                RPSGameEvents.InvokeOnAllowPlayerToSlap();
                RPSAudioManager.instance.Play("SlapBarMoving");
            });
        });
    }
    
}
