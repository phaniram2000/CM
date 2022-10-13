using DG.Tweening;
using UnityEngine;

public class CheatingHusband : MonoBehaviour
{
	private Animator _animator;
	private static readonly int ToObserveHash = Animator.StringToHash("ToObserve");
	private static readonly int ToKissHash = Animator.StringToHash("ToKiss");
	private static readonly int ToReactHash = Animator.StringToHash("ToReact");
	private static readonly int ToRunHash = Animator.StringToHash("ToRun");
	private static readonly int ToStandHash = Animator.StringToHash("Stand");
	private static readonly int SayNoHash = Animator.StringToHash("SayNo");

	[SerializeField] private Transform finalRunTransform;
	[SerializeField] private Transform blackmailerFinalTransform;

	private void OnEnable()
	{
		BlackmailingEvents.ToObserve += Observe;
		BlackmailingEvents.ToKiss += ToKiss;
		BlackmailingEvents.FoundTakingPictures += FoundTakingPhotos;
		BlackmailingEvents.ToNextGamePhase += Stand;
		BlackmailingEvents.SayNo += SayNo;
		BlackmailingEvents.ToInterruptTheSequence += ToKiss;

	}

	private void OnDisable()
	{
		BlackmailingEvents.ToObserve -= Observe;
		BlackmailingEvents.ToKiss -= ToKiss;
		BlackmailingEvents.FoundTakingPictures -= FoundTakingPhotos;
		BlackmailingEvents.ToNextGamePhase -= Stand;
		BlackmailingEvents.SayNo -= SayNo;
		BlackmailingEvents.ToInterruptTheSequence -= ToKiss;
	}

	private void Start()
	{
		_animator = GetComponent<Animator>();
	}

	private void Observe()
	{
		_animator.SetTrigger(ToObserveHash);
	}

	private void ToKiss()
	{
		_animator.SetTrigger(ToKissHash);
	}

	private void FoundTakingPhotos()
	{
		_animator.SetTrigger(ToReactHash);
		//transform.DOLookAt(blackMailerTransform.position, 0.5f).SetEase(Ease.Linear);
		DOVirtual.DelayedCall(2f,Run);
	}

	private void Run()
	{
		_animator.SetTrigger(ToRunHash);
		transform.DOLookAt(finalRunTransform.position, 0.5f).SetEase(Ease.Linear).OnComplete(() =>
		{
			//DOVirtual.DelayedCall(2f, () => transform.DOMove(finalRunTransform.position, 5f)).SetEase(Ease.Linear);
			transform.DOMove(finalRunTransform.position, 3f).SetEase(Ease.Linear);
		});
	}

	private void Stand()
	{
		_animator.SetTrigger(ToStandHash);
		transform.DOLookAt(blackmailerFinalTransform.position, 0.25f);
	}

	private void SayNo()
	{
		_animator.SetBool(SayNoHash,true);
	}

	private void SayYes()
	{
		_animator.SetBool(SayNoHash,false);
	}
}
