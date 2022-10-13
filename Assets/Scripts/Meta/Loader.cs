using System.Collections.Generic;
using DG.Tweening;
using Meta;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Loader : MonoBehaviour
{
	[SerializeField] private bool showDebugStrings;
	[SerializeField] private ShopCategory loaderCategory, secondCategory;
	
	[SerializeField] private Image blackBackground;

	[SerializeField] private Transform skinRayBeams;
	[SerializeField] private Button claimLoaderItemButton, skipLoaderItemButton;
	[SerializeField] private TextMeshProUGUI percentageUnlockedText, skipLoaderItemButtonText;
	
	[SerializeField] private Image coloredLoaderImage, blackLoaderImage;
	[SerializeField] private List<Sprite> coloredMaskSprites, blackMaskSprites;
	[SerializeField] private List<Sprite> coloredHatSprites, blackHatSprites;

	[Header("Settings"), SerializeField] private int levelsPerUnlock = 5;
	[SerializeField] private float tweenDuration, panelOpenWait;

	private MainCanvasController _mainCanvas;
	private Canvas _canvas;
	private float _currentSkinPercentageUnlocked;
	private bool _unlockedThisTime, _doneWithFirstCategory, _doneWithSecondCategory;
	
	private static int GetLoaderIndex => ShopStateController.CurrentState.GetLoaderIndex();

	private void OnEnable() => MetaEvents.ShopItemSelect += OnShopItemSelect;

	private void OnDisable() => MetaEvents.ShopItemSelect -= OnShopItemSelect;

	private void Start()
	{
		_canvas = GetComponent<Canvas>();
		_mainCanvas = GameObject.FindGameObjectWithTag("MainCanvas").GetComponent<MainCanvasController>();

		Initialise();
		
		skipLoaderItemButton.gameObject.SetActive(false);
		claimLoaderItemButton.interactable = false;
	}

	private void Initialise()
	{
		_doneWithFirstCategory = ShopStateController.CurrentState.GetState().CategoryStates[(int)loaderCategory].AreAllItemsUnlocked();
		_doneWithSecondCategory = ShopStateController.CurrentState.GetState().CategoryStates[(int)secondCategory].AreAllItemsUnlocked();
		
		_currentSkinPercentageUnlocked = PlayerPrefs.GetFloat("currentSkinPercentageUnlocked", 0f);
		
		if(_doneWithFirstCategory && _doneWithSecondCategory) return;
		if (_doneWithFirstCategory) loaderCategory = secondCategory;

		if(showDebugStrings)
		{
			print($"status 1st cat {_doneWithFirstCategory}, 2nd cat {_doneWithSecondCategory}");
			print(
				$"assigning {GetLoaderIndex}/{(_doneWithFirstCategory ? coloredHatSprites : coloredMaskSprites).Count} for current cat = {loaderCategory}");
		}
		coloredLoaderImage.sprite = (_doneWithFirstCategory ? coloredHatSprites : coloredMaskSprites)[GetLoaderIndex - 1];
		blackLoaderImage.sprite = (_doneWithFirstCategory ? blackHatSprites : blackMaskSprites)[GetLoaderIndex - 1];

		if ((int) (_currentSkinPercentageUnlocked * 100) >= 100)
			percentageUnlockedText.text = 100 + "%";
		else
			percentageUnlockedText.text = (int) (_currentSkinPercentageUnlocked * 100) + "%";

		blackLoaderImage.fillAmount = 1 - _currentSkinPercentageUnlocked;
	}

	public void ShowLoader()
	{
		if(_doneWithFirstCategory && _doneWithSecondCategory)
		{
			_mainCanvas.ShowWinPanelAfterLoader();
			return;
		}
			
		blackBackground.gameObject.SetActive(true);
		var color = blackBackground.color;
		blackBackground.color = Color.clear;
		blackBackground.DOColor(color, .75f);

		_canvas.enabled = true;
		
		var oldValue = _currentSkinPercentageUnlocked;
		_currentSkinPercentageUnlocked += 1 / (float) levelsPerUnlock;

		PlayerPrefs.SetFloat("currentSkinPercentageUnlocked", _currentSkinPercentageUnlocked);

		skinRayBeams.DORotate(Vector3.forward * 180, 4f)
			.SetLoops(-1, LoopType.Incremental)
			.SetEase(Ease.Linear);
		
		DOTween.To(() => blackLoaderImage.fillAmount, value => blackLoaderImage.fillAmount = value,
				1 - _currentSkinPercentageUnlocked, tweenDuration)
			.SetEase(Ease.OutCubic);

		DOTween.To(() => oldValue, value => oldValue = value, _currentSkinPercentageUnlocked, tweenDuration)
			.SetEase(Ease.OutCubic)
			.OnUpdate(() => percentageUnlockedText.text = (int) (oldValue * 100) + "%");

		claimLoaderItemButton.interactable = true;

		DOVirtual.DelayedCall(panelOpenWait, EnableSkipSkinButton);

		_unlockedThisTime = _currentSkinPercentageUnlocked > 0.99f; 
		if (_unlockedThisTime) return;
		
		claimLoaderItemButton.gameObject.SetActive(false);
		skipLoaderItemButtonText.text = "Continue";
	}

	public void PressGetSkinButton()
	{
		_canvas.enabled = false;
		ReceiveLoaderReward();
		if(AudioManager.instance)
			AudioManager.instance.Play("Button");
		Vibration.Vibrate(30);
	}

	public void PressSkipSkinButton()
	{
		_canvas.enabled = false;
		_mainCanvas.ShowWinPanelAfterLoader();
		if(AudioManager.instance) AudioManager.instance.Play("Button");
		Vibration.Vibrate(30);

		if (!_unlockedThisTime) return;

		ShopStateController.CurrentState.GetState().CategoryStates[(int)loaderCategory].ItemStates[GetLoaderIndex] = ShopItemState.RejectedInLoader;
		
		FindNewLoaderItem(GetLoaderIndex, false);
		ResetLoader();
	}

	private void ReceiveLoaderReward()
	{
		claimLoaderItemButton.interactable = false;
		skipLoaderItemButton.interactable = false;

		DOVirtual.DelayedCall(0.25f, _mainCanvas.ShowWinPanelAfterLoader);

		MetaEvents.InvokeShopItemSelect(loaderCategory, GetLoaderIndex,false);
		ResetLoader();
		Vibration.Vibrate(20);
	}

	private void FindNewLoaderItem(int currentIndex, bool hasUnlocked)
	{
		if(_doneWithFirstCategory && _doneWithSecondCategory) return;
		var changed = false;
		
		var a = (HatName)currentIndex;
		var b = (MaskName)currentIndex;

		if(showDebugStrings)
		{
			var st = "";
			st += _doneWithFirstCategory ? a : b;
			print($"loader finding new item from {currentIndex} which is {st}");

			st = _doneWithFirstCategory ? (a + 1).ToString() : (b + 1).ToString();
			print(
				$"will go from {currentIndex} which is {st}, to {ShopStateHelpers.GetCategoryItemCount(loaderCategory)}, current cat = {loaderCategory}");
		}

		//find a item from current index to last
		for (var i = currentIndex; i < ShopStateHelpers.GetCategoryItemCount(loaderCategory); i++)
		{
			var x = (HatName)i;
			var y = (MaskName)i;

			var str = "";
			str += _doneWithFirstCategory ? x : y;
			
			if(showDebugStrings)
				print($"{str} {ShopStateController.CurrentState.GetState().CategoryStates[(int) loaderCategory].ItemStates[i]} at {i}");
			if (ShopStateController.CurrentState.GetState().CategoryStates[(int) loaderCategory].ItemStates[i] != ShopItemState.Locked)
				continue;

			ShopStateController.CurrentState.SetNewLoaderIndex(i);
			changed = true;
			break;
		}

		if (changed) return;

		//if all items after me are unlocked, try to find new before me
		for (var i = 1; i < currentIndex; i++)
		{
			var x = (HatName)i;
			var y = (MaskName)i;

			var str = "";
			str += _doneWithFirstCategory ? x : y;
			
			if(showDebugStrings)
				print($"{str} {ShopStateController.CurrentState.GetState().CategoryStates[(int) loaderCategory].ItemStates[i]} at {i}");
			if (ShopStateController.CurrentState.GetState().CategoryStates[(int) loaderCategory].ItemStates[i] != ShopItemState.Locked)
				continue;

			ShopStateController.CurrentState.SetNewLoaderIndex(i);
			changed = true;
			break;
		}

		if (changed) return;
		
		switch (_doneWithFirstCategory)
		{
			case false:
				//if all items after me are unlocked, changeCategory and try again
				ShopStateController.CurrentState.GetState().CategoryStates[(int) loaderCategory].MarkAllItemsUnlocked();
				_doneWithFirstCategory = true;
				if(showDebugStrings)
					print($"done w masks");

				loaderCategory = secondCategory;
				ShopStateController.CurrentState.SetNewLoaderIndex(1);
				//FindNewLoaderItem(0, false);
				return;
			case true when !_doneWithSecondCategory:
				//if didn't find anything make sure loader isn't called anymore
				ShopStateController.CurrentState.GetState().CategoryStates[(int) loaderCategory].MarkAllItemsUnlocked();
				_doneWithSecondCategory = true;
				
				if(showDebugStrings)
					print($"done w hats");
				break;
		}
	}

	private void ResetLoader()
	{
		_currentSkinPercentageUnlocked = 0f;
		blackLoaderImage.fillAmount = 1 - _currentSkinPercentageUnlocked;

		PlayerPrefs.SetFloat("currentSkinPercentageUnlocked", _currentSkinPercentageUnlocked);
		
		Initialise();
	}
	
	private void EnableSkipSkinButton() => skipLoaderItemButton.gameObject.SetActive(true);

	private void OnShopItemSelect(ShopCategory category, int index, bool _)
	{
		if(showDebugStrings) print($"Buy something {index} w loader {GetLoaderIndex}");
		if(category != loaderCategory) return;
		
		if(showDebugStrings) print("Category matches");
		if(index != GetLoaderIndex) return;
		
		if(showDebugStrings) print("loader index matches");
		FindNewLoaderItem(index, false);
		ResetLoader();
	}
}
