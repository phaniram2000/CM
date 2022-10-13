using System;

public static class CheatingEvents 
{
	public static event Action CheaterFound;
	public static event Action DoneCheating;
	
	public static void InvokeCheaterFound() => CheaterFound?.Invoke();
	public static void InvokeDoneCheating() => DoneCheating?.Invoke();

}
