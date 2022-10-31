using System;

public static class GameEventsTrain
{
	public static event Action TapToPlay;
	public static event Action EndLevel;
	public static event Action IsHoldingCake;

	public static void InvokeTapToPlay() => TapToPlay?.Invoke();
	public static void InvokeEndLevel() => EndLevel?.Invoke();
	public static void InvokeIsHoldingCake() => IsHoldingCake?.Invoke();
}
