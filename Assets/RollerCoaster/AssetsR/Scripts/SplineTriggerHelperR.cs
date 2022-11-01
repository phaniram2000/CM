using DG.Tweening;
using Kart;
using StateMachine;
using UnityEngine;

public class SplineTriggerHelperR : MonoBehaviour
{
	private MainKartController _player;

	
	private void Start()
	{
		_player = GameObject.FindWithTag("Player").GetComponent<MainKartController>();
	}

	public void EnterHighSpeed() => EnterAction();
	public void EnterNormalSpeed() => EnterNormalcy();
	
	public void Cheer()
	{
		if(!AudioManagerR.instance) return;
		
		AudioManagerR.instance.Play("Hype" + Random.Range(1, 5));
		AudioManagerR.instance.Play("Hype" + Random.Range(1, 5));
		AudioManagerR.instance.Play("Hype" + Random.Range(1, 5));
		Vibration.Vibrate(5);
	}

	public void EnterHypeArea()
	{
		//if(AudioManager.instance) AudioManager.instance.Play("Hype" + Random.Range(1, 5));
		if (AudioManagerR.instance)
		{
			var random = Random.Range(1, 3);
			AudioManagerR.instance.Play("Jump" + random);
		}

		GameEventsR.InvokeUpdateHype(true);
	}

	public void ExitHypeArea() => 	GameEventsR.InvokeUpdateHype(false);

	public void RemoveInputControl() => MoveOnTrackState.ChangeStatePersistence(true);
	public void RestoreInputControl() => MoveOnTrackState.ChangeStatePersistence(false);
	
	public void EnterLeftHelix()
	{
		EnterAction();
		
		_player.PlayerAudio.DistantCameraDistanceVolume();
		CameraFxControllerR.only.SetSpeedLinesStatus(false);
		GameEventsR.InvokeEnterHelix(true);
		Vibration.Vibrate(5);
	}

	public void EnterRightHelix()
	{
	    EnterAction();
    	
		_player.PlayerAudio.DistantCameraDistanceVolume();
		CameraFxControllerR.only.SetSpeedLinesStatus(false);
		GameEventsR.InvokeEnterHelix(false);
		Vibration.Vibrate(5);

	}

	public void ExitHelix()
	{
		EnterNormalcy();
		
		_player.PlayerAudio.NormalCameraDistanceVolume();
		GameEventsR.InvokeExitHelix();		
		Vibration.Vibrate(5);
	}

	public void PassengerJump()
	{
		_player.AddedKartsManager.MakePassengersJump(1);
		CameraFxControllerR.only.DoCustomFov(75);
		_player.PlayerAudio.SlowMoPitch();
		TimeController.only.SlowDownTime();
		GameEventsR.InvokeUpdateHype(true);
		RemoveInputControl();
		
		if(AudioManagerR.instance) AudioManagerR.instance.Play("Jump" + Random.Range(1, 3));

		DOVirtual.DelayedCall(0.75f, () =>
		{
			TimeController.only.RevertTime();
			_player.PlayerAudio.NormalTimeScalePitch();
			RestoreInputControl();
		});
		Vibration.Vibrate(10);
	}
	
	public void PassengerJumpUninterrupted()
	{
		_player.AddedKartsManager.MakePassengersJump(1);
		CameraFxControllerR.only.DoCustomFov(75);
		_player.PlayerAudio.SlowMoPitch();
		TimeController.only.SlowDownTime();
		GameEventsR.InvokeUpdateHype(true);
		RemoveInputControl();
		if(AudioManagerR.instance) AudioManagerR.instance.Play("Jump");

		DOVirtual.DelayedCall(0.75f, () =>
		{
			TimeController.only.RevertTime();
			_player.PlayerAudio.NormalTimeScalePitch();
//			RestoreInputControl();
		});
		Vibration.Vibrate(10);

	}

	public void PassengerJumpNoSloMo()
	{
		_player.AddedKartsManager.MakePassengersJump(1);
		CameraFxControllerR.only.DoCustomFov(75);
		GameEventsR.InvokeUpdateHype(true);
	}

	public void PassengerJumpCustomDuration(float duration = 1f)
	{
		_player.AddedKartsManager.MakePassengersJump(duration);
		CameraFxControllerR.only.DoCustomFov(75);
		_player.PlayerAudio.SlowMoPitch();
		TimeController.only.SlowDownTime();
		GameEventsR.InvokeUpdateHype(true);
		RemoveInputControl();
		if(AudioManagerR.instance) AudioManagerR.instance.Play("Jump" + Random.Range(1, 3));

		DOVirtual.DelayedCall(duration * 0.75f, () =>
		{
			TimeController.only.RevertTime();
			_player.PlayerAudio.NormalTimeScalePitch();
			RestoreInputControl();
		});
	}

	public void OnReachTrackEnd()
	{
		DampCameraR.only.isDeliveryLevel = false;
		GameEventsR.InvokeReachEndOfTrack();
		EnterHypeArea();
		
		if(AudioManagerR.instance)
			AudioManagerR.instance?.Play("ReachEndTrack");
		
		GameEventsR.InvokePlayerOffFever();
		
	}

	public void EnterArea(int currentAreaCode) => GameEventsR.InvokeStartParade(currentAreaCode);

	public void AttackAction(int currentAreaCode) => GameEventsR.InvokeAttackPlayer(currentAreaCode);

	public void OnEnterSpecialCamera(Transform specialCamera) => DampCameraR.only.OnEnterSpecialCamera(specialCamera);
	public void OnEnterSpecialCameraSlow(Transform specialCamera) => DampCameraR.only.OnEnterSpecialCamera(specialCamera, true);

	public void OnExitSpecialCamera() => DampCameraR.only.OnExitSpecialCamera();

	private void EnterNormalcy()
	{
		_player.TrackMovement.SetNormalSpeedValues();
		ExitHypeArea();
		CameraFxControllerR.only.SetSpeedLinesStatus(false);
		CameraFxControllerR.only.DoNormalFov();
	}


	private void EnterAction()
	{
		_player.TrackMovement.SetHighSpeedValues();
		CameraFxControllerR.only.SetSpeedLinesStatus(true);
		EnterHypeArea();
		CameraFxControllerR.only.DoWideFov();
		Vibration.Vibrate(5);
	}

	public void StopRailSound()
	{
		_player.PlayerAudio.StopRailSound();
	}

	public void ShowLandingImpact()
	{
		CameraFxControllerR.only.ScreenShake(3);
		CameraFxControllerR.only.DoNormalFov();
	}

	public void ShowFallingPullOutFov()
	{
		CameraFxControllerR.only.DoCustomFov(80);
	}
}
