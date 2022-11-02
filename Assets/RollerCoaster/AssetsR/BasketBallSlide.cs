using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class BasketBallSlide : MonoBehaviour
{
	[SerializeField] private List<GameObject> balls;
	[SerializeField] private List<Transform> destinationsList;
	[SerializeField] private Transform startPosition;
	[SerializeField] private Transform finalDestination;

	[SerializeField] private float moveDuration = 0.05f;
	[SerializeField] private float delayTime = 0.1f;
	[SerializeField] private float restartTime = 1f;

	private void Start()
	{
		Sequence mySequence = DOTween.Sequence();

		foreach (var ball in balls)
		{
			mySequence.AppendInterval(delayTime);
			mySequence.AppendCallback(() => Slide(ball));
		}

		mySequence.AppendInterval(restartTime);
		mySequence.SetLoops(-1);
	}

	private void Slide(GameObject ball)
	{
		ball.transform.position = startPosition.position;
		ball.SetActive(true);
		Sequence myMovementSequence = DOTween.Sequence();
		foreach (var destination in destinationsList)
		{
			myMovementSequence.Append(ball.transform.DOMove(destination.position, moveDuration)
				.SetEase(Ease.Linear));
		}

		myMovementSequence.AppendCallback(()=>MoveToFinalDestination(ball));
	}

	private void MoveToFinalDestination(GameObject ball)
	{
		ball.transform.DOMove(finalDestination.position, moveDuration * 3).SetEase(Ease.Linear)
			.OnComplete(() =>
		{
			ball.SetActive(false);
		});
	}
}
