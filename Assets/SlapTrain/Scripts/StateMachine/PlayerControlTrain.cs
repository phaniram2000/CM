using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class PlayerControlTrain : MonoBehaviour
{
	private InputStateBaseTrain _currentState;

	public IdleStateTrain idleState = new IdleStateTrain();
	public WaitingToSlapState waitingToSlapState = new WaitingToSlapState();
	public SlappingState slappingState = new SlappingState();
	public DisabledStateTrain disabledState = new DisabledStateTrain();

	private Animator _animator;
	private static readonly int StartToSlapHash = Animator.StringToHash("StartToSlap");
	private static readonly int SlapHash = Animator.StringToHash("Slap");
	private static readonly int EndSlappingHash = Animator.StringToHash("EndSlapping");
	private static readonly int JumpHash = Animator.StringToHash("Jump");
	private static readonly int DanceHash = Animator.StringToHash("Dance");
	private static readonly int HitPoleHash = Animator.StringToHash("HitPole");

	public bool hasTappedToPlay;
	public bool toSlap;

	[SerializeField] private Transform jumpTransform;
	[SerializeField] private float jumpTime;
	[SerializeField] private Transform cameraPos;

	[SerializeField] private List<Rigidbody> ragdollRigidBodies;
	private Rigidbody _rb;

	[SerializeField] private Transform bagTransform;
	[SerializeField] private GameObject bag;

	[SerializeField] private GameObject pastry;
	[SerializeField] private bool isHoldingPastry;
	
	private Camera _camera;

	private Hand _hand;

	private Tween _myTween;
	private Vector3 _bagScale;
	
		
	private void OnEnable()
	{
		GameEventsTrain.EndLevel += Jump;
	}

	private void OnDisable()
	{
		GameEventsTrain.EndLevel -= Jump;

	}

	private void Awake()
	{
		_currentState = idleState;
		_currentState.OnEnter(this);
	}

	private void Start()
	{
		_animator = GetComponent<Animator>();
		_rb = GetComponent<Rigidbody>();
		
		foreach (var ragdoll in ragdollRigidBodies)
		{
			ragdoll.isKinematic = true;
		}

		_camera = Camera.main;
		_hand = GetComponentInChildren<Hand>();
		_bagScale = bag.transform.lossyScale;

		if (isHoldingPastry)
		{
			GameEventsTrain.InvokeIsHoldingCake();
			pastry.SetActive(true);
		}
		else
			pastry.SetActive(false);
	}
	
	private void Update()
	{
		if (InputExtensions.GetFingerDown())
		{
			if (!hasTappedToPlay)
			{
				GameEvents.InvokeTapToPlay();
				hasTappedToPlay = true;
				if(AudioManager.instance)
					AudioManager.instance.Play("Train");
			}
		}
		// if (!HasTappedToPlay()) return;
		
		_currentState.OnUpdate(this);
		
		if(Input.GetKeyDown(KeyCode.A)) ActivateRagdoll();
	}

	public void SwitchState(InputStateBaseTrain newState)
	{
		_currentState.OnExit(this);
		_currentState = newState;
		_currentState.OnEnter(this);
	}

	public void StartSlapping()
	{
		_animator.SetTrigger(StartToSlapHash);
	}

	public void Slap()
	{
		_animator.SetTrigger(SlapHash);
	}

	public void StopSlapping()
	{
		_animator.SetTrigger(EndSlappingHash);
	}

	private bool HasTappedToPlay()
	{
		if (hasTappedToPlay) return true;
		
		if(InputExtensions.GetFingerDown())
			GameEvents.InvokeTapToPlay();
		
		hasTappedToPlay = true;
		return hasTappedToPlay;
	}

	// private bool HasMadeTheFirstTouch()
	// {
	// 	
	// }
	
	private void Jump()
	{
		print("Jump loop ");
		transform.parent = null;
		transform.DORotate(new Vector3(0, -180, 0), 0.25f).SetEase(Ease.Linear);
		_camera.transform.parent = null;
		_camera.transform.DOMove(cameraPos.position, 1f).SetEase(Ease.Linear);
		_camera.transform.DORotate(cameraPos.rotation.eulerAngles, 1.25f).SetEase(Ease.Linear);
		_animator.SetTrigger(JumpHash);
		transform.DOJump(jumpTransform.position, 1f, 1, jumpTime).SetEase(Ease.Linear).OnComplete(() =>
		{
			_animator.SetTrigger(DanceHash);
		});
	}

	public void HitWithPole()
	{
		if(AudioManager.instance)
			AudioManager.instance.Play("Hurt");
		
		_animator.SetTrigger(HitPoleHash);
		if (GameManagerTrain.Instance.totalHearts <= 0)
		{
			ActivateRagdoll();
			//DOVirtual.DelayedCall(2f, () => { GameManagerTrain.Instance.ShowLostUi(); });
			GameEvents.InvokeGameLose(-1);
			if (AudioManager.instance)
			{
				AudioManager.instance.Pause("Hurt");
				AudioManager.instance.Play("Shock");
			}
		}
	}
	
	private void ActivateRagdoll()
	{
		_animator.enabled = false;
		_hand.transform.GetComponent<Collider>().enabled = false;
		//print("Here");
		_rb.isKinematic = false;
		//GetComponent<CapsuleCollider>().isTrigger = false;
		//GetComponent<CapsuleCollider>().enabled = false;
		foreach (var ragdoll in ragdollRigidBodies)
		{
			//var x = fallPositionTransform.position - ragdoll.transform.position;
			ragdoll.isKinematic = false;
			ragdoll.AddForce(Vector3.back * 5f, ForceMode.Impulse);
			//ragdoll.AddForce(x ,ForceMode.Impulse);
		}

		transform.parent = null;
		_camera.transform.parent = null;
	}

	public void MoveThePhoneToBag(GameObject phone)
	{
		phone.transform.parent = bagTransform;
		phone.transform.DORotate(new Vector3(16, -202, -177), 0.25f).SetEase(Ease.Linear);
		phone.transform.DOLocalJump(bagTransform.localPosition, 1f, 1, 0.5f).SetEase(Ease.Linear).OnComplete(() =>
		{
			if(_myTween.IsActive())
				_myTween.Kill(true);
			_myTween = bag.transform.DOScale(_bagScale * 1.2f, 0.1f).SetEase(Ease.Linear).SetLoops(2,LoopType.Yoyo);
			phone.SetActive(false);
			//AudioManager.instance.Play("Bag");
		});
		//if(AudioManager.instance)AudioManager.instance.Play("Slap");
	}

	public void ShowPastry()
	{
		if (!isHoldingPastry) return;
		pastry.SetActive(true);
	}

	public void HidePastry()
	{
		if (!isHoldingPastry) return;
		pastry.SetActive(false);
	}
}
