using UnityEngine;

namespace Meta
{
	public class ShopStateController : MonoBehaviour
	{
		//cant use call application.datapath from instance field initializer
		public static ShopStateSerializer ShopStateSerializer { get; private set; }
		public static ShopStateHelpers CurrentState { get; private set; } 

		private void Awake()
		{
			ShopStateSerializer = new ShopStateSerializer(Application.persistentDataPath + "/shopState.save");
			CurrentState = new ShopStateHelpers(ShopStateSerializer.LoadSavedState());
		}

		private void Update()
		{
			if (Input.GetKeyDown(KeyCode.D)) ShopStateSerializer.DeleteSavedState();
			if (Input.GetKeyDown(KeyCode.O)) AlterBankBalance(500, true);
		}

		public static void SelectShopItem(ShopCategory category, int index)
		{
			//mark purchased weapon as selected
			CurrentState.GetState().CategoryStates[(int) category].ItemStates[index] = ShopItemState.Selected;
			
			//make sure nobody else is selected/ old one is now marked as unlocked
			for (var i = 0; i < ShopStateHelpers.GetCategoryItemCount(category); i++)
			{
				if (i == index) continue;

				if (CurrentState.GetState().CategoryStates[(int) category].ItemStates[i] == ShopItemState.Selected)
					CurrentState.GetState().CategoryStates[(int) category].ItemStates[i] = ShopItemState.Unlocked;
			}
		}

		public static void AlterBankBalance(int change, bool shouldSaveState = false)
		{
			var old = CurrentState.GetBankBalance; 
			CurrentState.GetState().BankBalance += change;
			if(shouldSaveState)
				ShopStateSerializer.SaveCurrentState();
			
			
			MetaEvents.InvokeAlterBankBalance(old, CurrentState.GetBankBalance);
		}

		public static void AlterRichRank(int change, bool shouldSaveState = false)
		{
			var old = CurrentState.GetRichRank;
			CurrentState.GetState().RichRank += change;
			if (CurrentState.GetState().RichRank <= 0)
			{
				CurrentState.GetState().RichRank = 1;
			}
			if(shouldSaveState)
				ShopStateSerializer.SaveCurrentState();
			
			MetaEvents.InvokeAlterRichRank(old, CurrentState.GetRichRank);
		}
	}
}