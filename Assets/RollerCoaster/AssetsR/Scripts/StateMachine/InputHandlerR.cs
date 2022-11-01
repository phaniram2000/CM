using System;
using Kart;
using UnityEngine;

public enum InputStateR {DisabledR, IdleOnTracks,MoveOnTracks,FallingFlying, ForwardFlying }

namespace StateMachine
{
	public class InputHandlerR : MonoBehaviour
	{
		//current state holder	
		private static InputStateBaseR _currentInputState;

		//all states
		private static readonly DisabledStateR DisabledStateR = new DisabledStateR();
		//track
		private static readonly IdleOnTrackState IdleOnTrackState = new IdleOnTrackState();
		private static readonly MoveOnTrackState MoveOnTrackState = new MoveOnTrackState();
		//flying
		private static readonly ForwardFlyingState ForwardFlyingState = new ForwardFlyingState();
		private static readonly FallingFlyingState FallingFlyingState = new FallingFlyingState();
		
		private bool _hasTappedToPlay;

		private void OnEnable()
		{
			GameEvents.TapToPlay += OnTapToPlay;
			GameEventsR.ReachEndOfTrack += OnReachEndOfTrack;
			GameEventsR.RunOutOfPassengers += OnStopOnBonusRamp;
			GameEventsR.PlayerDeath += OnPlayerDeath;
		}

		private void OnDisable()
		{
			GameEvents.TapToPlay -= OnTapToPlay;
			GameEventsR.ReachEndOfTrack -= OnReachEndOfTrack;
			GameEventsR.RunOutOfPassengers -= OnStopOnBonusRamp;
			GameEventsR.PlayerDeath -= OnPlayerDeath;
		}

		private void Start()
		{
			InputExtensionsR.IsUsingTouch = Application.platform != RuntimePlatform.WindowsEditor &&
											(Application.platform == RuntimePlatform.Android ||
											 Application.platform == RuntimePlatform.IPhonePlayer);
			InputExtensionsR.TouchInputDivisor = MyHelpersR.RemapClamped(1920, 2400, 30, 20, Screen.height);

			var player = GameObject.FindGameObjectWithTag("Player");

			_ = new TrackStateBase(player.GetComponent<KartTrackMovement>());
			_ = new FlyingStateBase(player.GetComponent<KartFlyMovement>());

			_currentInputState = IdleOnTrackState;
			
			Vibration.Init();
		}

		private void Update()
		{
			if(!_hasTappedToPlay) return;
			
			//print($"{_currentInputState}");
			if(!(_currentInputState is DisabledStateR) && !(_currentInputState is MoveOnTrackState && MoveOnTrackState.IsPersistent))
			{
				var newState = HandleInput();

				if (_currentInputState != newState)
				{
					_currentInputState?.OnExit();
					_currentInputState = newState;
					_currentInputState?.OnEnter();
				}
			}

			_currentInputState?.Execute();
		}

		private void FixedUpdate()
		{
			_currentInputState?.FixedExecute();
		}

		private static InputStateBaseR HandleInput()
		{
			if (_currentInputState is TrackStateBase)
			{
				if (InputExtensionsR.GetFingerHeld())
					return MoveOnTrackState;
				
				return IdleOnTrackState;
			}
			
			if (_currentInputState is FlyingStateBase)
			{
				if (InputExtensionsR.GetFingerHeld())
					return ForwardFlyingState;
				
				return FallingFlyingState;
			}
			
			return _currentInputState;
		}

		public static void AssignNewState(InputStateR state)
		{
			_currentInputState?.OnExit();
			_currentInputState = state switch
			{
				InputStateR.DisabledR => DisabledStateR,
				InputStateR.IdleOnTracks => IdleOnTrackState,
				InputStateR.MoveOnTracks => MoveOnTrackState,
				InputStateR.FallingFlying => FallingFlyingState,
				InputStateR.ForwardFlying => ForwardFlyingState,
				_ => throw new ArgumentOutOfRangeException(nameof(state), state,
					"aisa kya pass kar diya vro tune yahaan")
			};

			_currentInputState?.OnEnter();
		}

		private static void AssignNewState(InputStateBaseR newState)
		{
			_currentInputState?.OnExit();
			_currentInputState = newState;
			_currentInputState?.OnEnter();
		}

		private void OnTapToPlay() => _hasTappedToPlay = true;

		private static void OnReachEndOfTrack() => AssignNewState(InputStateR.FallingFlying);

		private static void OnStopOnBonusRamp() => AssignNewState(InputStateR.DisabledR);

		private static void OnPlayerDeath() => AssignNewState(InputStateR.DisabledR);
	}
}