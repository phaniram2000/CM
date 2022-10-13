using DG.Tweening;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class CheatingClassroomTeacher : MonoBehaviour
{
	[SerializeField] private GameObject exclamationImage;
	[SerializeField] private GameObject detectionCone;
	
	[SerializeField] private Rig rig;
	[SerializeField] private Transform headTarget;
	[SerializeField] private Transform student1, student2;
	[SerializeField] private float inBetweenWaitTime = 2f;
	
	private Vector3 _pos;
	private Vector3 _currentPos;
	private WeightedTransform _weightTransform;

	private Animator _anim;
	private static readonly int FoundCheaterHash = Animator.StringToHash("FoundCheater");
	private static readonly int ToSleepHash = Animator.StringToHash("ToSleep");
	private Sequence _mySeq;

	[SerializeField] private ParticleSystem sleepParticle;
	private void OnEnable()
	{
		CheatingEvents.CheaterFound += OnCheaterFound;
		CheatingEvents.DoneCheating += OnGotFooled;
		GameEvents.TapToPlay += OnTapToPlay;
	}

	private void OnDisable()
	{
		CheatingEvents.CheaterFound -= OnCheaterFound;
		CheatingEvents.DoneCheating -= OnGotFooled;
		GameEvents.TapToPlay -= OnTapToPlay;
	}

	private void Start()
	{
		_anim = GetComponent<Animator>();
		
		_currentPos = transform.position;
		_pos = headTarget.transform.position;
		
		exclamationImage.SetActive(false);
		detectionCone.SetActive(false);
		
		
	}

	private void LookRight()
	{
		SetWeight();
		headTarget.DOMoveX(student2.position.x, 1f);
	}
	private void LookLeft() => headTarget.DOMoveX(student1.position.x, 1f);
	private void LookStraight() => headTarget.DOMoveX(_pos.x, 1f);
	private void SetWeight() => rig.weight = 1f;
	private void RemoveWeight() => rig.weight = 0f;
	private void Alerted()
	{
		exclamationImage.SetActive(true);
		ToWakeUp();
	}

	private void Calm()
	{
		exclamationImage.SetActive(false);
		ToSleep();
		
	}

	private void StartDetecting()
	{
		detectionCone.SetActive(true);
		if (AudioManager.instance)
			AudioManager.instance.Play("HmmFemale");
		
	}

	private void EndDetecting() => detectionCone.SetActive(false);

	private void OnCheaterFound()
	{
		EndDetecting();
		RemoveWeight();
		Calm();
		transform.position = _currentPos;
		_anim.SetTrigger(FoundCheaterHash);
		transform.DOLookAt(GameObject.FindGameObjectWithTag("Player").transform.position, 0.5f);
		_mySeq.Kill();
	}

	private void OnGotFooled()
	{
		EndDetecting();
		RemoveWeight();
		_mySeq.Kill();
	}
	
	private void ToSleep()
	{
		_anim.SetBool(ToSleepHash, true);
		Sleep();
	}

	private void ToWakeUp()
	{
		_anim.SetBool(ToSleepHash, false);
		WakeUp();
	}
	
	private void OnTapToPlay()
	{
		_mySeq = DOTween.Sequence();
		_mySeq.PrependInterval(3f);
		_mySeq.AppendCallback(Alerted);
		_mySeq.AppendInterval(inBetweenWaitTime);
		_mySeq.AppendCallback(StartDetecting);
		_mySeq.AppendCallback(LookRight);
		_mySeq.AppendInterval(inBetweenWaitTime);
		_mySeq.AppendCallback(LookLeft);
		_mySeq.AppendInterval(inBetweenWaitTime);
		_mySeq.AppendCallback(LookStraight);
		_mySeq.AppendInterval(inBetweenWaitTime);
		_mySeq.AppendCallback(EndDetecting);
		_mySeq.AppendCallback(Calm);
		_mySeq.AppendCallback(RemoveWeight);
		_mySeq.AppendInterval(inBetweenWaitTime);
		_mySeq.SetLoops(-1);
	}

	private void Sleep()
	{
		sleepParticle.Play();
	}

	private void WakeUp()
	{
		sleepParticle.Stop();
	}
}
