using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Cannon : MonoBehaviour
{
	[SerializeField] private Transform bounceTarget;
	[SerializeField] private Transform cannonMouth;
	[SerializeField] private Transform secondDestination;
	
	[SerializeField] private List<GameObject> cannonBalls;

	[SerializeField] private float jumpPower = 3f, jumpDuration = 1f;
	[SerializeField] private int numberOfJumps = 1;
	[SerializeField] private float delayInterval = 0.1f;
	
    private void Start()
    {
        ShootSimultaneously();
    }

	private void ShootSimultaneously()
	{
		Sequence mySequence = DOTween.Sequence();
		foreach (var ball in cannonBalls)
		{
			mySequence.AppendInterval(delayInterval);
			mySequence.AppendCallback(()=>ShootBall(ball));
		}

		mySequence.AppendInterval(2f);
		mySequence.SetLoops(-1);
	}

	private void ShootBall(GameObject ball)
	{
		ball.transform.position = cannonMouth.position;
		ball.SetActive(true);
		ball.transform.DOJump(bounceTarget.position, jumpPower, numberOfJumps, jumpDuration).SetEase(Ease.Linear).
			OnComplete(()=>
			{
				ball.transform.DOJump(secondDestination.position, jumpPower, numberOfJumps, jumpDuration).SetEase(Ease.Linear).
					OnComplete(()=>ball.SetActive(false));
			});
	}
}
