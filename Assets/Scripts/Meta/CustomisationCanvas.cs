using System;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Meta
{
	public partial class CustomisationCanvas : MonoBehaviour
	{
		public static CustomisationCanvas shop;
		
		[SerializeField] private List<ShopCategoryUI> shopCategories;
		[SerializeField] private GameObject shopItemPrefab;

		[SerializeField] private TextMeshProUGUI bankAmount;
		
		private void OnEnable()
		{
			MetaEvents.ShopItemSelect += OnShopItemSelect;
			MetaEvents.AlterBankBalance += OnAlterBankBalance;
		}

		private void OnDisable()
		{
			MetaEvents.ShopItemSelect -= OnShopItemSelect;
			MetaEvents.AlterBankBalance -= OnAlterBankBalance;
		}

		private void Awake()
		{
			ReadCurrentShopState(true);

			if (!shop) shop = this;
			else Destroy(gameObject);
		}

		private void Start()
		{
			for(var i = 0; i< shopCategories.Count; i++) ((RectTransform)shopCategories[i].itemHolder.transform).anchoredPosition = Vector2.zero;
			EnableShopCategory(0);
		}

		private void Update()
		{
			if (Input.GetKeyDown(KeyCode.R)) SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
		}

		/// <summary>
		/// This function not only reads the change of the values, but also sets all the ShopItems to the visual and behavioural representation they should be in.
		/// </summary>
		/// <param name="initialising"> Optional parameter. Only set to true when calling to initialise values/ generate scroll views.</param>
		private void ReadCurrentShopState(bool initialising = false)
		{
			var currentShopState = initialising
				? ShopStateController.ShopStateSerializer.LoadSavedState()
				: ShopStateController.CurrentState.GetState();

			for (var i = 0; i < shopCategories.Count; i++)
			{
				var currentCount = ShopStateHelpers.GetCategoryItemCount(i);
				var currentCategory = shopCategories[i];

				var currentRect = (RectTransform)currentCategory.itemHolder.transform;
				currentRect.sizeDelta = new Vector2(
					Mathf.CeilToInt(currentCount / (float)currentCategory.itemHolder.constraintCount) *
					(currentCategory.itemHolder.cellSize.x + currentCategory.itemHolder.spacing.x),
					currentRect.sizeDelta.y);
				
				for (var j = 0; j < currentCount; j++)
				{
					var item = initialising
						? Instantiate(shopItemPrefab, currentCategory.itemHolder.transform).GetComponent<ShopItem>()
						: currentCategory.itemHolder.transform.GetChild(j).GetComponent<ShopItem>();

					var itemState = currentShopState.CategoryStates[i].ItemStates[j];
					item.SetSkinIndex(j);
					item.SetState(itemState);
					item.SetIconSprite(GetOutfitSprite(currentCategory, j));
					item.SetIsCharacterItem(currentCategory.myCategory);

					//if you are having an index out of bounds error over here, check if prices and sprites have equal no of items
					item.SetPriceAndAvailability(GetCustomisationCost(currentCategory, j));
				}
			}

			if(initialising)
				UpdateBankBalanceText();
		}

		private void SaveCurrentShopState()
		{
			//save the newest made change of state
			ShopStateController.ShopStateSerializer.SaveCurrentState();
			
			//make shop items represent their state acc to new change of state 
			ReadCurrentShopState();
		}
		
		private void EnableShopCategory(int index)
		{
			for (var i = 0; i < shopCategories.Count; i++)
			{
				shopCategories[i].itemHolder.transform.parent.parent.gameObject.SetActive(i == index);
				shopCategories[i].buttonSprite.SetSelected(i == index);
			}
			if (AudioManager.instance)
			{
				AudioManager.instance.Play("Button");
			}
			Vibration.Vibrate(30);
		}

		public void SelectButton(int index) => EnableShopCategory(index);

		private void OnShopItemSelect(ShopCategory shopCategory, int index, bool shouldDeductCoins)
		{
			if (shouldDeductCoins)
			{
				//if was locked before this, Decrease coin count
				var oldAmount = ShopStateController.CurrentState.GetBankBalance;
				var newAmount = oldAmount - 
								GetCustomisationCost(shopCategories[
									shopCategories.FindIndex(cat => cat.myCategory == shopCategory)], index);
				
				ShopStateController.CurrentState.GetState().BankBalance = newAmount;

				DOTween.To(() => oldAmount, value => oldAmount = value, newAmount, 1.5f)
					.SetEase(Ease.OutCubic)
					.OnUpdate(() => bankAmount.text = "$ " + oldAmount);
			}

			//Save the state and reflect it in Shop UI
			SaveCurrentShopState();
		}

		private void OnAlterBankBalance(int oldAmount, int newAmount)
		{
			print("on alter bak balance");
			
			TweenCashText(oldAmount, newAmount);
			ReadCurrentShopState();
		}
	}
	
	public partial class CustomisationCanvas
	{
	#region Helpers and getters

		[Serializable] private struct ShopCategoryUI
		{
			public ShopCategory myCategory;
			public GridLayoutGroup itemHolder;
			public ButtonSprite buttonSprite;

			public List<Sprite> sprites;
			public List<int> costs;
			
			[Serializable] public struct ButtonSprite
			{
				[SerializeField] private Image button;
				[SerializeField] private  Sprite selected, deselected;
				
				public void SetSelected(bool status) => button.sprite = status ? selected : deselected;
			}
		}
		
		private void UpdateBankBalanceText()
		{
			var x = ShopStateController.CurrentState.GetBankBalance;
			MetaEvents.InvokeAlterBankBalance(x, x);
			TweenCashText(x, x);
		}
		
		private void TweenCashText(int oldAmount, int newAmount)
		{
			var temp = oldAmount;
			DOTween.To(() => temp, value => temp = value, newAmount, 1f)
				.SetEase(Ease.OutCubic)
				.OnUpdate(() => bankAmount.text = $"$ {temp}");
		}

		private static Sprite GetOutfitSprite(ShopCategoryUI category, int index)
		{
			var currentList = category.sprites;
		
			if (index >= currentList.Count)
				return currentList[^1];

			return currentList[index];
		}

		private static int GetCustomisationCost(ShopCategoryUI category, int index) => category.costs[index];
	#endregion
	}
}