using DG.Tweening;
using TMPro;
using UnityEngine;

public class BlackmailConversation : MonoBehaviour
{

	[SerializeField] private GameObject[] couplesResponses;
	[SerializeField] private GameObject coupleConversationBubble;

	[SerializeField] private GameObject blackmailConversationUi;
	[SerializeField] private GameObject initialConversationPanel;
	[SerializeField] private GameObject secondConversationPanel;
	[SerializeField] private GameObject picsThreatPanel;
	[SerializeField] private GameObject leakingInformationThreat;
	[SerializeField] private GameObject moneyThreatPanel;
	[SerializeField] private GameObject money;
	
	private Sequence _myFirstSeq;
	
	private void OnEnable()
	{
		BlackmailingEvents.ToNextGamePhase += FirstBlackmailingRoutine;
	}

	private void OnDisable()
	{
		BlackmailingEvents.ToNextGamePhase -= FirstBlackmailingRoutine;
	}

	public void LookWhatIHaveFound()
	{
		//initialConversationPanel.SetActive(false);
		//couplesResponses[0].SetActive(false);
		Sequence mySeq = DOTween.Sequence();
		mySeq.AppendCallback(() => initialConversationPanel.SetActive(false));
		mySeq.AppendCallback(() => secondConversationPanel.SetActive(false));
		mySeq.AppendCallback(() => couplesResponses[0].SetActive(false));
		//mySeq.AppendCallback(() => couplesResponses[1].SetActive(true));
		mySeq.AppendInterval(2f);
		mySeq.AppendCallback(()=>picsThreatPanel.SetActive(true));

		if (AudioManager.instance)
		{
			AudioManager.instance.Play("Button");
			AudioManager.instance.Play("LookWhatIHaveFound");
			DOVirtual.DelayedCall(1.5f, () =>
			{
				couplesResponses[1].SetActive(true);
				AudioManager.instance.Play("WhatDoYouHave");
			});
		}
		
		Vibration.Vibrate(30);
	}

	public void NoneOfYourBusiness()
	{
		DisableAllConvoCanvas();
		GameCanvas.game.MakeGameResult(1,1);
		BlackmailingEvents.InvokeFinalLose();
		DOVirtual.DelayedCall(1.5f,BlackmailingEvents.InvokeSayNo);
		Vibration.Vibrate(30);
		
		if(AudioManager.instance)
			AudioManager.instance.Play("WantToJoin");
	}

	public void PositivePic()
	{
		couplesResponses[1].SetActive(false);
		
		Sequence mySeq = DOTween.Sequence();
		mySeq.AppendCallback(()=>picsThreatPanel.SetActive(false));
		//mySeq.AppendInterval(1f);
		//mySeq.AppendCallback(() => couplesResponses[2].SetActive(true));
		mySeq.AppendInterval(3f);
		mySeq.AppendCallback(() => leakingInformationThreat.SetActive(true));

		if (AudioManager.instance)
		{
			AudioManager.instance.Play("Button");
			couplesResponses[2].SetActive(true);
			AudioManager.instance.Play("WhatDoYouPlan");
		}
		Vibration.Vibrate(30);
	}

	public void NegativePic()
	{
		DisableAllConvoCanvas();
		GameCanvas.game.MakeGameResult(1,1);
		BlackmailingEvents.InvokeFinalLose();
		BlackmailingEvents.InvokeSayNo();
		
		if (AudioManager.instance)
			AudioManager.instance.Play("Button");
		
		Vibration.Vibrate(30);

	}

	public void WillSendToYourWife()
	{
		couplesResponses[2].SetActive(false);

		Sequence mySeq = DOTween.Sequence();
		mySeq.AppendCallback(()=>leakingInformationThreat.SetActive(false));
		mySeq.AppendCallback(() =>
		{
			couplesResponses[3].SetActive(true);
		});
		mySeq.AppendInterval(3f);
		mySeq.AppendCallback(() => moneyThreatPanel.SetActive(true));

		if (AudioManager.instance)
		{
			AudioManager.instance.Play("Button");
			AudioManager.instance.Play("WillSendItToYourWife");
			DOVirtual.DelayedCall(1.5f, () => AudioManager.instance.Play("NoPlease"));
		}
		Vibration.Vibrate(30);

	}

