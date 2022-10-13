using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class JewelleryGuard : MonoBehaviour
{
	[SerializeField] private GameObject detectionCone;
	[SerializeField] private GameObject exclamationImage;
	[SerializeField] private GameObject baton;
	[SerializeField] private Transform batonReachDestination;
	[SerializeField] private Transform policeLookAtTransform;
	
	[SerializeField] private Rig rig;
	[SerializeField] private float inBetweenWaitTime = 2f;

	private Animator _animator;
	private static readonly int ToLookHash = Animator.StringToHash("ToLook");
	private static readonly int ToThrowHash = Animator.StringToHash("ToThrow");

	private Sequence _mySeq;

	private void OnEnable()
	{
		GameEvents.TapToPlay += OnTapToPlay;
		JewelleryHeistEvents.FoundTheThief += OnFindingThief;
		JewelleryHeistEvents.HeistComplete += OnHeistCompleted;
	}

	private void OnDisable()
	{
		GameEvents.TapToPlay -= OnTapToPlay;
		JewelleryHeistEvents.FoundTheThief -= OnFindingThief;
		JewelleryHeistEvents.HeistComplete -= OnHeistCompleted;
	}

	private void Start()
	{
		_animator = GetComponent<Animator>();
	}
	
	private void SetWeight() => DOTween.To(() => rig.weight, val => rig.weight = val, 1f, 0.5f);
    
    private void RemoveWeight() => DOTween.To(() => rig.weight, val => rig.weight = val, 0f, 0.5f);
	
	private void Alerted() => exclamationImage.SetActive(true);

	private void Calm() => exclamationImage.SetActive(false);

	private void StartDetecting() => detectionCone.SetActive(true);

	private void EndDetecting() => detectionCone.SetActive(false);

	private void StartLooking()
	{
		_animator.SetBool(ToLookHash, true);
		if (AudioManager.instance)
		{
			AudioManager.instance.Play("Hmm");
		}
	}

	private void StopLooking()
	{
		_animator.SetBool(ToLookHash, false);
	}
	
	private void OnTapToPlay()
	{
		_mySeq = DOTween.Sequence();
		_mySeq.PrependInterval(3f);
		_mySeq.AppendCallback(Alerted);
		_mySeq.AppendInterval(inBetweenWaitTime);
		_mySeq.AppendCallback(StartDetecting);
		_mySeq.AppendCallback(StartLooking);
		_mySeq.AppendCallback(SetWeight);
		_mySeq.AppendInterval(inBetweenWaitTime + 2f);
		_mySeq.AppendCallback(EndDetecting);
		_mySeq.AppendCallback(Calm);
		_mySeq.AppendCallback(RemoveWeight);
		_mySeq.AppendInterval(inBetweenWaitTime);
		_mySeq.AppendCallback(StopLooking);
		_mySeq.AppendInterval(inBetweenWaitTime);
		_mySeq.SetLoops(-1);
	}

	private void PrepareToThrowTheBaton()
	{
		//Hit the thief with baton
		transform.DOLookAt(policeLookAtTransform.position,0.25f).SetEase(Ease.Linear).OnComplete(() =>
		{
			_animator.SetTrigger(ToThrowHash);
		});
		if (AudioManager.instance)
		{
			AudioManager.instance.Play("Hey");
		}
	}

	private void OnFindingThief()
	{
		IgnoreTheSequence();
		PrepareToThrowTheBaton();
	}

	public void ThrowTheBaton()
	{
		baton.transform.parent = null;
		_animator.SetBool(ToLookHash, true);
		baton.transform.DOMove(batonReachDestination.position, 0.25f).OnComplete(()=>baton.SetActive(false));
		//DOVirtual.DelayedCall(0.5f,() =>baton.transform.DOMove(batonReachDestination.position, 0.25f));
	}

	private void OnHeistCompleted()
	{
		IgnoreTheSequence();
	}

	private void IgnoreTheSequence()
	{
		_mySeq.Kill();
		Calm();
		EndDetecting();
		RemoveWeight();
		DOVirtual.DelayedCall(0.5f, PrepareToThrowTheBaton);
	}
}
