using System.Collections.Generic;
using Cinemachine;
using DG.Tweening;
using UnityEngine;

public class SlidingDownBurglar : MonoBehaviour
{
	[SerializeField] private float dropDownSpeed = 1f;
	[SerializeField] private float rayDistance = 50f;
	[SerializeField] private float draggedObjectDistance;
	[SerializeField] private List<Rigidbody> ragdollRigidBodies;
	[SerializeField] private Animator animator;
	[SerializeField] private GameObject karen;
	[SerializeField] private Transform runToJumpTransform;
	[SerializeField] private Transform jumpToFallTransform;
	[SerializeField] private float runTime = 1f;
	[SerializeField] private CinemachineVirtualCamera cam;
	[SerializeField] private GameObject helpPanel;
	[SerializeField] private GameObject rope;
	
	private Rigidbody _rb;
	private Camera _camera;

	private static readonly int RunHash = Animator.StringToHash("Run");
	private static readonly int JumpHash = Animator.StringToHash("Jump");
	private static readonly int FallHash = Animator.StringToHash("Fall");
	private static readonly int VictoryHash = Animator.StringToHash("Victory");
	private static readonly int DefeatHash = Animator.StringToHash("Defeat");

	public int totalEscapePhases = 0;
	
	private Transform _hitObj;
	private Ray _ray;
	private TargetMover _mover;

	private bool _startDropping;
	private bool _toDetect;
	private bool _toShowHelp;

	private void OnEnable()
	{
		GameEvents.TapToPlay += OnTapToPlay;
		LaserEscapeEvents.HitWithLaser += OnHitWithLasers;
		LaserEscapeEvents.EscapedAllTheLasers += OnEscapingAllTheLasers;
		LaserEscapeEvents.CrossedOneLaserGroup += OnCrossingOneLaserGroup;
	}

	private void OnDisable()
	{
		GameEvents.TapToPlay -= OnTapToPlay;
		LaserEscapeEvents.HitWithLaser -= OnHitWithLasers;
		LaserEscapeEvents.EscapedAllTheLasers -= OnEscapingAllTheLasers;
		LaserEscapeEvents.CrossedOneLaserGroup -= OnCrossingOneLaserGroup;
	}

	private void Start()
	{
		_camera= Camera.main;
		//animator = GetComponent<Animator>();
		_rb = GetComponent<Rigidbody>();
		
		foreach (var ragdoll in ragdollRigidBodies)
		{
			ragdoll.isKinematic = true;
		}

		_toShowHelp = true;
		rope.SetActive(false);
	}

	private void Update()
	{
		if (_startDropping)
		{
			transform.position += transform.forward * (Time.deltaTime * dropDownSpeed);
		}
		
		if(Input.GetKeyDown(KeyCode.P))
			LaserEscapeEvents.InvokeResetTargetPositions();
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("SlowSpeedCollider"))
		{
			dropDownSpeed /= 2f;
			other.gameObject.SetActive(false);
			RotateBody();
			LaserEscapeEvents.InvokeEnableRigs();
			_toDetect = true;
			if(_toShowHelp) helpPanel.SetActive(true);
		}

		if (!other.CompareTag("LaserClear")) return;
		dropDownSpeed = 2f;
		//other.gameObject.SetActive(false);
		ResetRotation();
		_toDetect = false;
		helpPanel.SetActive(false);
		_toShowHelp = false;
		if (AudioManager.instance)
		{
			AudioManager.instance.Play("LaserClear");
		}
	}

	public void MoveTheSelectedLimb()
	{
		if (!_hitObj) return;
		
		var ray = _camera.ScreenPointToRay(InputExtensions.GetInputPosition());

		_hitObj.position = _camera.transform.position + (ray.direction.normalized * (draggedObjectDistance * 1));

	}

	public void StartDragging()
	{
		if (!_toDetect) return;
		
		_ray = _camera.ScreenPointToRay(Input.mousePosition);

		if (Physics.Raycast(_ray, out var hit, rayDistance))
		{
			if (hit.collider.CompareTag("Player")) return;
			print(hit.collider.tag);
			if (hit.collider.CompareTag("IKCubes"))
			{
				_hitObj = hit.collider.transform;
				_mover = _hitObj.GetComponent<TargetMover>();
				_mover.isSelected = true;
				draggedObjectDistance = Vector3.Distance(_hitObj.position, _ray.origin);
			}
		}
	}

	public void ResetHitObj()
	{
		if(_mover)
			_mover.isSelected = false;
		
		_hitObj = null;
		_mover = null;
	}
	
	private void ActivateRagdoll()
	{
		_startDropping = false;
		animator.enabled = false;

		_rb.isKinematic = false;
		GetComponent<CapsuleCollider>().isTrigger = false;
		GetComponent<CapsuleCollider>().enabled = false;
		foreach (var ragdoll in ragdollRigidBodies)
		{
			//var x = fallPositionTransform.position - ragdoll.transform.position;
			ragdoll.isKinematic = false;
			//ragdoll.AddForce(x ,ForceMode.Impulse);
		}

		//GameCanvas.game.MakeGameResult(1, 1);
		

		DOVirtual.DelayedCall(1.5f, ()=>GameCanvas.game.MakeGameResult(1, 1));
	}

	private void OnHitWithLasers()
	{
		ActivateRagdoll();
		helpPanel.SetActive(false);
		AudioManager.instance.Pause("Rope");
		helpPanel.SetActive(false);
		rope.SetActive(false);
	}

	private void OnEscapingAllTheLasers()
	{
		animator.SetTrigger(VictoryHash);
	}

	private void OnCrossingOneLaserGroup()
	{
		totalEscapePhases--;
		if (totalEscapePhases < 1)
		{
			_startDropping = false;
			GameCanvas.game.MakeGameResult(0,0);
			LaserEscapeEvents.InvokeEscapedAllTheLasers();
			if(AudioManager.instance)
				AudioManager.instance.Play("NiceFemale");
		}
	}

	private void OnTapToPlay()
	{
		//_startDropping = true;
		//run to jump pos
		animator.SetTrigger(RunHash);
		cam.transform.DORotate(Vector3.right * 90f, 5f);

		transform.DOMove(runToJumpTransform.position, runTime).SetEase(Ease.Linear).OnComplete(() =>
		{
			animator.SetTrigger(JumpHash);
			
			transform.DOJump(jumpToFallTransform.position, 1f, 1, 1f).OnComplete(() =>
			{
				rope.SetActive(true);
				animator.SetTrigger(FallHash);
				_startDropping = true;
				transform.DORotate(new Vector3(90f, 0f, 0f), 0.25f).SetEase(Ease.Linear);
				AudioManager.instance.Play("Rope");
			});
			if (AudioManager.instance)
			{
				AudioManager.instance.Play("Jump");
				AudioManager.instance.Pause("Run");
			}
		});

		if (AudioManager.instance)
		{
			AudioManager.instance.Play("Run");
		}
	}

	private void RotateBody()
	{
		karen.transform.DOLocalRotate(new Vector3(-18f,0f,0f), 0.5f);// = Quaternion.Euler(-18f,0f,0f);
	}

	private void ResetRotation()
	{
		karen.transform.DOLocalRotate(Vector3.zero, 0.5f);// = Quaternion.Euler(-18f,0f,0f);
	}
}
