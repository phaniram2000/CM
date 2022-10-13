using System;
using Meta;

public static partial class MetaEvents
{
	public static event Action<int, int> AlterBankBalance, AlterRichRank;
	public static event Action<ShopCategory, int, bool> ShopItemSelect;
}
public static partial class MetaEvents
{
	public static void InvokeAlterBankBalance(int oldBalance, int newBalance) => AlterBankBalance?.Invoke(oldBalance, newBalance);
	public static void InvokeAlterRichRank(int oldRank, int newRank) => AlterRichRank?.Invoke(oldRank, newRank);
	public static void InvokeShopItemSelect(ShopCategory category, int index, bool shouldDeductBank)
	{
		ShopStateController.SelectShopItem(category, index);
		ShopItemSelect?.Invoke(category, index,
			shouldDeductBank);
	}
}