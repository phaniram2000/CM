using System;

public static partial class BankEvents
{
	public static event Action DoneWithInput;
}

public static partial class BankEvents
{
	public static void InvokeDoneWithInput() => DoneWithInput?.Invoke();
}