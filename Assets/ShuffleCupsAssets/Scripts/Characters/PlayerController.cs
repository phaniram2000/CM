using DG.Tweening;
using UnityEngine;
using UnityEngine.Animations.Rigging;


namespace ShuffleCups
{
	public class PlayerController : MonoBehaviour
{
	[Header("Targets and Rigs")] public Transform leftHand;
	public Transform rightHand;
	[SerializeField] private Rig leftHandRig, rightHandRig;
	
	private Animator _anim;

	private Vector3 _initPosL, _initPosR;
	private bool _hasHadHeadHit, _isGirl, _isFirstSelection = true;
	
	private static readonly int ShouldFidget = Animator.StringToHash("shouldFidget");
	private static readonly int Die = Animator.StringToHash("die");
	private static readonly int Win = Animator.StringToHash("win");

	private void OnEnable()
	{
		GameEvents.Singleton.ShuffleStart += OnShuffleStart;
		GameEvents.Singleton.ShuffleEnd += OnShuffleEnd;
		
		GameEvents.Singleton.PlayerTapCup += OnCupSelect;
		GameEvents.Singleton.GameResult += OnGameResult;
	}
	
	private void OnDisable()
	{
		GameEvents.Singleton.ShuffleStart -= OnShuffleStart;
		GameEvents.Singleton.ShuffleEnd -= OnShuffleEnd;
		
		GameEvents.Singleton.PlayerTapCup -= OnCupSelect;
		GameEvents.Singleton.GameResult -= OnGameResult;
	}

	private void Start()
	{
		_anim = GetComponent<Animator>();
		_isGirl = TryGetComponent(out GirlPlayerController _);
		
		if(leftHand)
			_initPosL = leftHand.position;
		if(rightHand)
			_initPosR = rightHand.position;
	}

	private void OnShuffleStart()
	{
		if(!_isGirl)
			_anim.SetBool(ShouldFidget, true);
	}

	private void OnShuffleEnd()
	{
		if(!_isGirl)
			_anim.SetBool(ShouldFidget, false);
	}

	private void OnCupSelect(CupController cup)
	{
		if(!LevelFlowController.only.CanMakeNextAttempt()) return;
		
		if (_isFirstSelection)
		{			
			_isFirstSelection = false;
			//if (DOTween.TotalActiveTweens() > 0) return; 
		}
		var handTarget = cup.GetHandTarget(_initPosL, _initPosR, out var isLeftHand, out var exit);
		
		if(exit) return;

		var seq = DOTween.Sequence();
		
		//tap sounds
		seq.InsertCallback(0.2f, () => AudioManager.instance.Play("cup", 0.35f));
		seq.InsertCallback(0.5f, () => AudioManager.instance.Play("cup", 0.35f));
		
		if (isLeftHand)
		{
			seq.Insert(0f, 
			leftHand.DOLocalRotateQuaternion(leftHand.localRotation * Quaternion.Euler(-Vector3.forward * 30f), 0.2f)
				.SetLoops(4, LoopType.Yoyo));
			seq.Insert(0f, leftHand.DOMove(handTarget.position, 0.5f).SetEase(Ease.OutQuart));
			seq.Append(leftHand.DOMove(_initPosL, 0.5f).SetEase(Ease.OutQuart));
		}
		else
		{
			seq.Insert(0f, 
			rightHand.DOLocalRotateQuaternion(rightHand.localRotation * Quaternion.Euler(-Vector3.forward * 30f), 0.2f)
				.SetLoops(4, LoopType.Yoyo));
			seq.Insert(0f, rightHand.DOMove(handTarget.position, 0.5f).SetEase(Ease.OutQuart));
			seq.Append(rightHand.DOMove(_initPosR, 0.5f).SetEase(Ease.OutQuart));
		}

		seq.AppendCallback(() => GameEvents.Singleton.InvokeMakeSelection(isLeftHand, cup));
	}

	private void OnGameResult(bool didWin)
	{
		if(!didWin) return;

		_anim.SetTrigger(Win);
		DOTween.To(() => leftHandRig.weight, value => leftHandRig.weight = value, 0f, 0.5f);
		DOTween.To(() => rightHandRig.weight, value => rightHandRig.weight = value, 0f, 0.5f);
	}

	private void OnCollisionExit(Collision other)
	{
		if (!other.collider.CompareTag("Missile")) return;

		var seq = DOTween.Sequence();
		
		seq.AppendCallback(() => _anim.SetTrigger(Die));
		seq.AppendInterval(0.25f);
		seq.AppendCallback(() => AudioManager.instance.Play("thud"));

		Destroy(other.gameObject);
	}
}
}

