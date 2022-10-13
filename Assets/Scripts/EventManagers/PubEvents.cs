using System;

public static partial class PubEvents
{
	public static event Action StartInput, DoneWithInput;
	public static event Action MoveQueueAhead;
	public static event Action StartPostPub;
}

public static partial class PubEvents
{
	public static void InvokeStartInput() => StartInput?.Invoke();
	public static void InvokeDoneWithInput() => DoneWithInput?.Invoke();
	public static void InvokeMoveQueueAhead() => MoveQueueAhead?.Invoke();
	public static void InvokeStartPostPub() => StartPostPub?.Invoke();
}