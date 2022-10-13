using System;

public static partial class PatternMoneyEvents
{
	public static event Action CompletePatternStage;
}

public static partial class PatternMoneyEvents
{
	public static void InvokeCompletePatternStage() => CompletePatternStage?.Invoke();
}