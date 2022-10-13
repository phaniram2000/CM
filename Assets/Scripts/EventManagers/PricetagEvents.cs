using System;

public static partial class PricetagEvents
{
    public static event Action DoneWithInput;
}

public static partial class PricetagEvents
{
    public static void InvokeDoneWithInput() => DoneWithInput?.Invoke();
}
