using System;
using System.Collections.Generic;
using System.Linq;

namespace Meta
{
	public enum ShopCategory { Outfit, Mask, Hat };
	
	[Serializable] public class ShopState
	{
		public int BankBalance { get; set;}

		public int RichRank { get; set;}

		public int LoaderIndex { get; set; }
		
		public List<ShopCategoryState> CategoryStates;

		public ShopState(int newBankBalance, int richRank, int loaderIndex, 
			List<ShopCategoryState> states)
		{
			BankBalance = newBankBalance;
			RichRank = richRank;
			CategoryStates = states;
			LoaderIndex = loaderIndex;
		}
	}

	[Serializable] public struct ShopCategoryState
	{
		public ShopCategory Category;
		public Dictionary<int, ShopItemState> ItemStates;
		
		private bool _allItemsUnlocked;
		public bool MarkAllItemsUnlocked() => _allItemsUnlocked = true;

		public bool AreAllItemsUnlocked()
		{
			if (_allItemsUnlocked) return true;

			if (ItemStates.Any(state => state.Value is ShopItemState.Locked or ShopItemState.RejectedInLoader))
				return false;

			_allItemsUnlocked = true;
			return true;
		}
	}
	
	public class ShopStateHelpers
	{
		private readonly ShopState _shopState;

		public ShopStateHelpers(ShopState shopState) => _shopState = shopState;

		public ShopState GetState() => _shopState;
		
		public int GetBankBalance => _shopState.BankBalance;
		public int GetRichRank => _shopState.RichRank;

		public void SetNewLoaderIndex(int index) => _shopState.LoaderIndex = index;
		public int GetLoaderIndex() => _shopState.LoaderIndex;

		public Dictionary<int, ShopItemState> GetItemStates(ShopCategory category)
		{
			foreach (var state in _shopState.CategoryStates)
			{
				if (state.Category == category)
					return state.ItemStates;
			}
			return null;
		}

		public static int GetCategoryItemCount(ShopCategory category)
		{
			return category switch
			{
				ShopCategory.Outfit => Enum.GetNames(typeof(OutfitName)).Length,
				ShopCategory.Mask => Enum.GetNames(typeof(MaskName)).Length,
				ShopCategory.Hat => Enum.GetNames(typeof(HatName)).Length,
				_ => throw new ArgumentOutOfRangeException(nameof(category), category, null)
			};
		}

		public static int GetCategoryItemCount(int categoryIndex)
		{
			var category = (ShopCategory)categoryIndex;
			return GetCategoryItemCount(category);
		}
	}
}