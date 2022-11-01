using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class IrrigationRotator : MonoBehaviour
{
	[SerializeField] private GameObject objToRotate;
	[SerializeField] private GameObject rotator1,rotator2;
	[SerializeField] private GameObject rotatorParts1,rotatorParts2;
	
	[SerializeField] private float delayBetweenRotation = 2f;
	[SerializeField] private float rotationSpeed = 2f;
	[SerializeField] private float delayToRestartRotation = 3f;
	
	private bool _isHit;
	
	private void Start()
	{
		Sequence mySequence = DOTween.Sequence();
		mySequence.AppendCallback(RotateTheFan);
		mySequence.SetLoops(-1);
	}

	private void OnTriggerEnter(Collider other)
	{
		if(_isHit) return;
		
		if (!other.CompareTag("Player") && !other.CompareTag("Kart")) return;
		
		rotator1.SetActive(false);
		rotator2.SetActive(false);
		rotatorParts1.transform.parent = null;
		rotatorParts2.transform.parent = null;
		rotatorParts1.SetActive(true);
		rotatorParts2.SetActive(true);
		_isHit = true;
	}

	private void RotateTheFan()
	{
		var currentRotation = objToRotate.transform.rotation.eulerAngles;
		objToRotate.transform.DORotate(currentRotation + Vector3.up * 90, rotationSpeed).SetEase(Ease.Linear);
	}
}
