using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ShowerPrankEvents
{
	public static event Action StartPrank;
	public static event Action EndPrank;
	public static event Action GotFoundPranking;
	public static event Action DonePranking;

	public static void InvokeStartPrank() => StartPrank?.Invoke();
	public static void InvokeEndPrank() => EndPrank?.Invoke();
	public static void InvokeGotFoundPranking() => GotFoundPranking?.Invoke();
	public static void InvokeDonePranking() => DonePranking?.Invoke();
}
