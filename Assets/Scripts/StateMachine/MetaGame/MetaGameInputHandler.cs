using StateMachine;
using UnityEngine;

public class MetaGameInputHandler : AInputHandler
{
	[SerializeField] private float normalLength;

	protected override void InitialiseDerivedState()
	{
		_ = new MetaMovementState(
			GameObject.FindGameObjectWithTag("Player").GetComponent<MetaPlayer>(),
			normalLength);
	}

	protected override InputStateBase HandleInput()
	{
		if (InputExtensions.GetFingerHeld()) return new MetaMovementState(InputExtensions.GetInputPosition());
		
		return CurrentInputState;
	}
}