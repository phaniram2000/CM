using StateMachine;
using UnityEngine;

public class JewelleryHeistInputHandler : AInputHandler
{
	[SerializeField] private JewelleryThief jewelleryThief;
	private static JewellryStealingState _jewelleryStealingState;

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
		_jewelleryStealingState = new JewellryStealingState(jewelleryThief);
	}

	protected override InputStateBase HandleInput()
	{
		if (HasNoInput()) return CurrentInputState;

		var ray = Camera.ScreenPointToRay(InputExtensions.GetInputPosition());
		if (!Physics.Raycast(ray, out var hit, InputStateBase.RaycastDistance)) return CurrentInputState;
			
		if (hit.collider.CompareTag("JewelleryHeist")) return _jewelleryStealingState;

		return CurrentInputState;
	}

	private void DisableInput()
	{
		AssignNewState(InputState.Disabled);
	}
}
