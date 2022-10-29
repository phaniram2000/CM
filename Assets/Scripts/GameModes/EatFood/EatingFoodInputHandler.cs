using StateMachine;
using UnityEngine;

public class EatingFoodInputHandler : AInputHandler
{
	[SerializeField] private EatingPerson eatingPerson;
	
	private static EatingState _eatingState;

	protected override void OnEnable()
	{
		base.OnEnable();
		EatingFoodEvents.GotFull += DisableStateInput;
	}

	protected override void OnDisable()
	{
		base.OnDisable();
		EatingFoodEvents.GotFull -= DisableStateInput;
	}

	protected override void InitialiseDerivedState()
	{
		_eatingState = new EatingState(eatingPerson);
		//ResetInterval();

		SetCustomIdleState(new StandingIdle(eatingPerson));
	}

	protected override InputStateBase HandleInput()
	{
		if (HasNoInput()) return CurrentInputState;

		var ray = Camera.ScreenPointToRay(InputExtensions.GetInputPosition());
		if (!Physics.Raycast(ray, out var hit, InputStateBase.RaycastDistance)) return CurrentInputState;
			
		if(hit.collider.CompareTag("EatingFoodCollider")) return _eatingState;
			
		return CurrentInputState;
	}

	private void DisableStateInput()
	{
		AssignNewState(InputState.Disabled);
	}
}
