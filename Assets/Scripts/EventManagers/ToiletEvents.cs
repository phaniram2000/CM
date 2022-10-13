using System;

public static partial class ToiletEvents
{
	public static event Action DoneWithInput, TimerStart, TimerExpired;
	public static event Action DoorSelected, GroupDone;
	
	public static event Action DontAllowToiletSwap;
}

public static partial class ToiletEvents
{
	public static void InvokeDoneWithInput() => DoneWithInput?.Invoke();
	public static void InvokeStartTimer() => TimerStart?.Invoke();
	public static void InvokeTimerExpiry() => TimerExpired?.Invoke();
	public static void InvokeGroupDone() => GroupDone?.Invoke();
	public static void InvokeDoorSelected() => DoorSelected?.Invoke();

	public static void InvokeOnDontAllowToiletSwap() => DontAllowToiletSwap?.Invoke();
}