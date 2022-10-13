using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameCanvas : MonoBehaviour
{
	public static GameCanvas game;
	
	[SerializeField] private Transform warningTransform, instructionsTransform;
	[SerializeField] private Button doneButton, clearButton;
	[SerializeField] private TextMeshProUGUI resultText, tapToPlayText;

	[SerializeField] private Color preDrawTextColor;
	[SerializeField] private float moveDuration, warningStayDuration;
	[SerializeField] private float tapToPlayDelay = 0f;
	
	[Header("Ghost Transforms"), SerializeField]
	private Transform instructionsHidden;

	[SerializeField] private Transform instructionsVisible,
		warningHidden, warningVisible,
		resultHidden, resultVisible,
		doneHidden, doneVisible,
		clearHidden, clearVisible;

	private bool _isWarningVisible, _isClearVisible;
	private string _initResultText;
	private int _lastResult;

	private void OnEnable()
	{
		GameEvents.PreDraw += OnPreDraw;
		
		if (GameRules.GetGameMode is GameMode.Pub or GameMode.Bank or GameMode.PriceTag)
			PubEvents.StartInput += ShowDoneButton;
		else if(GameRules.GetGameMode is GameMode.Toilet or GameMode.Classroom)
				print("In toilet");
		else
			GameEvents.TapToPlay += ShowDoneButton;

		GameEvents.TapToPlay += OnTapToPlay;
		GameEvents.GameWin += OnGameWin;
		GameEvents.GameLose += OnGameLose;

		GameEvents.ShowDoneButton += ShowDoneButton;
		GameEvents.ShowDoneButton += EnableDoneIntreacteable;
	}

	private void OnDisable()
	{
		GameEvents.PreDraw -= OnPreDraw;
		
		if (GameRules.GetGameMode is GameMode.Pub or GameMode.Bank or GameMode.PriceTag)
			PubEvents.StartInput -= ShowDoneButton;
		else if(GameRules.GetGameMode is GameMode.Toilet or GameMode.Classroom)
				print("In toilet");
		else
			GameEvents.TapToPlay -= ShowDoneButton;
		
		GameEvents.TapToPlay -= OnTapToPlay;
		GameEvents.GameWin -= OnGameWin;
		GameEvents.GameLose -= OnGameLose;
		
		GameEvents.ShowDoneButton -= ShowDoneButton;
		GameEvents.ShowDoneButton -= EnableDoneIntreacteable;
	}

	private void Awake()
	{
		if (!game) game = this;
		else Destroy(gameObject);
	}

	private void Start()
	{
		_initResultText = resultText.text;
		ShowInstructions();
		clearButton.gameObject.SetActive(GameRules.GetRuleSet != null && GameRules.GetRuleSet.CanResetInput);
	}

	public void SetRecognisedNumber(int result)
	{
		_lastResult = result;

		string str;

		switch (GameRules.GetGameMode)
		{
			case GameMode.Pub:
				str = result > 0 ? result + "8" : "???";
				break;
			case GameMode.Classroom:
				str = result > 0 ? result + "5" : "???";
				break;
			case GameMode.PriceTag:
			default:
				str = result.ToString();
				break;
		}

		if(GameRules.GetRuleSet.GetHelperBase)
			SetResultText(str, GameRules.GetRuleSet.GetHelperBase.ResultTextFormatter());
		else
			SetResultText(str, null);
		
		ShowDoneButton();
		ShowClearButton();
		ShowResultPreview();
	}

	private void SetResultText(string defaultString, Func<int, string> delegatedResultText = null)
	{
		if (delegatedResultText == null)
		{
			resultText.text = _initResultText.Replace("abc", defaultString);
			return;
		}

		resultText.text = delegatedResultText(_lastResult);
	}

	public void PressDoneButton()
	{
		doneButton.interactable = false;
		if(GameRules.GetGameMode == GameMode.Classroom && _lastResult < 0)
		{
			if(AudioManager.instance)
				AudioManager.instance.Play("Cancel");
			Vibration.Vibrate(15);
			return;
		}
		if(GameRules.GetGameMode == GameMode.Toilet && !ToiletHelper.GetAreSignsSwapped)
		{
			if(AudioManager.instance)
				AudioManager.instance.Play("Cancel");
			Vibration.Vibrate(15);
			return;
		}
		
		HideClearButton();
		HideDoneButton();
		HideInstructions();
		HideResultPreview();
		GameEvents.InvokePressDoneButton();

		if(AudioManager.instance)
			AudioManager.instance.Play("Button");
		
		switch (GameRules.GetGameMode)
		{
			case GameMode.Toilet:
				ToiletEvents.InvokeDoneWithInput();
				return;
			case GameMode.Pub:
				PubEvents.InvokeDoneWithInput();
				return;
			case GameMode.Bank:
				BankEvents.InvokeDoneWithInput();
				return;
			case GameMode.PriceTag:
				PricetagEvents.InvokeDoneWithInput();
				return;
			case GameMode.Jackpot: return;
			case GameMode.Classroom:
			case GameMode.PhoneUnlockPattern:
			case GameMode.Cheat:
				//continue with execution
				break;
			default:
				throw new ArgumentOutOfRangeException();
		}
		
		MakeGameResult();
		Vibration.Vibrate(15);
	}

	public void PressClearButton()
	{
		if(!GameRules.GetRuleSet.CanResetInput)
		{
			if(AudioManager.instance)
				AudioManager.instance.Play("Cancel");
			Vibration.Vibrate(15);
			return;
		}

		if(AudioManager.instance)
			AudioManager.instance.Play("Clear");
		
		GameRules.GetRuleSet.TryResetInput();
		HideClearButton();
		HideResultPreview();
		SetRecognisedNumber(-1);
		
		clearButton.interactable = false;
		DOVirtual.DelayedCall(0.5f, () => EnableDoneIntreacteable());
		DOVirtual.DelayedCall(0.5f, () => clearButton.interactable = true);
		Vibration.Vibrate(15);
	}

	
	//This done specifi to solve clasrrom done button bug,look for it in future.
	private void EnableDoneIntreacteable()
	{
		doneButton.interactable = true;
	}

	/// <summary>
	/// Feed in custom input here for levels that require feeding in custom input to CheckGameResult
	/// Feed in custom result here for directly announcing the game results
	/// and not referring to any IRuleSet, -1 underconfident loss, 0 win, 1 overconfident loss
	/// </summary>
	/// <param name="customInput"></param>
	/// <param name="customResult"></param>
	public void MakeGameResult(int customInput = 500, int customResult = -500)
	{
		var result = customResult;
		
		if(result == -500)
			result = GameRules.GetRuleSet.
				CheckGameResult(customInput != 500 ? customInput : _lastResult);

		switch (result)
		{
			case 0:
				GameEvents.InvokeGameWin();
				break;
			case -1:
				GameEvents.InvokeGameLose(-1);
				break;
			case 1:
				GameEvents.InvokeGameLose(1);
				break;
		}

		doneButton.interactable = false;
		DOVirtual.DelayedCall(0.5f, () => doneButton.interactable = true);
	}

	public int CheckGameResult(int input) => GameRules.GetRuleSet.CheckGameResult(input);

	public int GetLastResult() => _lastResult;

	private void ShowInstructions() => ShowTarget(instructionsTransform, instructionsVisible.position);

	private void HideInstructions() => HideTarget(instructionsTransform, instructionsHidden.position);

	private void ShowResultPreview() => ShowTarget(resultText.transform.parent, resultVisible.position);

	private void HideResultPreview() => HideTarget(resultText.transform.parent, resultHidden.position);

	private void ShowDoneButton() => ShowTarget(doneButton.transform, doneVisible.position);

	private void HideDoneButton() => HideTarget(doneButton.transform, doneHidden.position);

	//Auto Hides

	private void ShowWarning()
	{
		if(_isWarningVisible) return;
		
		_isWarningVisible = true;

		warningTransform.DOMoveY(warningVisible.position.y, moveDuration)
			.SetEase(Ease.OutElastic)
			.OnComplete(() =>
				warningTransform.DOMoveY(warningHidden.position.y, moveDuration)
					.SetEase(Ease.InElastic)
					.SetDelay(warningStayDuration)
					.OnComplete(() => _isWarningVisible = false));
	}

	private void ShowClearButton()
	{
		if(_isClearVisible) return;
		
		_isClearVisible = true;
		ShowTarget(clearButton.transform, clearVisible.position);
	}

	private void HideClearButton()
	{
		if(!_isClearVisible) return;
		
		_isClearVisible = false;
		ShowTarget(clearButton.transform, clearHidden.position);
	}

	private void HideTarget(Transform target, Vector3 position) => target.DOMove(position, moveDuration).SetEase(Ease.InElastic);

	private void ShowTarget(Transform target, Vector3 position) => target.DOMove(position, moveDuration).SetEase(Ease.OutElastic);

	private void OnGameEnd()
	{
		HideInstructions();
		HideClearButton();
		HideDoneButton();

		DOTween.Kill(warningTransform, true);
	}

	private void OnPreDraw()
	{
		tapToPlayText.DOColor(preDrawTextColor, 0.5f).SetLoops(-1, LoopType.Yoyo);
		tapToPlayText.transform.DOScale(0, 0.5f)
			.SetLoops(2, LoopType.Yoyo)
			.OnStepComplete(() => tapToPlayText.text = "TAP TO CONTINUE");
	}

	private void OnTapToPlay()
	{
		//HideInstructions();
		tapToPlayText.transform.DOScale(tapToPlayText.transform.localScale * 1.15f, 0.25f).SetLoops(2, LoopType.Yoyo);
		
		if (tapToPlayDelay > 0f)
			tapToPlayText.DOColor(Color.clear, tapToPlayDelay * 0.6f)
				.SetDelay(tapToPlayDelay * 0.4f)
				.SetEase(Ease.InExpo)
				.OnComplete(() => tapToPlayText.gameObject.SetActive(false));
		else
			tapToPlayText.gameObject.SetActive(false);

		if(AudioManager.instance)
			AudioManager.instance.Play("TapToPlay");
	}

	private void OnGameWin() => OnGameEnd();

	private void OnGameLose(int _) => OnGameEnd();
}