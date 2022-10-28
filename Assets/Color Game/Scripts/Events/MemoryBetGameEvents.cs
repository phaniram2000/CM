using System;

public static class MemoryBetGameEvents
{

	public static event Action<int> IncreasePlatformSize;

	public static event Action WrongAnswer;


	public static void InvokeOnIncreasePlatformSize(int obj) => IncreasePlatformSize?.Invoke(obj);

	public static void InvokeOnWrongAnswer() => WrongAnswer?.Invoke();
}