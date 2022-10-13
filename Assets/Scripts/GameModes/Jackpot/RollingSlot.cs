using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class RollingSlot : MonoBehaviour
{
	private Animator _animator;

	private void OnEnable()
	{
		GameEvents.TapToPlay += OnTapToPlay;
	}

	private void OnDisable()
	{
		GameEvents.TapToPlay += OnTapToPlay;
	}

	private void Start()
	{
		_animator = GetComponent<Animator>();
		//transform.DORotate(transform.rotation.eulerAngles + Vector3.right * 180f, 3f, RotateMode.LocalAxisAdd).SetEase(Ease.Linear).SetLoops(-1);
	}

	private void OnTapToPlay()
	{
//		_animator.StopPlayback();
		_animator.enabled = false;
	}
}
