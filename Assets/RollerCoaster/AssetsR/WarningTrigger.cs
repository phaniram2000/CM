using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class WarningTrigger : MonoBehaviour
{
	[SerializeField] private float delayForDeactivation = 3f;

	private bool _isPlayerOnFever;

	private void OnEnable()
	{
		GameEventsR.PlayerOnFever += DisableWarningPanel;
		GameEventsR.PlayerOffFever += ResetTrigger;
	}

	private void OnDisable()
	{
		GameEventsR.PlayerOnFever -= DisableWarningPanel;
		GameEventsR.PlayerOffFever -= ResetTrigger;
	}

	private void OnTriggerEnter(Collider other)
	{
		if (_isPlayerOnFever) return;
			
		if (!other.CompareTag("Player")) return;
		
		GameEventsR.InvokeObstacleWarningOn();
		DOVirtual.DelayedCall(delayForDeactivation,GameEventsR.InvokeObstacleWarningOff);
	}

	private void DisableWarningPanel()
	{
		_isPlayerOnFever = true;
		GameEventsR.InvokeObstacleWarningOff();
	}

	private void ResetTrigger()
	{
		_isPlayerOnFever = false;
	}
}
