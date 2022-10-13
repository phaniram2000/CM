using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace Meta
{
	public class MetaCharacterCustomisation : MonoBehaviour
	{
		public CharacterName characterName;
		[SerializeField] private OutfitMeshes outfitMeshes;
		[SerializeField] private List<GameObject> masks, hats;
		[SerializeField] private List<bool> hatsToTurnOffHairIn;
		[SerializeField] private List<GameObject> hairObjectsToTurnOff;

		public List<Outfit> outfits;
		[SerializeField] private int currentOutfit, currentHat, currentMask;

		private void OnEnable()
		{
			MetaEvents.ShopItemSelect += OnShopItemSelect;
		}

		private void OnDisable()
		{
			MetaEvents.ShopItemSelect -= OnShopItemSelect;
		}

		private void Start()
		{
			if (outfitMeshes.bottoms.Count == 0 || outfitMeshes.tops.Count == 0 || outfitMeshes.hair.Count == 0 || outfitMeshes.shoes.Count == 0)
			{
				Debug.LogError("Some/All Lists not initialised.");
				return;
			}

			GetCurrentApparel();
			RefreshApparel();
			var temp = 0f;

			DOTween.To(() => temp, value => temp = value, 111, 12);
		}

		private void RefreshApparel()
		{
			outfits[currentOutfit].SetOutfit(ref outfitMeshes);
			EnableOnlyIndexInList(ref hats, currentHat);
			EnableOnlyIndexInList(ref masks, currentMask);
		}
		
		private void RefreshOutfit() => outfits[currentOutfit].SetOutfit(ref outfitMeshes);

		private void GetCurrentApparel()
		{
			var currentOutfitStates = ShopStateController.CurrentState.GetItemStates(ShopCategory.Outfit);
			var currentHatStates = ShopStateController.CurrentState.GetItemStates(ShopCategory.Hat);
			var currentMaskStates = ShopStateController.CurrentState.GetItemStates(ShopCategory.Mask);

			var i = -1;
			foreach (var outfit in currentOutfitStates)
			{
				i++;
				if (outfit.Value != ShopItemState.Selected) continue;

				currentOutfit = i;
				break;
			}

			i = -1;
			foreach (var hat in currentHatStates)
			{
				i++;
				if (hat.Value != ShopItemState.Selected) continue;

				currentHat = i - 1;
				break;
			}
			
			i = -1;
			foreach (var mask in currentMaskStates)
			{
				i++;
				if (mask.Value != ShopItemState.Selected) continue;

				currentMask = i - 1;
				break;
			}
		}

		private void OnShopItemSelect(ShopCategory shopCategory, int index, bool _)
		{
			switch (shopCategory)
			{
				case ShopCategory.Outfit:
					currentOutfit = index;
					RefreshOutfit();
					break;
				case ShopCategory.Mask:
					EnableOnlyIndexInList(ref masks, index - 1);
					break;
				case ShopCategory.Hat:
					EnableOnlyIndexInList(ref hats, index - 1);
					SortOutHairVisibility(index);
					break;
				default:
					throw new ArgumentOutOfRangeException(nameof(shopCategory), shopCategory, null);
			}
		}

		private void SortOutHairVisibility(int index)
		{
			foreach (var hair in hairObjectsToTurnOff) hair.SetActive(!hatsToTurnOffHairIn[index]);
		}

		public static void EnableOnlyIndexInList(ref List<GameObject> list, int index) { for (var i = 0; i < list.Count; i++) list[i].SetActive(i == index); }
	}
}