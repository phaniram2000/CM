using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Meta
{
	public enum ShopItemState { Locked, Unlocked, Selected, RejectedInLoader }

	//public class ShopItem : MonoBehaviour, IWantsAds
	public class ShopItem : MonoBehaviour
	{
		[SerializeField] private ShopItemState myState;
		[SerializeField] private Image icon, selected, textUnderlay;
		[SerializeField] private TextMeshProUGUI costText;
		[SerializeField] private Color cantBuyColor;

		[SerializeField] private GameObject stickyCost, unlockedText;
		//[SerializeField] private GameObject stickyCost, stickyAds;
		
		private static bool _canClickOnItem = true;
		private const float CanTouchCooldownTimer = 0.25f;

		private ShopCategory _myCategory;
		private int _myIndex;
		private bool _isAvailable;

		[SerializeField] private float punchScale, shakeStrength, punchScaleDuration;
		//All things ad related are kept downstairs

		public void SetIconSprite(Sprite image)
		{
			if (image)
			{
				icon.sprite = image;
				icon.color = Color.white;
				icon.enabled = true;
			}
			else
				icon.enabled = false;
		}

		public void SetState(ShopItemState state)
		{
			myState = state;
			switch (myState)
			{
				case ShopItemState.Locked or ShopItemState.RejectedInLoader:
					selected.enabled = false;
					unlockedText.SetActive(false);
					textUnderlay.enabled = true;
					//sticky cost or sticky ads will be turned on according to availability, already in the loop of the calling function
					break;
				case ShopItemState.Unlocked:
					selected.enabled = false;
					unlockedText.SetActive(true);
					textUnderlay.enabled = true;
					//stickyAds.SetActive(false);
					break;
				case ShopItemState.Selected:
					selected.enabled = true;
					unlockedText.SetActive(false);
					textUnderlay.enabled = false;
					//stickyAds.SetActive(false);
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
		}

		public void SetSkinIndex(int idx) => _myIndex = idx;

		public void SetIsCharacterItem(ShopCategory category) => _myCategory = category;

		public void SetPriceAndAvailability(int price)
		{
			if (myState is ShopItemState.Selected or ShopItemState.Unlocked)
			{
				// if the item is already owned
				costText.gameObject.SetActive(false);
				costText.color = Color.white;
				stickyCost.SetActive(false);
				return;
			}

			costText.text = price.ToString();
			_isAvailable = CheckAvailability(price);

			//stickyAds.SetActive(!_isAvailable && (YcHelper.InstanceExists && YcHelper.IsAdAvailable()));
			//stickyAds.SetActive(!_isAvailable && (ApplovinManager.instance && ApplovinManager.instance.enableAds));
			
			costText.color = _isAvailable ? Color.white : cantBuyColor;
		}

		private static bool CheckAvailability(int price) => price <= ShopStateController.CurrentState.GetBankBalance;

		public void ClickOnButton()
		{
			switch (myState)
			{
				case ShopItemState.Locked:
					ClickOnLocked();
					break;
				case ShopItemState.Unlocked or ShopItemState.RejectedInLoader:
					ClickOnUnlocked();
					break;
				case ShopItemState.Selected or ShopItemState.RejectedInLoader:
					PositiveFeedback();
					break;
				default:
					break;
			}
		}

		private void ClickOnLocked()
		{
			if (myState is not ShopItemState.Locked or ShopItemState.RejectedInLoader) return;

			if (!_canClickOnItem) return;

			_canClickOnItem = false;
			DOVirtual.DelayedCall(CanTouchCooldownTimer, () => _canClickOnItem = true);

			AudioManager.instance.Play("Button");
			//confetti and/or power up vfx

			if (_isAvailable)
			{
				PositiveFeedback();
				MetaEvents.InvokeShopItemSelect(_myCategory, _myIndex, true);
				return;
			}
			
			NegativeFeedback();
			
			/*
			if (!ApplovinManager.instance) return;
			if (!ApplovinManager.instance.enableAds) return;
		
			if (!ApplovinManager.instance.TryShowRewardedAds()) return;

			AdsMediator.StartListeningForAds(this);
		
			//if (!YcHelper.InstanceExists || !YcHelper.IsAdAvailable()) return;			
			//YcHelper.ShowRewardedAds(AdRewardReceiveBehaviour);
			*/
		}

		private void ClickOnUnlocked()
		{
			if (!_canClickOnItem) return;

			_canClickOnItem = false;
			DOVirtual.DelayedCall(CanTouchCooldownTimer, () => _canClickOnItem = true);

			MetaEvents.InvokeShopItemSelect(_myCategory, _myIndex, false);
			
			if(AudioManager.instance)
				AudioManager.instance.Play("Button");
			PositiveFeedback();
		}
		
		private void PositiveFeedback()
		{
			DOTween.Kill(transform, true);
			transform.DOPunchScale(Vector3.one * punchScale, punchScaleDuration);
		}

		private void NegativeFeedback()
		{
			DOTween.Kill(transform, true);
			((RectTransform)transform).DOShakeAnchorPos(punchScaleDuration, Vector3.up * shakeStrength, 100);
		}
		
		/*
		private void AdRewardReceiveBehaviour()
		{
			if (_isWeaponItem)
				GameEvents.Only.InvokeWeaponSelect(_mySkinIndex, false);
			else
				GameEvents.Only.InvokeSkinSelect(_mySkinIndex, false);

			AdsMediator.StopListeningForAds(this);
		}

		public void OnAdRewardReceived(string adUnitId, MaxSdkBase.Reward reward, MaxSdkBase.AdInfo adInfo) => AdRewardReceiveBehaviour();

		public void OnShowDummyAd() => AdRewardReceiveBehaviour();

		public void OnAdFailed(string adUnitId, MaxSdkBase.ErrorInfo errorInfo, MaxSdkBase.AdInfo adInfo) => AdsMediator.StopListeningForAds(this);

		public void OnAdFailedToLoad(string adUnitId, MaxSdkBase.ErrorInfo errorInfo) => AdsMediator.StopListeningForAds(this);

		public void OnAdHidden(string adUnitId, MaxSdkBase.AdInfo adInfo) => AdsMediator.StopListeningForAds(this);		
		*/
	}
}