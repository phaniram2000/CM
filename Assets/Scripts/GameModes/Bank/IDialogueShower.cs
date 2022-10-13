using DG.Tweening;
using TMPro;
using UnityEngine;

public interface IDialogueShower
{
	protected TextMeshPro DialogueText { get; }
	public Vector3 InitDialogueScale { get; }

	public Tween ShowDialogue(string text, float time, Vector3 initScale)
	{
		// uncomment this if you want to disable dialogues for video recording
		//return null;
		
		DialogueText.transform.parent.DOScale(initScale, 0.25f).SetEase(Ease.OutBack);
		DialogueText.text = text;
		return DOVirtual.DelayedCall(time, () => { })
			.OnComplete(() =>
			{
				DialogueText.transform.parent.DOScale(0, 0.25f).SetEase(Ease.InBack);
				DialogueText.text = "";
			});
	}
}