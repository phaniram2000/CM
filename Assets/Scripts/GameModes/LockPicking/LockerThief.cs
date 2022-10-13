using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class LockerThief : MonoBehaviour
{
	[SerializeField] private Transform sneakInDirectionTransform;
	[SerializeField] private Transform sneakInFirstPosition;
	[SerializeField] private Transform sneakInSecondPosition;
	[SerializeField] private GameObject door;
	[SerializeField] private GameObject safeDoor;
	[SerializeField] private GameObject questionCanvas;
	[SerializeField] private Transform lockerFrontTransform;
	[SerializeField] private GameObject swipeText;
	[SerializeField] private GameObject hand;
	
	private Animator _animator;
	private static readonly int SneakHash = Animator.StringToHash("Sneak");
	private static readonly int StandHash = Animator.StringToHash("stand");
	private static readonly int VictoryHash = Animator.StringToHash("Victory");

	private void OnEnable()
	{
		GameEvents.TapToPlay += OnTapToPlay;
		BlackmailingEvents.FinalWin += OnWin;
	}

	private void OnDisable()
	{
		GameEvents.TapToPlay -= OnTapToPlay;
		BlackmailingEvents.FinalWin -= OnWin;
	}

	private void Start()
	{
		_animator = GetComponent<Animator>();
	}
	
	private void OnTapToPlay()
	{
		// Start the sneaking in!
		StartSneaking();
	}

	private void StartSneaking()
	{
		_animator.SetTrigger(SneakHash);
		transform.DOLookAt(sneakInDirectionTransform.position, 0.25f);
		transform.DOMove(sneakInFirstPosition.position, 3f).SetEase(Ease.Linear).OnComplete(() =>
		{
			BlackmailingEvents.InvokeToNextGamePhase();
			transform.DOLookAt(sneakInDirectionTransform.position, 0.25f);
			door.transform.DORotate(new Vector3(-90f, 0f, 276.924f), 0.2f);
			if (AudioManager.instance)
				AudioManager.instance.Play("DoorOpen");
			transform.DOMove(sneakInSecondPosition.position, 2f).SetEase(Ease.Linear).OnComplete(() =>
			{
				swipeText.SetActive(true);
				hand.SetActive(true);
				_animator.SetTrigger(StandHash);
				DOVirtual.DelayedCall(1.5f, ()=>questionCanvas.SetActive(true));
				DOVirtual.DelayedCall(2.5f, ()=>swipeText.SetActive(false));
				DOVirtual.DelayedCall(2.5f, ()=>hand.SetActive(false));
			});
			//move camera inside here
		});
		if (AudioManager.instance)
			AudioManager.instance.Play("FootSteps");
	}

	private void OnWin()
	{
		_animator.SetTrigger(VictoryHash);
		questionCanvas.SetActive(false);
		OpenSafeDoor(); 
		DOVirtual.DelayedCall(0.5f,StandAtLocker);
		if (AudioManager.instance)
			AudioManager.instance.Play("NiceFemale");
	}

	private void OpenSafeDoor()
	{
		safeDoor.transform.DORotate(new Vector3(0f, 27.515f, 0f), 2f);
		if (AudioManager.instance)
			AudioManager.instance.Play("LockerOpen");
	}

	private void StandAtLocker()
	{
		transform.DOMove(lockerFrontTransform.position, 0.5f);
		transform.DOLookAt(sneakInFirstPosition.position, 0.5f);
	}
}
