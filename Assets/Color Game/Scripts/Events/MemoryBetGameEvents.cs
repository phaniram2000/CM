using System;

public static class MemoryBetGameEvents
{

	public static event Action<int> IncreasePlatformSize;

	public static event Action WrongAnswer;

	public static event Action ResetValue;

	public static event Action BetButtonPressed;


	public static void InvokeOnIncreasePlatformSize(int obj) => IncreasePlatformSize?.Invoke(obj);

	public static void InvokeOnWrongAnswer() => WrongAnswer?.Invoke();

	public static void InvokeOnResetValue() => ResetValue?.Invoke();

	public static void InvokeOnBetButtonPressed() => BetButtonPressed?.Invoke();
}