using System;
using System.Collections.Generic;
using DG.Tweening;
using Dreamteck.Splines;
using UnityEngine;

public class ToiletHelper : HelperBase
{
	private bool _areSignsSwapped;
	public static bool GetAreSignsSwapped
	{
		get => Get._areSignsSwapped;
		set => Get._areSignsSwapped = value;
	}
	
	public static void StartLeftDoor() => Get.ExitLeftDoor();
	public static void StartRightDoor() => Get.ExitRightDoor();

	private static ToiletHelper Get;
	private int _groupsSent = 0;

	[SerializeField] private Transform hiddenTransform;


	[SerializeField] private SplineComputer menEntrySpline, womenEntrySpline,
									menExitSpline, womenExitSpline, menExitSplineKicked;

	[SerializeField] private List<GroupMember> enterGroup, exitGroup1Order, exitGroup2Order;

	public override Action OnSwapComplete() => ToggleSwap;
	
	private void OnEnable()
	{
		ToiletEvents.DoneWithInput += OnDoneInput;
	}

	private void OnDisable()
	{
		ToiletEvents.DoneWithInput -= OnDoneInput;
	}

	private void Awake()
	{
		if (!Get) Get = this;
		else Destroy(gameObject);
	}

	private SplineComputer GetMenEntrySpline() => _areSignsSwapped ? womenEntrySpline : menEntrySpline;

	private SplineComputer GetWomenEntrySpline() => _areSignsSwapped ? menEntrySpline : womenEntrySpline;

	private SplineComputer GetMenExitSpline() => _areSignsSwapped ? womenExitSpline : menExitSpline;

	private SplineComputer GetWomenExitSpline() => _areSignsSwapped ? menExitSpline : womenExitSpline;

	public SplineComputer GetMenExitSplineKicked() => menExitSplineKicked;

	public Vector3 GetHiddenLocation() => hiddenTransform.position;

	private void ToggleSwap() => _areSignsSwapped = !_areSignsSwapped;

	private void SendToToilets()
	{
		var cumulative = 0f;
		foreach (var member in enterGroup)
		{
			cumulative += member.waitTimeDelta;
			member.spliner.GetNpc(out var isFemale)
				.EnterToilets(cumulative, isFemale ? GetWomenEntrySpline() : GetMenEntrySpline());
		}
	}

	private void ExitRightDoor()
	{
		var cumulative = 0f;
		var spline = GetMenExitSpline();
		foreach (var member in exitGroup1Order)
		{
			cumulative += member.waitTimeDelta;
			var npc = member.spliner.GetNpc(out var isFemale);

			if (isFemale)
				((ToiletFemale)npc).ChaseAwayMen(cumulative, GetMenExitSplineKicked());
			else
				((ToiletMale)npc).RunFromWomensToilet(cumulative, spline);
		}

		DOVirtual.DelayedCall(cumulative, () =>
		{
			ToiletEvents.InvokeGroupDone();
			CheckIfGameDone();
		});
	}

	private void ExitLeftDoor()
	{
		var cumulative = 0f;
		var spline = GetWomenExitSpline();
		foreach (var member in exitGroup2Order)
		{
			cumulative += member.waitTimeDelta;
			var npc = member.spliner.GetNpc(out var isFemale);
			
			if (isFemale)
				((ToiletFemale)npc).RunAwayFromMen(cumulative, spline);
			else
				((ToiletMale)npc).RunTowardsWomen(cumulative, spline);
		}
		
		DOVirtual.DelayedCall(cumulative, () =>
		{
			ToiletEvents.InvokeGroupDone();
			CheckIfGameDone();
		});
	}

	private void CheckIfGameDone()
	{
		if (++_groupsSent >= 2)
		{
			DOVirtual.DelayedCall(3f, GameEvents.InvokeGameWin);
			if (AudioManager.instance)
			{
				DOVirtual.DelayedCall(2f,()=>AudioManager.instance.Play("WhatHappened"));
			}
		}
	}

	private void OnDoneInput() => SendToToilets();
}

[Serializable]
public struct GroupMember
{
	public ToiletSplineFollower spliner;
	public float waitTimeDelta;
}