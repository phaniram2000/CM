using System;

public static class JewelleryHeistEvents
{
	public static event Action FoundTheThief;
	public static event Action HeistComplete;
	public static event Action GotHitByTheBaton;

	public static void InvokeFoundTheThief() => FoundTheThief?.Invoke();
	public static void InvokeHeistComplete() => HeistComplete?.Invoke();
	public static void InvokeGotHitByTheBaton() => GotHitByTheBaton?.Invoke();
}
