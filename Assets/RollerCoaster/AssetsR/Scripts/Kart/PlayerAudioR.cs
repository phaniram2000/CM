using DG.Tweening;
using UnityEngine;

namespace Kart
{
	[RequireComponent(typeof(AudioSource))]
	public class PlayerAudioR : MonoBehaviour
	{
		[SerializeField] private Limits pitch;
		[SerializeField] private float slowMoPitchScale, distantCameraVolumeScale;
		
		[SerializeField] private AudioSource rails, wind;
		private KartTrackMovement _kartTrackMovement;
		private Tween _volumeMotionTween, _volumeDistanceTween, _pitchTween;

		private float _initPitch, _initVolume;
		private bool _isPlaying;
		
		//private MainKartController _my;

		private void OnEnable()
		{
			GameEventsR.PlayerDeath += OnPlayerDeath;
			GameEventsR.ReachEndOfTrack += OnReachEndOfTrack;
		}

		private void OnDisable()
		{
			GameEventsR.PlayerDeath -= OnPlayerDeath;
			GameEventsR.ReachEndOfTrack -= OnReachEndOfTrack;
		}

		private void Start()
		{
			//_my = GetComponent<MainKartController>();
			_kartTrackMovement = GetComponent<KartTrackMovement>();

			_initPitch = rails.pitch;
			_initVolume = rails.volume;
		}

		public void UpdatePitch() => rails.pitch = wind.pitch =
			MyHelpersR.RemapClamped(_kartTrackMovement.CurrentLimits.min, _kartTrackMovement.CurrentLimits.max, 
				pitch.min, pitch.max, _kartTrackMovement.GetCurrentSpeed());

		public void StartMoving()
		{
			if (_volumeMotionTween.IsActive()) _volumeMotionTween.Kill();
			_volumeMotionTween = DOTween.To(GetVolume, SetVolume, 1f, 0.25f);
		}

		public void StopMoving()
		{
			if (_volumeMotionTween.IsActive()) _volumeMotionTween.Kill();
			_volumeMotionTween = DOTween.To(GetVolume, SetVolume, 0f, 1f);
		}

		public void DistantCameraDistanceVolume()
		{
			if (_pitchTween.IsActive()) _pitchTween.Kill();
			_pitchTween = DOTween.To(GetPitch, SetPitch, _initVolume * distantCameraVolumeScale, 0.25f);
		}

		public void NormalCameraDistanceVolume()
		{
			if (_pitchTween.IsActive()) _pitchTween.Kill();
			_pitchTween = DOTween.To(GetPitch, SetPitch, _initVolume, 0.25f);
		}

		public void SlowMoPitch()
		{
			if (_pitchTween.IsActive()) _pitchTween.Kill();
			_pitchTween = DOTween.To(GetPitch, SetPitch, _initPitch * slowMoPitchScale, 0.25f);
		}

		public void NormalTimeScalePitch()
		{
			if (_pitchTween.IsActive()) _pitchTween.Kill();
			_pitchTween = DOTween.To(GetPitch, SetPitch, _initPitch, 0.25f);
		}
		
		private float GetVolume() => rails.volume;
		private void SetVolume(float value) => rails.volume = value;
		
		private float GetPitch() => rails.pitch;
		private void SetPitch(float value) => rails.pitch = value;

		private void OnPlayerDeath() => StopMoving();

		private void OnReachEndOfTrack() => StopMoving();

		public void PlayIfNotPlaying()
		{
			if(_isPlaying) return;
			
			rails.Play();
			_isPlaying = true;
		}
		
		public void StopRailSound()
		{
			if (_volumeMotionTween.IsActive()) _volumeMotionTween.Kill();
			_volumeMotionTween = DOTween.To(GetVolume, SetVolume, 0f, 0.1f);
		}
	}
}