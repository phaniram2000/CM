using UnityEngine;

namespace ShuffleCups
{
	public class InputHandler : MonoBehaviour
{
	public static InputHandler Only;

	public bool testingUsingTouch;
	
	//derived states
	private static PullingState _pullingState;
	private static readonly IdleState IdleState = new IdleState();
	private static readonly DisabledState DisabledState = new DisabledState();
	
	//current state holder	
	private static InputStateBase _currentInputState;

	private bool _tappedToPlay, _shouldHandleInput = true;

	private void OnEnable()
	{
		PaperGameEvents.Singleton.tapToPlay += OnGameStart;
		
		PaperGameEvents.Singleton.tearPaper += OnGameOver;
		PaperGameEvents.Singleton.playerCrossFinishLine += OnGameOver;
		PaperGameEvents.Singleton.aiCrossFinishLine += OnGameOver;

		global::GameEvents.TapToPlay += OnGameStart;
	}

	private void OnDisable()
	{
		PaperGameEvents.Singleton.tapToPlay -= OnGameStart;
		
		PaperGameEvents.Singleton.tearPaper -= OnGameOver;
		PaperGameEvents.Singleton.playerCrossFinishLine -= OnGameOver;
		PaperGameEvents.Singleton.aiCrossFinishLine -= OnGameOver;
		
		global::GameEvents.TapToPlay -= OnGameStart;
	}

	private void Awake()
	{
		if (!Only) Only = this;
		else Destroy(gameObject);
	}

	private void Start()
	{
		if (testingUsingTouch) InputExtensions.IsUsingTouch = true; 
		else InputExtensions.IsUsingTouch = Application.platform != RuntimePlatform.WindowsEditor &&
											(Application.platform == RuntimePlatform.Android || 
											 Application.platform == RuntimePlatform.IPhonePlayer);
		
		InputExtensions.TouchInputDivisor = GameExtensions.RemapClamped(1920, 2400, 35, 25, Screen.height);

		var player = GameObject.FindGameObjectWithTag("Player").GetComponent<PaperPullerPlayer>();
		
		_ = new InputStateBase(player, PaperLevelFlowController.only.decreaseMultiplier);
		_pullingState = new PullingState(PaperLevelFlowController.only.increaseMultiplier, player.myData.pullingSpeed);

		_currentInputState = IdleState;
	}

	private void Update()
	{
		if(!_shouldHandleInput) return;
		if(!_tappedToPlay) return;
		
		if(_currentInputState is IdleState)
		{
			var newState = HandleInput();
			
			if(_currentInputState != newState)
			{
				_currentInputState?.OnExit();
				_currentInputState = newState;
				_currentInputState?.OnEnter();
			}
		}
		else if (InputExtensions.GetFingerUp() && !InputStateBase.IsPersistent)
			AssignNewState(IdleState);
		
		_currentInputState?.Execute();
		if(_currentInputState != DisabledState)
			InputStateBase.ReflectInGame();
	}

	private void FixedUpdate() => _currentInputState?.FixedExecute();

	private static InputStateBase HandleInput()
	{
		if (!InputExtensions.GetFingerHeld()) return _currentInputState;

		if(Mathf.Abs(InputExtensions.GetInputDelta().y) > 0f) return _pullingState;
		
		return _currentInputState;
	}

	private static void AssignNewState(InputStateBase newState)
	{
		_currentInputState?.OnExit();
		_currentInputState = newState;
		_currentInputState?.OnEnter();
	}

	private static void ChangeStateToDisabled()
	{
		AssignNewState(DisabledState);
	}

	private void OnGameStart() => _tappedToPlay = true;

	private void OnGameOver()
	{
		_shouldHandleInput = false;
		ChangeStateToDisabled();
	}
}
}

