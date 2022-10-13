using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChatController : MonoBehaviour
{
	[SerializeField] private Button positiveButton, negativeButton;
	[SerializeField] private TextMeshProUGUI positiveButtonText, negativeButtonText;
	
	[Header("Chat bubble parents"), SerializeField] private Transform contact;
	[SerializeField] private Transform player;

	[SerializeField] private List<ChatResponse> chatResponses;
	private int _totalBubbles, _currentBubble, _currentChatResponse;

	private void OnEnable() => PatternMoneyEvents.CompletePatternStage += OnCompletePatternStage;

	private void OnDisable() => PatternMoneyEvents.CompletePatternStage -= OnCompletePatternStage;

	private void Start()
	{
		_totalBubbles = Mathf.Max(contact.childCount, player.childCount);
		UpdateResponseButtons();
	}

	public void ProgressChatPositive()
	{
		if(AudioManager.instance)
			AudioManager.instance.Play("Button");
		DotAndShowText(true);
		Vibration.Vibrate(100);

	}

	public void ProgressChatNegative()
	{
		if(AudioManager.instance)
			AudioManager.instance.Play("Button");
		DotAndShowText(false);
		Vibration.Vibrate(100);

	}

	private void DotAndShowText(bool isPositive)
	{
		DisableButtons();
		if (_currentBubble >= _totalBubbles) return;

		var seq = DOTween.Sequence();
		
		//enabling 2 children because there is a dummy transparent GameObject after real one which is 
		//the height of the reply bubble
		for (var i = 0; i < 2; i++)
		{
			var item = player.GetChild(_currentBubble);
			if (item) AddToChatSequence(item, ref seq,
				isPositive, true, _currentChatResponse);

			item = contact.GetChild(_currentBubble + 1);
			if (item) AddToChatSequence(item, ref seq,
				isPositive, false, _currentChatResponse);

			_currentBubble++;
		}

		if (!isPositive)
			seq.Append(DOVirtual.DelayedCall(1f, () => GameCanvas.game.MakeGameResult()));
		else
			seq.AppendCallback(() => { if(UpdateResponseButtons()) EnableButtons(); });

		_currentChatResponse++;
	}

	private void AddToChatSequence(Transform target, ref Sequence seq, bool isPositive, bool isPlayer, int currentChatResponse)
	{
		if(!target)
		{
			print("target is null");
			return;
		}
		
		seq.AppendInterval(0.25f);
		seq.AppendCallback(() => target.gameObject.SetActive(true));

		if (target.childCount == 0) return;
		seq.AppendCallback(() => target.GetComponentInChildren<TextMeshProUGUI>().text = "...");
		seq.AppendInterval(1.5f);
		seq.AppendCallback(() => target.GetComponentInChildren<TextMeshProUGUI>().text = isPlayer
			? GetNextPlayerLine(isPositive, currentChatResponse)
			: GetNextContactLine(isPositive, currentChatResponse));
			
		seq.AppendInterval(1f);
	}

	private bool UpdateResponseButtons()
	{
		if (_currentChatResponse >= chatResponses.Count)
		{
			DisableButtons();
			
			GameEvents.InvokeGameWin();
			return false;
		}
		
		positiveButtonText.text = chatResponses[_currentChatResponse].GetButtonText(true);
		negativeButtonText.text = chatResponses[_currentChatResponse].GetButtonText(false);
		return true;
	}

	private void DisableButtons()
	{
		positiveButton.interactable = negativeButton.interactable = false;
		positiveButtonText.text = negativeButtonText.text = "...";
	}

	private void EnableButtons(float inSecs = 0)
	{
		if (inSecs < 0.001f)
		{
			positiveButton.interactable = negativeButton.interactable = true;
			return;
		}
		
		DOVirtual.DelayedCall(inSecs, 
			() => positiveButton.interactable = negativeButton.interactable = true);
	}

	private string GetNextPlayerLine(bool isPositive, int chatResponse) => chatResponses[chatResponse].GetPlayerReply(isPositive);

	private string GetNextContactLine(bool isPositive, int chatResponse) => chatResponses[chatResponse].GetContactReply(isPositive);

	private void OnCompletePatternStage() => UpdateResponseButtons();
}

[System.Serializable]
public struct ChatResponse
{
	[SerializeField] private string positiveButtonText, negativeButtonText;
	[SerializeField] private string positivePlayerReply, negativePlayerReply;
	[SerializeField] private string positiveContactReply, negativeContactReply;

	public string GetButtonText(bool isPositive) => isPositive ? positiveButtonText : negativeButtonText;
	public string GetPlayerReply(bool isPositive) => isPositive ? positivePlayerReply : negativePlayerReply;
	public string GetContactReply(bool isPositive) => isPositive ? positiveContactReply : negativeContactReply;
}