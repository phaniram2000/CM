using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class LaserEscapeEvents
{
	public static event Action HitWithLaser;
	public static event Action EscapedAllTheLasers;
	public static event Action CrossedOneLaserGroup;
	public static event Action ResetTargetPositions;
	public static event Action EnableRigs;


	public static void InvokeHitWithLaser() => HitWithLaser?.Invoke();

	public static void InvokeEscapedAllTheLasers() => EscapedAllTheLasers?.Invoke();
	public static void InvokeCrossedOneLaserGroup() => CrossedOneLaserGroup?.Invoke();
	public static void InvokeResetTargetPositions() => ResetTargetPositions?.Invoke();
	public static void InvokeEnableRigs() => EnableRigs?.Invoke();
}
