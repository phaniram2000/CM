using StateMachine;
using UnityEngine;

public class LaserEscapeInputHandler : AInputHandler
{
	[SerializeField] private SlidingDownBurglar burglar;
	private static LimbsArrangingState _limbsArrangingState;

	protected override void OnEnable()
	{
		base.OnEnable();
		JewelleryHeistEvents.FoundTheThief += DisableInput;
	}

	protected override void OnDisable()
	{
		base.OnDisable();
		JewelleryHeistEvents.FoundTheThief -= DisableInput;
	}

	protected override void InitialiseDerivedState()
	{
		_limbsArrangingState = new LimbsArrangingState(burglar);
	}

	protected override InputStateBase HandleInput()
	{
		if (HasNoInput()) return CurrentInputState;
		else return _limbsArrangingState;
		
		var ray = Camera.ScreenPointToRay(InputExtensions.GetInputPosition());
		if (!Physics.Raycast(ray, out var hit, InputStateBase.RaycastDistance)) return CurrentInputState;
			
		if (hit.collider.CompareTag("LaserEscapeCollider")) return _limbsArrangingState;

		return CurrentInputState;
	}

	private void DisableInput()
	{
		AssignNewState(InputState.Disabled);
	}
}
