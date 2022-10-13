using System;
using UnityEngine;

public class BankHelper : HelperBase
{
	[SerializeField] private Transform drawCamSource, drawCamDest,
										preDrawCamSource, preDrawCamDest;

	private void Start()
	{
		SetDrawCam();
		SetPreDrawCam();
	}

	private void SetDrawCam()
	{
		drawCamDest.position = drawCamSource.position;
		drawCamDest.rotation = drawCamSource.rotation;
	}

	private void SetPreDrawCam()
	{
		preDrawCamDest.position = preDrawCamSource.position;
		preDrawCamDest.rotation = preDrawCamSource.rotation;
	}

	public override Func<int, string> ResultTextFormatter() =>
		id => ((BankRuleSet)GameRules.GetRuleSet).desiredGestureId == id 
		? "Match!" 
		: "No Match!";
}