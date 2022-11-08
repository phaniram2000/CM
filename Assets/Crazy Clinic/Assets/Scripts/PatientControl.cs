using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class PatientControl : MonoBehaviour
{
	[SerializeField] private Transform finalWalkPosTransform;
	[SerializeField] private float walkDuration;

	[SerializeField] private ParticleSystem leftEarFlame;
	[SerializeField] private ParticleSystem rightEarFlame;
	[SerializeField] private ParticleSystem coneFlame;

	[SerializeField] private GameObject coneDisfigured;
	[SerializeField] private GameObject cone;
	[SerializeField] private GameObject sliceableCone;
	[SerializeField] private float slicingSpeed = 1f;
	[SerializeField] private bool toStartSlicing;
	[SerializeField] private Rigidbody sliceableRigidBody;
	
	[SerializeField] private SkinnedMeshRenderer coneMesh;
	
	[SerializeField] private SkinnedMeshRenderer mesh;
	[SerializeField] private Texture dirtyFaceTex;

	[SerializeField] private GameObject hair;

	[SerializeField] private List<ParticleSystem> fireworks;

	private Animator _animator;
	private static readonly int ToStandHash = Animator.StringToHash("ToStand");
	private static readonly int ToWalkHash = Animator.StringToHash("ToWalk");
	private static readonly int HammerAfterEffectsHash = Animator.StringToHash("HammerHitAfterEffect");
	private static readonly int ExplosionAfterEffectHash = Animator.StringToHash("ExplosionAfterEffect");
	private static readonly int DanceHash = Animator.StringToHash("Dance");

	
	private Camera _camera;

	private ParticleSystem LeftEarFlame => leftEarFlame;

	private ParticleSystem RightEarFlame => rightEarFlame;

	public GameObject ConeDisfigured => coneDisfigured;

	public bool isKid;

	public GameObject helpTextObj;
	
	private void OnEnable()
	{
		GameEvents.TapToPlay += OnTapToPlay;
		GameEventsCrazy.HammerTheHead += OnHammeringTheHead;
		GameEventsCrazy.ExplosionEffects += OnExplosion;
		GameEventsCrazy.PrepareToCutTheCone += OnPreparingToCut;
		GameEventsCrazy.DoneCutting += OnDoneCutting;
	}

	private void OnDisable()
	{
		GameEvents.TapToPlay -= OnTapToPlay;
		GameEventsCrazy.HammerTheHead -= OnHammeringTheHead;
		GameEventsCrazy.ExplosionEffects -= OnExplosion;
		GameEventsCrazy.PrepareToCutTheCone -= OnPreparingToCut;
		GameEventsCrazy.DoneCutting -= OnDoneCutting;
	}

	private void Start()
	{
		_camera = Camera.main;
		_animator = GetComponent<Animator>();
		ConeDisfigured.SetActive(false);
		hair.SetActive(false);
		if(AudioManager.instance)
			AudioManager.instance.Play("RemoveCone");
	}

	private void Update()
	{
		//if(Input.GetKeyDown(KeyCode.D)) Strt();
		
		if (!toStartSlicing) return;

		if (!InputHandlerCrazy.GetFingerHeld()) return;
		
		var currentBlendWeight = coneMesh.GetBlendShapeWeight(0);
		if (currentBlendWeight >= 70f)
		{
			slicingSpeed += 2.5f;
		}

		if (currentBlendWeight > 120f)
		{
			GameEventsCrazy.InvokeDoneCutting();
			return;
		}
		coneMesh.SetBlendShapeWeight(0, currentBlendWeight + slicingSpeed * Time.deltaTime);
	}

	private void OnTapToPlay()
	{
		_animator.SetTrigger(ToWalkHash);
		transform.DOMove(finalWalkPosTransform.position, walkDuration).SetEase(Ease.Linear).OnComplete(() =>
		{
			//move camera closer
			var camTransform = _camera.transform;
			camTransform.DOMove(GameManagerCrazy.Instance.CuttingCameraTransform.position, 0.5f).
				SetEase(Ease.Linear).OnComplete(
				() =>
				{
					GameManagerCrazy.Instance.OptionsMenu.SetActive(true);	
				});
			camTransform.DORotate(new Vector3(15f, 0f, 0f), 0.25f).SetEase(Ease.Linear);
			_animator.SetTrigger(ToStandHash);
			if(AudioManager.instance)
				AudioManager.instance.Pause("Walk");
			
		});
		if(AudioManager.instance)
			AudioManager.instance.Play("Walk");
		helpTextObj.SetActive(false);
		
	}

	private void OnHammeringTheHead()
	{
		ConeDisfigured.SetActive(true);
		cone.SetActive(false);
		_animator.SetTrigger(HammerAfterEffectsHash);
		DOVirtual.DelayedCall(0.5f,GameEventsCrazy.InvokeHideTheHands);
		// DOVirtual.DelayedCall(3f, GameManagerCrazy.Instance.ShowLostUi);
		DOVirtual.DelayedCall(1f, ()=>GameEvents.InvokeGameLose(-1));
		if (AudioManager.instance)
		{
			AudioManager.instance.Play("MaleScream");
			DOVirtual.DelayedCall(3f, ()=>AudioManager.instance.Pause("MaleScream"));
		}
		Vibration.Vibrate(30);
	}

	private void OnExplosion()
	{
		coneFlame.Play();
		LightFireworks();
		// cone.SetActive(false);
		DOVirtual.DelayedCall(1f,()=>
		{
			if(isKid)	
				hair.SetActive(true);
			cone.transform.DOMove(cone.transform.up * 5f, 3f).SetEase(Ease.Linear);
		});
		LeftEarFlame.Play();
		RightEarFlame.Play();
		_animator.SetTrigger(ExplosionAfterEffectHash);
		DOVirtual.DelayedCall(0.5f,GameEventsCrazy.InvokeHideTheHands);
		if(isKid) mesh.materials[4].mainTexture = dirtyFaceTex;
		// DOVirtual.DelayedCall(3f, GameManagerCrazy.Instance.ShowLostUi);
		DOVirtual.DelayedCall(1f, ()=>GameEvents.InvokeGameLose(-1));
	}

	private void OnPreparingToCut()
	{
		cone.SetActive(false);
		sliceableCone.SetActive(true);
		toStartSlicing = true;
	}

	private void OnDoneCutting()
	{
		_animator.SetTrigger(DanceHash);
		sliceableRigidBody.isKinematic = false;
		sliceableRigidBody.AddForce(transform.forward * (5f * Time.deltaTime));
		if(isKid) hair.SetActive(true);
		DOVirtual.DelayedCall(1f, GameEvents.InvokeGameWin);
		// DOVirtual.DelayedCall(3f, GameManagerCrazy.Instance.ShowWinUi);
		Vibration.Vibrate(30);
	}

	private void LightFireworks()
	{
		foreach (var firework in fireworks)
		{
			DOVirtual.DelayedCall(0.15f,()=>
			{
				firework.Play();
				
			});
		}

		if (AudioManager.instance)
		{
			AudioManager.instance.Play("FireWorks");
			AudioManager.instance.Play("MaleScream");
			Vibration.Vibrate(30);
			DOVirtual.DelayedCall(3f, ()=>AudioManager.instance.Pause("MaleScream"));
		}
	}
}
