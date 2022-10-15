using Meta;

public static class MyPlayerPrefsSave
{
	public static void SetTotalMoney(float money) => SetMoney(money);
	public static float GetTotalMoney() => GetMoney();

	private static float GetMoney() => ShopStateController.CurrentState.GetBankBalance;
	private static void SetMoney(float money) => ShopStateController.CurrentState.GetState().BankBalance = (int)money;
}