using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class JewelleryThief : MonoBehaviour
{
	[SerializeField] private Transform slideUpTransform;
	[SerializeField] private Transform slideDownTransform;
	[SerializeField] private Transform fallPositionTransform;
	[SerializeField] private Transform escapePosition;

	[SerializeField] private GameObject rope;
	
	[SerializeField] private float slideTime = 1f;
	[SerializeField] private List<GameObject> itemListToSteal;
	[SerializeField] private Transform bagPos;
	[SerializeField] private int totalItemsToSteal;
	private float _itemPickupInterval = 0.5f;
	private float _itemPickupTime = 0.5f;
	private float _temp;
	
	public bool isCaught;

	private Animator _animator;
	private static readonly int StartStealingHash = Animator.StringToHash("StartStealing");
	private static readonly int StopStealingHash = Animator.StringToHash("StopStealing");
	private static readonly int FallHash = Animator.StringToHash("Fall");

	public bool isStealing;

	private Rigidbody _rb;
	[SerializeField] private List<Rigidbody> ragdollRigidBodies;

	[SerializeField] private float laughTimer = 1f;
	[SerializeField] private float laughTimerMultiplier = 0.1f;

	private void OnEnable()
	{
		JewelleryHeistEvents.GotHitByTheBaton += OnGotHitByTheBaton;
		JewelleryHeistEvents.FoundTheThief += OnGotFound;
		// JewelleryHeistEvents.HeistComplete += OnHeistComplete;
	}

	private void OnDisable()
	{
		JewelleryHeistEvents.GotHitByTheBaton -= OnGotHitByTheBaton;
		JewelleryHeistEvents.FoundTheThief -= OnGotFound;
		// JewelleryHeistEvents.HeistComplete -= OnHeistComplete;
	}

	private void Start()
	{
		_rb = GetComponent<Rigidbody>();
		foreach (var ragdoll in ragdollRigidBodies)
		{
			ragdoll.isKinematic = true;
		}
		
		_animator = GetComponent<Animator>();
		SlideUpAndHide();
		_temp = _itemPickupInterval;
		totalItemsToSteal = itemListToSteal.Count;
	}

	private void Update()
	{
		if(Input.GetKeyDown(KeyCode.U)) ActiveRagdoll();
	}

	public void SlideUpAndHide()
	{
		transform.DOMove(slideUpTransform.position, slideTime).SetEase(Ease.Linear);
		if (AudioManager.instance)
		{
			AudioManager.instance.Play("Woosh");
		}
	}

	public void SlideDownAndSteal()
	{
		transform.DOMove(slideDownTransform.position, slideTime).SetEase(Ease.Linear);
		if (AudioManager.instance)
		{
			AudioManager.instance.Play("Woosh");
		}
	}

	public void Steal()
	{
		if (!isStealing)
		{
			StartStealing();
		}
		_temp -= Time.deltaTime;
		if (_temp <= 0f)
		{
			//pickup
			_temp = _itemPickupInterval;
			PickupTheItem();
		}

		laughTimer -= Time.deltaTime * laughTimerMultiplier;
		if (laughTimer <= 0)
		{
			if (AudioManager.instance)
			{
				AudioManager.instance.Play("Laugh");
				laughTimer = 1f;
				print("Here");
			}
		}
	}

	private void StartStealing()
	{
		_animator.SetTrigger(StartStealingHash);
		isStealing = true;
	}
	
	public void StopStealing()
	{
		_animator.SetTrigger(StopStealingHash);
		isStealing = false;
	}

	public void ResetInterval()
	{
		_temp = _itemPickupInterval;
	}

	private void PickupTheItem()
	{
		if (itemListToSteal.Count <= 0) return;

		var x = itemListToSteal[^1];
		x.transform.DOMove(bagPos.position, _itemPickupTime).SetEase(Ease.Linear).OnComplete(()=>
		{
			if (AudioManager.instance)
			{
				AudioManager.instance.Play("Pickup");
				Vibration.Vibrate(20);
			}
			x.SetActive(false);
		});
		itemListToSteal.Remove(x);
		totalItemsToSteal--;
		
		if (AudioManager.instance)
		{
			AudioManager.instance.Play("DiamondJump");
		}
		
		if(totalItemsToSteal <= 0 )
			DOVirtual.DelayedCall(1f,OnHeistComplete);	
	}

	private void OnGotHitByTheBaton()
	{
		//transform.DOMove(fallPositionTransform.position, 0.5f).SetEase(Ease.Linear);

		ActiveRagdoll();
		if (AudioManager.instance)
		{
			AudioManager.instance.Play("Scream");
		}
		// transform.DORotate(new Vector3(2.892f, -140.1f, 2.804f), 0.5f).SetEase(Ease.Linear);
		// transform.DOMove(fallPositionTransform.position, 0.5f).SetEase(Ease.Linear);
		// _animator.SetTrigger(FallHash);
	}

	private void ActiveRagdoll()
	{
		_animator.enabled = false;

		_rb.isKinematic = false;
		// GetComponent<CapsuleCollider>().isTrigger = false;
		GetComponent<CapsuleCollider>().enabled = false;
		foreach (var ragdoll in ragdollRigidBodies)
		{
			var x = fallPositionTransform.position - ragdoll.transform.position;
			ragdoll.isKinematic = false;
			ragdoll.AddForce(x ,ForceMode.Impulse);
		}

		DOVirtual.DelayedCall(3f, ()=>GameCanvas.game.MakeGameResult(1, 1));
	}

	private void OnGotFound()
	{
		_animator.SetTrigger(StartStealingHash);
		_animator.SetTrigger(FallHash);
		isCaught = true;
		rope.SetActive(false);
	}

	private void OnHeistComplete()
	{
		transform.DOMove(escapePosition.position, 2f).SetEase(Ease.Flash);
		if(AudioManager.instance)
			AudioManager.instance.Play("NiceFemale");
		DOVirtual.DelayedCall(1f, ()=>GameCanvas.game.MakeGameResult(0, 0));
		rope.SetActive(false);
	}

	public void ResetLaughTimer()
	{
		laughTimer = 1f;
	}
}
