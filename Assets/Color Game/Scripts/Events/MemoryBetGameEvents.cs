using System;

public static class MemoryBetGameEvents
{

	public static event Action<int> IncreasePlatformSize;


	public static void InvokeOnIncreasePlatformSize(int obj) => IncreasePlatformSize?.Invoke(obj);
}