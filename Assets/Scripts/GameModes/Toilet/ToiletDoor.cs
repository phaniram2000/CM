using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class ToiletDoor : MonoBehaviour
{
	[SerializeField] private GameObject tapToOpenText;
	[SerializeField] private bool isLeftDoor;
	
	private bool _hasBeenTapped;
	private static Tween _timerTween;
	private static bool _entrySortedOut;

	private static List<Transform> _npcsCollided = new();
	private Tween _tapTween;
	private Vector3 _initTapScale;
	private const float TweenRestartWaitTime = 2f;

	private void OnEnable()
	{
		ToiletEvents.TimerExpired += OnTimerExpiry;
		ToiletEvents.DoorSelected += OnDoorSelected;
		ToiletEvents.GroupDone += OnGroupDone;

	}

	private void OnDisable()
	{
		ToiletEvents.TimerExpired -= OnTimerExpiry;
		ToiletEvents.DoorSelected -= OnDoorSelected;
		ToiletEvents.GroupDone -= OnGroupDone;
	}

	private void Start()
	{
		_npcsCollided = new();
		_entrySortedOut = false;
		_initTapScale = tapToOpenText.transform.localScale;
		
		_timerTween = DOVirtual.DelayedCall(TweenRestartWaitTime, () =>
		{
			_entrySortedOut = true;
			ToiletEvents.InvokeStartTimer();
		});
		_timerTween.Rewind();
	}

	public bool TryOpen()
	{
		if (_hasBeenTapped) return false;
		
		_hasBeenTapped = true;
		if(isLeftDoor)
			ToiletHelper.StartLeftDoor();
		else
			ToiletHelper.StartRightDoor();
		
		ToiletEvents.InvokeDoorSelected();
		
		if(AudioManager.instance)
			AudioManager.instance.Play("Button");
		Vibration.Vibrate(50);
		
		return true;
	}

	private void OnCollisionEnter(Collision collision)
	{
		if(_entrySortedOut) return;

		if(!collision.collider.CompareTag("NPC")) return;
		var npcRoot = collision.transform;
		if(_npcsCollided.Contains(npcRoot)) return;
		
		_npcsCollided.Add(npcRoot);
		
		_timerTween.Restart();
	}

	private void SetTapToOpenStatus(bool shouldShow)
	{
		tapToOpenText.SetActive(shouldShow);
		if (shouldShow) _tapTween = tapToOpenText.transform.DOScale(_initTapScale * 1.1f, 0.25f).SetLoops(-1, LoopType.Yoyo);
		else _tapTween.Kill();
	}
	
	private void OnTimerExpiry() => SetTapToOpenStatus(true);
	private void OnDoorSelected() => SetTapToOpenStatus(false);
	
	private void OnGroupDone()
	{
		if(_hasBeenTapped) return;
		
		SetTapToOpenStatus(true);
	}
}