using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class KissingChildren : MonoBehaviour
{
	[SerializeField] private bool isBoy;
	[SerializeField] private Transform boyFinalEscapeTransform;
	[SerializeField] private ParticleSystem heartsParticle;
	public static bool StartedKissing;
	
	private Animator _anim;
	private static readonly int StartKissingHash = Animator.StringToHash("StartKissing");
	private static readonly int StopKissingHash = Animator.StringToHash("StopKissing");
	private static readonly int FoundKissingHash = Animator.StringToHash("FoundKissing");
	private static readonly int TimeToRunHash = Animator.StringToHash("TimeToRun");
	private static readonly int WonHash = Animator.StringToHash("Won");
	private static readonly int LostHash = Animator.StringToHash("Lost");
	private static readonly int HasWonHash = Animator.StringToHash("HasWon");
	
	public KissingCanvas canvas;

	private void OnEnable()
	{
		KissingEvents.StartKissing += OnStartKissing;
		KissingEvents.StopKissing += OnStopKissing;
		KissingEvents.GotFoundKissing += OnFoundKissing;
		KissingEvents.FooledFather += OnFoolingFather;
	}

	private void OnDisable()
	{
		KissingEvents.StartKissing -= OnStartKissing;
		KissingEvents.StopKissing -= OnStopKissing;
		KissingEvents.GotFoundKissing -= OnFoundKissing;
		KissingEvents.FooledFather -= OnFoolingFather;
	}
	
	private void Start()
	{
		_anim = GetComponent<Animator>();
	}

	private void OnStartKissing()
	{
		StartKissing();
		StartedKissing = true;
	}

	private void OnStopKissing()
	{
		StopKissing();
		StartedKissing = false;
	}

	private void OnFoundKissing()
	{
		FoundKissing();
	}
	
	private void StartKissing()
	{
		_anim.SetTrigger(StartKissingHash);
		heartsParticle.Play();
	}
	
	private void StopKissing()
	{
		heartsParticle.Stop();
		_anim.SetTrigger(StopKissingHash);
	}

	private void FoundKissing()
	{
		TimeToRun();
		_anim.SetTrigger(FoundKissingHash);

		HasLost();
		Lost();
		heartsParticle.Stop();
	}
	
	private void TimeToRun()
	{
		// _anim.SetTrigger(TimeToRunHash);
		if (isBoy)
		{
			DOVirtual.DelayedCall(2f, ()=>
				transform.DOMove(boyFinalEscapeTransform.position, 5f));
		}
	}
	
	private void Won()
	{
		_anim.SetTrigger(WonHash);
		
	}
	
	private void Lost()
	{
		_anim.SetTrigger(LostHash);
	}
	
	private void HasWon()
	{
		_anim.SetBool(HasWonHash, true);
		heartsParticle.Stop();
		if(AudioManager.instance)
			AudioManager.instance.Play("NiceFemale");
	}

	private void HasLost()
	{
		_anim.SetBool(HasWonHash, false);
	}

	private void OnFoolingFather()
	{
		HasWon();
		Won();
	}
}