	public void WillSendToYou()
	{
		DisableAllConvoCanvas();
		GameCanvas.game.MakeGameResult(1,1);
		BlackmailingEvents.InvokeFinalLose();
		BlackmailingEvents.InvokeSayNo();
		Vibration.Vibrate(30);

		if (AudioManager.instance)
		{
			AudioManager.instance.Play("Button");
			AudioManager.instance.Play("WillSendItToYou");
		}
	}

	public void MoreMoney()
	{
		moneyThreatPanel.SetActive(false);
		couplesResponses[3].SetActive(false);
		
		Sequence mySeq = DOTween.Sequence();
		mySeq.AppendCallback(() => couplesResponses[4].SetActive(true));
		mySeq.AppendCallback(()=> DOVirtual.DelayedCall(5f,SpawnMoney));
		mySeq.AppendCallback(BlackmailingEvents.InvokeFinalWin);
		mySeq.AppendInterval(3f);
		mySeq.AppendCallback(DisableAllConvoCanvas);
		mySeq.AppendInterval(1.5f);
		mySeq.AppendCallback(() => GameCanvas.game.MakeGameResult(0, 0));

		if (AudioManager.instance)
		{
			AudioManager.instance.Play("Button");
			AudioManager.instance.Play("WantOneMillionDollars");
			DOVirtual.DelayedCall(1.5f, () => AudioManager.instance.Play("OkMoneyCredited"));
		}
		Vibration.Vibrate(30);
	}

	private void SpawnMoney()
	{
		money.SetActive(true);
		//money.transform.DOMoveY(money.transform.position.y + 1f, 1f).SetLoops(-1, LoopType.Yoyo);
		var lossyScale = money.transform.lossyScale;
		money.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
		money.transform.DOScale(lossyScale, 1f).SetEase(Ease.OutBounce);

		if (AudioManager.instance)
		{
			AudioManager.instance.Play("NiceFemale");
			AudioManager.instance.Play("Woosh");
		}
		Vibration.Vibrate(30);
	}
	public void LessMoney()
	{
		DisableAllConvoCanvas();
		GameCanvas.game.MakeGameResult(1,1);
		BlackmailingEvents.InvokeFinalLose();
		BlackmailingEvents.InvokeSayNo();
		Vibration.Vibrate(30);
		
		if (AudioManager.instance)
		{
			AudioManager.instance.Play("Button");
			AudioManager.instance.Play("Want10Dollars");
		}
	}
	
	public void FirstBlackmailingRoutine()
	{

		_myFirstSeq = DOTween.Sequence();
		print(_myFirstSeq);

		_myFirstSeq.AppendCallback(() => blackmailConversationUi.SetActive(true));
		_myFirstSeq.AppendCallback(() => initialConversationPanel.SetActive(true));
		_myFirstSeq.AppendInterval(3f);
		_myFirstSeq.AppendCallback(() => coupleConversationBubble.SetActive(true));
		_myFirstSeq.AppendCallback(() => couplesResponses[0].SetActive(true));
		_myFirstSeq.AppendInterval(2f);
		_myFirstSeq.AppendCallback(ShowSecondBlackMailPanel);

		if (AudioManager.instance)
		{
			AudioManager.instance.Play("WhatAreYouDoing");
			DOVirtual.DelayedCall(3f, () => AudioManager.instance.Play("NoneOfYourBusiness"));
		}
	}

	private void ShowSecondBlackMailPanel()
	{
		initialConversationPanel.SetActive(false);
		secondConversationPanel.SetActive(true);
	}

	private void DisableAllConvoCanvas()
	{
		blackmailConversationUi.SetActive(false);
		coupleConversationBubble.SetActive(false);

		if (AudioManager.instance)
		{
			AudioManager.instance.Play("Button");
			DOVirtual.DelayedCall(1.5f,()=>AudioManager.instance.Play("NoMale"));
		}
		Vibration.Vibrate(30);
	}
}
