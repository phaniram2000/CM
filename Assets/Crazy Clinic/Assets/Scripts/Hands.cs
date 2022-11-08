using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class Hands : MonoBehaviour
{
	[SerializeField] private GameObject weldingGun;
	[SerializeField] private GameObject hammer;
	[SerializeField] private GameObject bomb;
	[SerializeField] private Transform bombFinalTransform;

	[SerializeField] private Rig rig;
	[SerializeField] private Transform target;
	[SerializeField] private Transform startTransform;
	[SerializeField] private Transform endTransform;
	[SerializeField] private float moveSpeed = 1f;

	private Animator _animator;
	private static readonly int BombDropHash = Animator.StringToHash("BombDrop");
	private static readonly int HammerHash = Animator.StringToHash("Hammer");
	private static readonly int winHash = Animator.StringToHash("win");
	private static readonly int StartWeldingHash = Animator.StringToHash("WeldingHold");
	private static readonly int StopWeldingHash = Animator.StringToHash("WeldingRelease");

	private GameObject Bomb { get => bomb; set => bomb = value; }

	private bool _toWeld;
	private bool _choseWelding;

	private bool _weldSoundOn;
	private void OnEnable()
	{
		GameEventsCrazy.OptionHammer += OnChoosingHammer;
		GameEventsCrazy.OptionCutting += OnChoosingCutting;
		GameEventsCrazy.OptionBomb += OnChoosingBombing;
		GameEventsCrazy.HideTheHands += OnHideTheHands;
		GameEventsCrazy.DoneCutting += OnDoneCutting;
	}

	private void OnDisable()
	{
		GameEventsCrazy.OptionHammer -= OnChoosingHammer;
		GameEventsCrazy.OptionCutting -= OnChoosingCutting;
		GameEventsCrazy.OptionBomb -= OnChoosingBombing;
		GameEventsCrazy.HideTheHands -= OnHideTheHands;
		GameEventsCrazy.DoneCutting -= OnDoneCutting;
	}

	private void Start()
	{
		_animator = GetComponent<Animator>();
		bomb.SetActive(false);
		hammer.SetActive(false);
	}

	private void Update()
	{
		if (_choseWelding)
		{
			if (InputHandlerCrazy.GetFingerHeld())
			{
				if (Vector3.Distance(target.position, endTransform.position) > 0.0f)
				{
					//move target down
					target.transform.position = Vector3.MoveTowards(target.position,
						endTransform.position,
						Time.deltaTime * moveSpeed);
					if (!_weldSoundOn)
					{
						if (AudioManager.instance)
						{
							AudioManager.instance.Play("Weld");
							_weldSoundOn = true;
						}
					}
				}
			}

			if (InputHandlerCrazy.GetFingerUp())
			{
				if (_weldSoundOn)
				{
					if(AudioManager.instance)
						AudioManager.instance.Pause("Weld");
					_weldSoundOn = false;
				}				
			}

		}
	}

	private void PlayThrowAnimation()
	{
		//_animator.SetTrigger("aim");	
		//_animator.SetTrigger("throw");	
		_animator.SetTrigger(BombDropHash);
		Bomb.SetActive(true);
	}

	private void PlayHammerAnimation()
	{
		hammer.SetActive(true);
		_animator.SetTrigger(HammerHash);	
	}
	
	public void ThrowTheBomb()
	{
		AudioManager.instance.Play("LitCrackers");
		//Bomb.transform.DOJump(bombFinalTransform.position, 0.1f,1,1f).SetEase(Ease.Linear);
		bomb.transform.parent = null;
		Bomb.transform.DOMove(bombFinalTransform.position, 0.5f).SetEase(Ease.Linear).OnComplete(()=>Bomb.SetActive(false));
		Bomb.transform.DOScale(Bomb.transform.lossyScale * 0.1f, 0.5f).SetEase(Ease.Linear);
		DOVirtual.DelayedCall(2f, ()=>
		{
			GameEventsCrazy.InvokeExplosionEffects();
			AudioManager.instance.Pause("Slide");
			AudioManager.instance.Pause("LitCracker");
		});
		if(AudioManager.instance)
			AudioManager.instance.Play("Slide");
	}

	public void HammerTheHead()
	{
		GameEventsCrazy.InvokeHammerTheHead();
	}

	private void OnHideTheHands()
	{
		_animator.SetTrigger(winHash);
	}

	private void OnChoosingHammer()
	{
		PlayHammerAnimation();	
	}

	private void OnChoosingCutting()
	{
		_choseWelding = true;
		_animator.SetTrigger(StartWeldingHash);
		rig.weight = 1f;
		target.transform.DOMove(startTransform.position, 0.5f).SetEase(Ease.Linear);
		GameEventsCrazy.InvokePrepareToCutTheCone();
		weldingGun.SetActive(true);
	}

	private void OnChoosingBombing()
	{
		PlayThrowAnimation();
	}

	private void OnDoneCutting()
	{
		weldingGun.SetActive(false);
		rig.weight = 0f;
		_animator.SetTrigger(StopWeldingHash);
		_animator.SetTrigger(winHash);
		if(AudioManager.instance)
			AudioManager.instance.Pause("Weld");
	}
}
