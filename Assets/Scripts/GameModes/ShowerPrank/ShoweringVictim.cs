using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class ShoweringVictim : MonoBehaviour
{
	private Animator _animator;

	private static readonly int StartWashingHash = Animator.StringToHash("StartWashing");
	private static readonly int StopWashingHash = Animator.StringToHash("EndWashing");
	private static readonly int LookHash = Animator.StringToHash("Look");
	private static readonly int IrritatedHash = Animator.StringToHash("Irritated");
	private static readonly int FoundPrankingHash = Animator.StringToHash("FoundPranking");
	private static readonly int ChaseHash = Animator.StringToHash("Chase");
	private static readonly int JumpHash = Animator.StringToHash("Jump");
	private static readonly int KnockOutHash = Animator.StringToHash("KnockOut");
	private static readonly int FallDownHash = Animator.StringToHash("FallDown");

	private Sequence _mySeq;

	[SerializeField] private Rig rig;
	[SerializeField] private GameObject detectionCone;
	[SerializeField] private GameObject exclamationImage;

	[SerializeField] private Transform lookAtTransform;
	[SerializeField] private Transform initialPosTransform;
	[SerializeField] private Transform pranksterTransform;
	[SerializeField] private Transform jumpPosTransform;
	[SerializeField] private Transform finalRunPosTransform;
	
	[SerializeField] private ParticleSystem headBubbles;
	[SerializeField] private ParticleSystem bodyBubbles;

	private void OnEnable()
	{
		//ShowerPrankEvents.StartPrank += StartWashing;
		ShowerPrankEvents.GotFoundPranking += FoundPrankster;
		ShowerPrankEvents.DonePranking += OnDonePranking;
		GameEvents.TapToPlay += ShampooRoutine;
	}

	private void OnDisable()
	{
		//ShowerPrankEvents.StartPrank -= StartWashing;
		ShowerPrankEvents.GotFoundPranking -= FoundPrankster;
		ShowerPrankEvents.DonePranking -= OnDonePranking;
		GameEvents.TapToPlay -= ShampooRoutine;
	}

	private void Start()
	{
		_animator = GetComponent<Animator>();

		//ShampooRoutine();
	}

	private void StartWashing()
	{
		_animator.SetTrigger(StartWashingHash);
		Calm();
		PlayHeadBubble();
		PlayBodyBubble();
		RemoveWeight();
		PlaySound();
	}

	private void StopWashing()
	{
		_animator.SetTrigger(StopWashingHash);
		detectionCone.SetActive(false);
		RemoveWeight();
		StopHeadBubble();
		PauseSound();
	}
	
	private void Look()
	{
		_animator.SetTrigger(LookHash);
		detectionCone.SetActive(true);
		SetWeight();
		PauseSound();
		if (AudioManager.instance)
			AudioManager.instance.Play("Hmm");
	}

	private void Irritated()
	{
		_animator.SetTrigger(IrritatedHash);
	}

	private void PlayHeadBubble()
	{
		headBubbles.Play();
	}
	
	private void StopHeadBubble()
	{
		headBubbles.Stop();
	}
	
	private void PlayBodyBubble()
	{
		bodyBubbles.Play();
	}

	private void ShampooRoutine()
	{
		_mySeq = DOTween.Sequence();
		
		// _mySeq.PrependInterval(3f);
		_mySeq.AppendCallback(StartWashing);
		_mySeq.AppendInterval(2f);
		_mySeq.AppendCallback(Alert);
		_mySeq.AppendInterval(2f);
		_mySeq.AppendCallback(StopWashing);
		_mySeq.AppendInterval(1f);
		_mySeq.AppendCallback(()=>transform.DOLookAt(lookAtTransform.position, 0.25f));
		_mySeq.AppendCallback(Look);
		_mySeq.AppendInterval(3f);
		_mySeq.AppendCallback(Calm);
		_mySeq.AppendCallback(()=>detectionCone.SetActive(false));
		_mySeq.AppendCallback(() => transform.DOLookAt(initialPosTransform.position, 0.25f).SetEase(Ease.Linear));
		// _mySeq.AppendInterval(3f);
		// _mySeq.AppendCallback(StartWashing);
		_mySeq.SetLoops(-1);
	}

	private void StopBodyBubble()
	{
		bodyBubbles.Stop();
	}

	private void SetWeight() => DOTween.To(() => rig.weight, val => rig.weight = val, 1f, 0.5f);

	private void RemoveWeight() => DOTween.To(() => rig.weight, val => rig.weight = val, 0f, 0.5f);

	private void FoundPrankster()
	{
		_mySeq.Kill();
		detectionCone.SetActive(false);
		RemoveWeight();
		Calm();
		// angry animation
		_animator.SetTrigger(FoundPrankingHash);
		transform.DOLookAt(pranksterTransform.position, 0.25f).SetEase(Ease.Linear);
		//Jump out and chase the prankster
		DOVirtual.DelayedCall(3f, ChaseThePrankster);
		PauseSound();
		if (AudioManager.instance)
			AudioManager.instance.Play("Hey");
	}

	private void ChaseThePrankster()
	{
		_animator.SetTrigger(JumpHash);
		transform.DOLookAt(jumpPosTransform.position, 0.1f);
		transform.DOJump(jumpPosTransform.position, 1, 1, 0.25f).SetEase(Ease.Linear).OnComplete(() =>
		{
			_animator.SetTrigger(ChaseHash);
			transform.DOMove(finalRunPosTransform.position, 3f).SetEase(Ease.Linear);
		});
	}

	private void Alert()
	{
		exclamationImage.SetActive(true);
	}

	private void Calm()
	{
		exclamationImage.SetActive(false);
	}

	private void OnDonePranking()
	{
		if(_mySeq.active)
			_mySeq.Kill();
		
		_animator.SetTrigger(KnockOutHash);
		_animator.SetTrigger(FallDownHash);
		detectionCone.SetActive(false);
		Calm();
		RemoveWeight();	
		StopBodyBubble();
		PauseSound();
		if (AudioManager.instance)
			AudioManager.instance.Play("Exhausted");
	}

	private void PlaySound()
	{
		if (AudioManager.instance)
			AudioManager.instance.Play("HeadWash");
	}

	private void PauseSound()
	{
		if (AudioManager.instance)
			AudioManager.instance.Pause("HeadWash");
	}
}
