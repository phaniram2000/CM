using System;

public static partial class GameEvents
{
	public static event Action PreDraw, TapToPlay;
	public static event Action PressDoneButton, DoneWithRuleSet;
	public static event Action GameWin;
	public static event Action<int> GameLose;

	public static event Action ShowDoneButton;
	
	
	

}

public static partial class GameEvents
{
	public static void InvokePreDraw() => PreDraw?.Invoke();
	public static void InvokeTapToPlay() => TapToPlay?.Invoke();
	public static void InvokePressDoneButton() => PressDoneButton?.Invoke();
	public static void InvokeDoneWithRuleSet() => DoneWithRuleSet?.Invoke();
	public static void InvokeGameWin() => GameWin?.Invoke();
	public static void InvokeGameLose(int result) => GameLose?.Invoke(result);

	public static void InvokeOnShowDoneButton() => ShowDoneButton?.Invoke();

	
}