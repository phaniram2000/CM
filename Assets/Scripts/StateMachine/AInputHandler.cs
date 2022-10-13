using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

namespace StateMachine
{
	public enum InputState { Disabled, Idle }

	public abstract class AInputHandler : MonoBehaviour
	{
		[SerializeField] private bool debugCurrentState;
		
		//current state holder
		protected static InputStateBase CurrentInputState;

		//all states
		private static readonly DisabledState DisabledState = new();
		private static IdleState IdleState = new();
		
		protected static Camera Camera;
		private bool _hasTappedToPlay;

		protected abstract void InitialiseDerivedState();

		protected abstract InputStateBase HandleInput();
		protected static void SetCustomIdleState(IdleState customIdleState) => IdleState = customIdleState;
		protected static bool HasNoInput() => !InputExtensions.GetFingerDown();
		
		protected virtual void OnEnable()
		{
			GameEvents.GameWin += OnGameWin;
			GameEvents.GameLose += OnGameLose;

			PubEvents.StartInput += OnStartInput;
		}

		protected virtual void OnDisable()
		{
			GameEvents.GameWin -= OnGameWin;
			GameEvents.GameLose -= OnGameLose;
			
			PubEvents.StartInput -= OnStartInput;
		}

		private void Start()
		{
			DOTween.KillAll();
			InitialiseBaseState();
			InitialiseDerivedState();
		}

		private void InitialiseBaseState()
		{
			Camera = Camera.main;
			InputExtensions.IsUsingTouch = Application.platform != RuntimePlatform.WindowsEditor &&
										   (Application.platform == RuntimePlatform.Android ||
											Application.platform == RuntimePlatform.IPhonePlayer);
			InputExtensions.TouchInputDivisor = MyHelpers.RemapClamped(1920, 2160, 30, 20, Screen.height);

			_ = new InputStateBase(Camera);
			IdleState = new IdleState();
			CurrentInputState = IdleState;
		}

		private void Update()
		{
			if (Input.GetKeyDown(KeyCode.R)) SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
			
			//Fast forward
			if (Input.GetKeyDown(KeyCode.LeftAlt)) Time.timeScale = 6f;
			else if (Input.GetKeyUp(KeyCode.LeftAlt)) Time.timeScale = 1f;
			
			if(!HandleTapToPlay()) return;
			
			// if(debugCurrentState)
			// 	print($"{CurrentInputState}");
			if(CurrentInputState is IdleState)
			{
				var newState = HandleInput();

				if (CurrentInputState != newState)
				{
					CurrentInputState?.OnExit();
					CurrentInputState = newState;
					CurrentInputState?.OnEnter();
				}
			}

			CurrentInputState?.Execute();
		}

		private void FixedUpdate()
		{
			CurrentInputState?.FixedExecute();
		}

		private bool HandleTapToPlay()
		{
			if (_hasTappedToPlay) return true;

			if (!HasTappedOverUi()) return false;
			
			if (GameRules.Get && GameRules.Get.hasPreDrawCam)
			{
				if (!GameRules.Get.hasSeenPreDrawCam)
				{
					DOVirtual.DelayedCall(1f, () => _hasTappedToPlay = false);
					GameEvents.InvokePreDraw();
					//tap cooldown maybe
					return false;
				}
			}

			_hasTappedToPlay = true;
			GameEvents.InvokeTapToPlay();
			print("Tap to play");
			return true;
		}

		private static bool HasTappedOverUi()
		{
			if (!InputExtensions.GetFingerDown()) return false;

			if (!EventSystem.current) { print("no event system"); return false; }

			if (EventSystem.current.IsPointerOverGameObject(InputExtensions.IsUsingTouch ? Input.GetTouch(0).fingerId : -1)) return false;

			return true;
		}

		public static void AssignNewState(InputState state)
		{
			CurrentInputState?.OnExit();
			CurrentInputState = state switch
			{
				InputState.Disabled => DisabledState,
				InputState.Idle => IdleState,
				_ => throw new ArgumentOutOfRangeException(nameof(state), state,
					"Idle yaan Disabled hi allowed hai yahaan")
			};

			CurrentInputState?.OnEnter();
		}

		private static void OnStartInput() => AssignNewState(InputState.Idle);

		private static void OnGameWin() => AssignNewState(InputState.Disabled);

		private static void OnGameLose(int i) => AssignNewState(InputState.Disabled);

		private void SceneManagerOnSceneUnloaded(Scene _) => _hasTappedToPlay = false;
	}
}