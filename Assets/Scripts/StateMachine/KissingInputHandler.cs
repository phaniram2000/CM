using StateMachine;
using UnityEngine;

public class KissingInputHandler : AInputHandler
{
	[SerializeField] private KissingChildren kissingChildren;

	private static KissingState _kissingState;
	protected override void InitialiseDerivedState()
	{
		_kissingState = new KissingState(kissingChildren);
	}

	protected override InputStateBase HandleInput()
	{
		if (HasNoInput()) return CurrentInputState;

		var ray = Camera.ScreenPointToRay(InputExtensions.GetInputPosition());
		if (!Physics.Raycast(ray, out var hit, InputStateBase.RaycastDistance)) return CurrentInputState;
			
		if(hit.collider.CompareTag("KissingCollider")) return _kissingState;
			
		return CurrentInputState;
	}
}
