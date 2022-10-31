using DG.Tweening;
using UnityEngine;

public class MovingTrain : MonoBehaviour
{
	[SerializeField] private Transform trainEndPosition;
	[SerializeField] private float travelDuration;

	private void OnEnable()
	{
		GameEvents.TapToPlay += OnTapToPlay;
	}

	private void OnDisable()
	{
		GameEvents.TapToPlay -= OnTapToPlay;
	}

	private void Start()
	{
		
	}

	private void OnTapToPlay()
	{
		transform.DOMove(trainEndPosition.position, travelDuration).SetEase(Ease.Linear);
	}
}
