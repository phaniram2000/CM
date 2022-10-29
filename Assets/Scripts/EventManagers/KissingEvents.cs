using System;

public static class KissingEvents
{
	public static event Action StartKissing;
	public static event Action StopKissing;
	public static event Action GotFoundKissing;
	public static event Action FooledFather;
	
	public static void InvokeStartKissing() => StartKissing?.Invoke();
	public static void InvokeStopKissing() => StopKissing?.Invoke();
	public static void InvokeGotFoundKissing() => GotFoundKissing?.Invoke();
	public static void InvokeFooledFather() => FooledFather?.Invoke();
}
