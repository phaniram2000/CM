using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PasscodeCanvas : MonoBehaviour
{
	[SerializeField] private string passcode;
	[SerializeField] private List<Image> passcodeBubbles;
	[SerializeField] private Sprite unlockedSprite;
	[SerializeField] private float bubblePunchScale, bubblePunchDuration, bubbleShakeStrength;
	
	[Header("Hints"), SerializeField] private List<string> hints;
	[SerializeField] private TextMeshProUGUI hintText;

	private int _currentBubble = 0;
	private string _attemptedPasscode = "";

	private const int MaxPasscodeLength = 4;
	
	private void OnValidate()
	{
		if(!Application.isEditor) return;
		if(Application.isPlaying) return;

		if (passcode.Length > MaxPasscodeLength) passcode = passcode.Remove(4, passcode.Length - MaxPasscodeLength);
		for (var i = 0; i < MaxPasscodeLength; i++)
		{
			if (char.IsDigit(passcode[i])) continue;

			passcode = passcode.Remove(i--);
		}
	}

	private void Start() => UpdateHintText();

	private bool TrySetBubble(int input)
	{
		if (int.Parse(passcode[_currentBubble].ToString()) != input)
		{
			passcodeBubbles[_currentBubble].transform.DOShakePosition(bubblePunchDuration, Vector3.up * bubbleShakeStrength, 100);
			passcodeBubbles[_currentBubble].DOColor(Color.red, bubblePunchDuration / 2).SetLoops(2, LoopType.Yoyo);
			return false;
		}

		passcodeBubbles[_currentBubble].transform.DOPunchScale(Vector3.one * bubblePunchScale, bubblePunchDuration);
		passcodeBubbles[_currentBubble].DOColor(Color.green, bubblePunchDuration / 2).SetLoops(2, LoopType.Yoyo);
		passcodeBubbles[_currentBubble].sprite = unlockedSprite;
		return true;
	}

	private void UpdateHintText()
	{
		hintText.transform.DOPunchScale(Vector3.one * bubblePunchScale, bubblePunchDuration);
		hintText.text =
			_currentBubble >= MaxPasscodeLength
				? hints[_currentBubble]
				: "Hint:\n" + hints[_currentBubble];
	}

	private bool IsDone() => string.CompareOrdinal(_attemptedPasscode, passcode) == 0;

	public void PressButton(int input)
	{
		if(AudioManager.instance)
			AudioManager.instance.Play("Button");
		
		Vibration.Vibrate(100);
		
		
		if(_currentBubble >= MaxPasscodeLength) return;

		if (!TrySetBubble(input)) return;

		_currentBubble++;
		_attemptedPasscode += input;
		UpdateHintText();
		
		if (_attemptedPasscode.Length != MaxPasscodeLength || !IsDone()) return;

		DOVirtual.DelayedCall(0.5f, PatternMoneyEvents.InvokeCompletePatternStage);
		DOVirtual.DelayedCall(1, () =>
		{
			if(AudioManager.instance)
				AudioManager.instance.Play("Swipe");
		});
		
	}
}