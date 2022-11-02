using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class TriangleFan : MonoBehaviour
{
	[SerializeField] private GameObject fan;
	[SerializeField] private float delayBetweenRotation = 2f;
	[SerializeField] private float rotationSpeed = 2f;
	[SerializeField] private float delayToRestartRotation = 3f;
    private void Start()
	{
		Sequence mySequence = DOTween.Sequence();
		mySequence.AppendCallback(RotateTheFan);
		mySequence.AppendInterval(delayBetweenRotation);
		mySequence.AppendCallback(RotateTheFan);
		mySequence.AppendInterval(delayToRestartRotation);
		mySequence.SetLoops(-1);
	}

	private void RotateTheFan()
	{
		var currentRotation = fan.transform.rotation.eulerAngles;
		fan.transform.DORotate(currentRotation + Vector3.forward * 90, rotationSpeed).SetEase(Ease.Linear);
	}
}
