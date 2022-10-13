using DG.Tweening;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class BankPlayer : MonoBehaviour
{
	[SerializeField] private Transform cheque, chequeDest;
	
	public BankPolice Police { private get; set; }
	[SerializeField] private Transform leftHandcuff, rightHandcuff;
	[SerializeField] private Transform arrestDest, policeStart;
	[SerializeField] private float arrestDuration;
	private Rig _rigLeft, _rigRight;

	private Animator _anim;
	private static readonly int Win = Animator.StringToHash("win");
	private static readonly int Walk = Animator.StringToHash("walk");
	private static readonly int Sign = Animator.StringToHash("sign");
	private static readonly int Waiting = Animator.StringToHash("waiting");

	private void Start()
	{
		_anim = GetComponent<Animator>();
		var rigBuilder = GetComponent<RigBuilder>();
		_rigLeft = rigBuilder.layers[0].rig;
		_rigRight = rigBuilder.layers[1].rig;
	}

	private void OnEnable()
	{
		GameEvents.TapToPlay += OnTapToPlay;
		BankEvents.DoneWithInput += OnDoneWithInput;
		GameEvents.GameWin += OnGameWin;
		GameEvents.GameLose += OnGameLose;
	}

	private void OnDisable()
	{
		GameEvents.TapToPlay -= OnTapToPlay;
		BankEvents.DoneWithInput -= OnDoneWithInput;
		GameEvents.GameWin -= OnGameWin;
		GameEvents.GameLose -= OnGameLose;
	}

	private void GetArrested() => DOVirtual.DelayedCall(0.25f, ArrestSeq);

	private void ArrestSeq()
	{
		var seq = DOTween.Sequence();

		seq.Append(Police.transform.DOMove(policeStart.position, 0.5f));

		seq.Append(DOTween.To(
				() => _rigLeft.weight,
				value => _rigLeft.weight = _rigRight.weight = value,
				1f, 1.5f)
			.SetEase(Ease.InOutElastic));
		var lookRotation = Quaternion.LookRotation(arrestDest.position - transform.position);
		seq.Join(transform.DORotateQuaternion(
			lookRotation, 1f));
		seq.Join(Police.transform.DORotateQuaternion(lookRotation, 0.5f));

		seq.Join(leftHandcuff.DOScale(1.5f, 0.25f)
			.SetLoops(2, LoopType.Yoyo)
			.SetEase(Ease.InOutBack)
			.OnStart(() => leftHandcuff.gameObject.SetActive(true)));
		seq.Join(rightHandcuff.DOScale(1.5f, 0.25f)
			.SetLoops(2, LoopType.Yoyo)
			.SetEase(Ease.InOutBack)
			.OnStart(() => rightHandcuff.gameObject.SetActive(true)));
		
		seq.AppendCallback(Police.ArrestMe);

		seq.AppendCallback(() =>
		{
			Police.transform.parent = transform;
			_anim.SetTrigger(Walk);
		});
		seq.Append(transform.DOMove(arrestDest.position, arrestDuration)
			.SetEase(Ease.Linear));
		
		if (AudioManager.instance)
		{
			AudioManager.instance.Play("GirlCry");
		}
	}

	private void SendCheque()
	{
		cheque.DORotate(Vector3.up * 180f, 1f, RotateMode.WorldAxisAdd);
		cheque.DOMove(chequeDest.position, 0.5f)
			.SetDelay(1f)
			.OnComplete(() => _anim.SetTrigger(Waiting));
	}

	private void OnTapToPlay() => _anim.SetTrigger(Sign);

	private void OnDoneWithInput() => SendCheque();

	private void OnGameWin()
	{
		_anim.SetTrigger(Win);
		var dir = Camera.main.transform.position - transform.position;
		dir.y = 0;
		transform.DORotateQuaternion(Quaternion.LookRotation(dir), 0.5f);
	}

	private void OnGameLose(int _) => GetArrested();
}