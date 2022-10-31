using StateMachine;
using UnityEngine;

public class ShampooInputHandler : AInputHandler
{
	[SerializeField] private Prankster prankster;
	
	private static PrankingState _shampooState;

	protected override void OnEnable()
	{
		base.OnEnable();
		ShowerPrankEvents.GotFoundPranking += DisableStateInput;
		ShowerPrankEvents.DonePranking += DisableStateInput;
	}

	protected override void OnDisable()
	{
		base.OnDisable();
		ShowerPrankEvents.GotFoundPranking -= DisableStateInput;
		ShowerPrankEvents.DonePranking -= DisableStateInput;
	}

	protected override void InitialiseDerivedState()
	{
		_shampooState = new PrankingState(prankster, this);
		//ResetInterval();
	}

	protected override InputStateBase HandleInput()
	{
		if (HasNoInput()) return CurrentInputState;

		var ray = Camera.ScreenPointToRay(InputExtensions.GetInputPosition());
		if (!Physics.Raycast(ray, out var hit, InputStateBase.RaycastDistance)) return CurrentInputState;
			
		if(hit.collider.CompareTag("ShampooCollider")) return _shampooState;
			
		return CurrentInputState;
	}

	private void DisableStateInput()
	{
		AssignNewState(InputState.Disabled);
	}
}
