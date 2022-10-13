using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EatingFoodEvents
{
	public static event Action GotFull;
	public static event Action AteAllTheFood;


	public static void InvokeGotFull() => GotFull?.Invoke();
	public static void InvokeAteAllFood() => AteAllTheFood?.Invoke();
}
