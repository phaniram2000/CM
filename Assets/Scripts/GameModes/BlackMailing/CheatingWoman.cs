using DG.Tweening;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class CheatingWoman : MonoBehaviour
{
	[SerializeField] private Rig rig;
	[SerializeField] private GameObject detectionCone;
	[SerializeField] private GameObject exclamationImage;
	[SerializeField] private GameObject blackMailCanvas;
	[SerializeField] private Transform finalChaseTransform;
	[SerializeField] private Transform blackmailerFinalTransform;
	
	private Animator _animator;
	private static readonly int ToObserveHash = Animator.StringToHash("ToObserve");
	private static readonly int ToKissHash = Animator.StringToHash("ToKiss");
	private static readonly int FoundTakingPhotosHash = Animator.StringToHash("FoundTakingPhotos");
	private static readonly int ToReactHash = Animator.StringToHash("ToReact");
	private static readonly int ToStandHash = Animator.StringToHash("Stand");
	private static readonly int ToArgueHash = Animator.StringToHash("ToArgue");
	
	private Sequence _mySeq;

	private bool _toInterruptTheSequence;

	private void Start()
	{
		_animator = GetComponent<Animator>();
	}

	private void OnEnable()
	{
		GameEvents.TapToPlay += OnTapToPlay;
		BlackmailingEvents.FoundTakingPictures += OnFoundTakingPhotos;
		BlackmailingEvents.ToNextGamePhase += Stand;
		BlackmailingEvents.GotFooled += OnGotFooled;
		BlackmailingEvents.SayNo += Argue;
		BlackmailingEvents.ToInterruptTheSequence += ToInterrupt;

	}

	private void OnDisable()
	{
		GameEvents.TapToPlay -= OnTapToPlay;
		BlackmailingEvents.FoundTakingPictures -= OnFoundTakingPhotos;
		BlackmailingEvents.ToNextGamePhase -= Stand;
		BlackmailingEvents.GotFooled -= OnGotFooled;
		BlackmailingEvents.SayNo -= Argue;
		BlackmailingEvents.ToInterruptTheSequence -= ToInterrupt;
	}
	
	private void StartDetecting()
	{
		if (_toInterruptTheSequence) return;
		
		print("In Start");
		_animator.SetTrigger(ToObserveHash);
		detectionCone.SetActive(true);
		SetWeight();
		BlackmailingEvents.InvokeToObserve();
		if (AudioManager.instance)
			AudioManager.instance.Play("HmmFemale");
	}

	private void ToKiss()
	{
		_animator.SetTrigger(ToKissHash);
		BlackmailingEvents.InvokeToKiss();
	}
	
	private void EndDetecting()
	{
		detectionCone.SetActive(false);
		RemoveWeight();
	}

	private void SetWeight() => DOTween.To(() => rig.weight, val => rig.weight = val, 1f, 1.25f);

	private void RemoveWeight() => DOTween.To(() => rig.weight, val => rig.weight = val, 0f, 0.5f);

	private void Alerted()
	{
		exclamationImage.SetActive(true);
		//blackMailCanvas.SetActive(false);
	}

	private void Calm()
	{
		exclamationImage.SetActive(false);
		//blackMailCanvas.SetActive(true);
	}

	private void KissingRoutine()
	{
		_mySeq = DOTween.Sequence();

		_mySeq.PrependInterval(2f);
		_mySeq.AppendCallback(Alerted);
		_mySeq.AppendInterval(1f);
		_mySeq.AppendCallback(StartDetecting);
		_mySeq.AppendInterval(3f);
		_mySeq.AppendCallback(Calm);
		_mySeq.AppendCallback(ToKiss);
		_mySeq.AppendCallback(EndDetecting);
		_mySeq.AppendInterval(3f);
		_mySeq.SetLoops(-1);
		
	}

	private void OnTapToPlay()
	{
		KissingRoutine();
		blackMailCanvas.SetActive(true);
	}

	private void OnFoundTakingPhotos()
	{
		blackMailCanvas.SetActive(false);
		_mySeq.Kill();
		_animator.SetTrigger(ToReactHash);
		DOVirtual.DelayedCall(2f,() => _animator.SetTrigger(FoundTakingPhotosHash));
		EndDetecting();
		Calm();
		transform.DOLookAt(finalChaseTransform.position, 0.5f).SetEase(Ease.Linear).OnComplete(() =>
		{
			DOVirtual.DelayedCall(2f, () => transform.DOMove(finalChaseTransform.position, 5f)).SetEase(Ease.Linear);
			//transform.DOMove(finalChaseTransform.position, 5f).SetEase(Ease.Linear);
		});
	}

	private void Stand()
	{
		_animator.SetTrigger(ToStandHash);
		transform.DOLookAt(blackmailerFinalTransform.position, 0.25f);
		RemoveWeight();
	}

	private void OnGotFooled()
	{
		Calm();
		EndDetecting();
		_mySeq.Kill();
	}

	private void Argue()
	{
		_animator.SetBool(ToArgueHash, true);
	}

	private void StayCalm()
	{
		_animator.SetBool(ToArgueHash,false);	
	}

	private void ToInterrupt()
	{
		_toInterruptTheSequence = true;
		OnGotFooled();
		ToKiss();
		DOVirtual.DelayedCall(0.5f,RemoveWeight);
	}
}
