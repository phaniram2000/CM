using DG.Tweening;
using UnityEngine;

public class CityAnimal : MonoBehaviour
{
	[SerializeField] private Transform startTransform;
	[SerializeField] private Transform endTransform;

	[SerializeField] private float walkDuration;
	
	private void Start()
	{
		transform.GetChild(0).position = startTransform.position;
		Sequence mySequence = DOTween.Sequence();
		mySequence.Append(transform.GetChild(0).DOMove(endTransform.position, walkDuration).SetEase(Ease.Linear));
		mySequence.AppendCallback(()=>transform.GetChild(0).position = startTransform.position);
		mySequence.SetLoops(-1);
	}
}