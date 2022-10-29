using StateMachine;
using UnityEngine;

public class JackpotInputHandler : AInputHandler
{
	[SerializeField] private SlotThief slotThief;
	
	private static SlotRollingState _slotRollingState;

	protected override void OnEnable()
	{
		base.OnEnable();
	}

	protected override void OnDisable()
	{
		base.OnDisable();
	}

	protected override void InitialiseDerivedState()
	{
		//_pictureClickingState = new PictureClickingState(blackmailer, this);
		_slotRollingState = new SlotRollingState(slotThief);
	}

	protected override InputStateBase HandleInput()
	{
		if (HasNoInput()) return CurrentInputState;

		var ray = Camera.ScreenPointToRay(InputExtensions.GetInputPosition());
		if (!Physics.Raycast(ray, out var hit, InputStateBase.RaycastDistance)) return CurrentInputState;
			
		if(hit.collider.CompareTag("JackpotCollider")) return _slotRollingState;
			
		return CurrentInputState;
	}

	private void DisableStateInput()
	{
		AssignNewState(InputState.Disabled);
	}
}
