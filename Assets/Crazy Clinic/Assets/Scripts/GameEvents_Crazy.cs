using System;

public static class GameEventsCrazy
{
	public static event Action TapToPlay;
	public static event Action OptionHammer;
	public static event Action OptionCutting;
	public static event Action OptionBomb;
	public static event Action HammerTheHead;
	public static event Action HideTheHands;
	public static event Action ShowTheHands;
	public static event Action ExplosionEffects;
	public static event Action PrepareToCutTheCone;
	public static event Action DoneCutting;
	

	public static void InvokeTapToPlay() => TapToPlay?.Invoke();
	public static void InvokeHammerTheHead() => HammerTheHead?.Invoke();
	public static void InvokeOptionBomb() => OptionBomb?.Invoke();
	public static void InvokeOptionHammer() => OptionHammer?.Invoke();
	public static void InvokeOptionCutter() => OptionCutting?.Invoke();
	public static void InvokeHideTheHands() => HideTheHands?.Invoke();
	public static void InvokeShowTheHands() => ShowTheHands?.Invoke();
	public static void InvokeExplosionEffects() => ExplosionEffects?.Invoke();

	public static void InvokePrepareToCutTheCone() => PrepareToCutTheCone?.Invoke();
	public static void InvokeDoneCutting() => DoneCutting?.Invoke();
}
