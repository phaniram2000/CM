using StateMachine;
using UnityEngine;

public class LockerInputHandler : AInputHandler
{
	[SerializeField] private LockerPicking lockerPicking;
	private static LockPickState _lockPickState;
	public bool reachedLocker;

	protected override void OnEnable()
	{
		base.OnEnable();

		BlackmailingEvents.ToNextGamePhase += OnReachingLocker;
	}

	protected override void OnDisable()
	{
		base.OnDisable();
		BlackmailingEvents.ToNextGamePhase -= OnReachingLocker;
	}

	protected override void InitialiseDerivedState()
	{
		_lockPickState = new LockPickState(lockerPicking);
	}

	protected override InputStateBase HandleInput()
	{
		if (HasNoInput()) return CurrentInputState;

		if (!reachedLocker) return CurrentInputState;
		
		var ray = Camera.ScreenPointToRay(InputExtensions.GetInputPosition());
		if (!Physics.Raycast(ray, out var hit, InputStateBase.RaycastDistance)) return CurrentInputState;
			
		if (hit.collider.CompareTag("LockerPickingCollider")) return _lockPickState;

		return CurrentInputState;
	}

	private void OnReachingLocker()
	{
		reachedLocker = true;
	}
}
