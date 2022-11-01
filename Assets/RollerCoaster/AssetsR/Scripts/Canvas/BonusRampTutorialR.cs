using DG.Tweening;
using UnityEngine;

public class BonusRampTutorialR : MonoBehaviour
{
	[SerializeField] private TMPro.TextMeshProUGUI instructionText;
	[SerializeField] private float scalingSize, scalingDuration;

	private void OnEnable()
	{
		GameEventsR.ReachEndOfTrack += OnReachEndOfTrack;
		GameEventsR.RunOutOfPassengers += OnReachEndOfBonusRamp;
	}

	private void OnDisable()
	{
		GameEventsR.ReachEndOfTrack -= OnReachEndOfTrack;
		GameEventsR.RunOutOfPassengers -= OnReachEndOfBonusRamp;
	}

	private void ShowInstructions()
	{
		instructionText.gameObject.SetActive(true);
		instructionText.transform.DOScale(Vector3.one * scalingSize, scalingDuration).SetLoops(-1, LoopType.Yoyo);
	}
	
	private void HideInstructions()
	{
		instructionText.gameObject.SetActive(false);
		DOTween.Kill(instructionText.transform);
	}

	private void OnReachEndOfTrack() => ShowInstructions();

	private void OnReachEndOfBonusRamp() => HideInstructions();
}