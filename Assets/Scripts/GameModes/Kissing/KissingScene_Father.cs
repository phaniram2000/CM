using DG.Tweening;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class KissingScene_Father : MonoBehaviour
{
	[SerializeField] private Rig rig;
	[SerializeField] private GameObject detectionCone;
	[SerializeField] private Transform headTarget;
	[SerializeField] private Transform directionToKids;
	[SerializeField] private GameObject exclamationImage;
	[SerializeField] private Transform inBetweenHuntPosition;
	[SerializeField] private Transform finalHuntPosition;
	[SerializeField] private Transform sideMoveTransform;
	[SerializeField] private GameObject gun;
	
	private Animator _animator;
	private static readonly int ToSitHash = Animator.StringToHash("ToSit");
	private static readonly int ToStandHash = Animator.StringToHash("ToStand");
	private static readonly int ToHuntHash = Animator.StringToHash("ToHunt");
	private static readonly int FoundKissingHash = Animator.StringToHash("FoundKissing");
	private static readonly int FoundMisbehavingHash = Animator.StringToHash("FoundMisbehaving");

	private Sequence _mySeq;
	
	private void OnEnable()
	{
		GameEvents.TapToPlay += OnTapToPlay;
		KissingEvents.GotFoundKissing += HuntDownTheBoy;
		KissingEvents.FooledFather += OnFooled;
	}

	private void OnDisable()
	{
		GameEvents.TapToPlay -= OnTapToPlay;
		KissingEvents.GotFoundKissing -= HuntDownTheBoy;
		KissingEvents.FooledFather -= OnFooled;
	}

	private void Start()
	{
		Calm();
		detectionCone.SetActive(false);
		_animator = GetComponent<Animator>();
	}

	private void Stand()
	{
		_animator.SetTrigger(ToStandHash); 
		LookAtKids();
	}

	private void LookAtKids()
	{
		transform.DOLookAt(directionToKids.position,2f,AxisConstraint.None,Vector3.up).OnComplete(StartDetecting);
		//StartDetecting();
	}

	private void Sit()
	{
		_animator.SetTrigger(ToSitHash);
		transform.DOLookAt(Vector3.zero, 1f,AxisConstraint.None,Vector3.up);
		EndDetecting();
	}
	

	private void FatherCheckingRoutine()
	{
		_mySeq = DOTween.Sequence();
		
		_mySeq.PrependInterval(3f);
		_mySeq.AppendCallback(Alerted);
		
		_mySeq.AppendInterval(1f);
		_mySeq.AppendCallback(Stand);

		_mySeq.AppendInterval(5f);
		_mySeq.AppendCallback(Calm);
		_mySeq.AppendCallback(Sit);

		_mySeq.AppendInterval(1f);
		_mySeq.SetLoops(-1);
	}

	private void StartDetecting()
	{
		detectionCone.SetActive(true);
		SetWeight();
		if (AudioManager.instance)
			AudioManager.instance.Play("Hmm");
	}

	private void EndDetecting()
	{
		detectionCone.SetActive(false);
		RemoveWeight();
	}

	private void SetWeight() => DOTween.To(() => rig.weight, val => rig.weight = val, 1f, 0.5f);

	private void RemoveWeight() => DOTween.To(() => rig.weight, val => rig.weight = val, 0f, 0.5f);

	private void Alerted()
	{
		exclamationImage.SetActive(true);
	}

	private void Calm()
	{
		exclamationImage.SetActive(false);
	}

	private void HuntDownTheBoy()
	{
		FoundKissingActions();
		DOVirtual.DelayedCall(2f, RunTowardsTheBoy);
	}
	
	private void FoundKissingActions()
	{
		transform.DOMove(sideMoveTransform.position, 1f);
		_animator.SetTrigger(FoundMisbehavingHash);
		_animator.SetTrigger(FoundKissingHash);
		EndDetecting();
		_mySeq.Kill();
		gun.SetActive(true);
		if (AudioManager.instance)
			AudioManager.instance.Play("Hey");
	}

	private void PlayHuntTheBoyAnim()
	{
		_animator.SetTrigger(ToHuntHash);
	}

	private void RunTowardsTheBoy()
	{
		PlayHuntTheBoyAnim();
		transform.DOMove(inBetweenHuntPosition.position, 1.5f).SetEase(Ease.Linear).OnComplete(() =>
		{
			transform.DOMove(finalHuntPosition.position, 1.5f).SetEase(Ease.Linear);
		});
	}

	private void OnTapToPlay()
	{
		FatherCheckingRoutine();
	}

	private void OnFooled()
	{
		Calm();
		EndDetecting();
	}
}
