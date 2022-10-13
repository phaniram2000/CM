using UnityEngine;

namespace StateMachine
{
	public class EraseInputHandler : AInputHandler
	{
		protected override void InitialiseDerivedState() { }

		protected override InputStateBase HandleInput()
		{
			if (HasNoInput()) return CurrentInputState;

			var ray = Camera.ScreenPointToRay(InputExtensions.GetInputPosition());
			if (!Physics.Raycast(ray, out var hit, InputStateBase.RaycastDistance)) return CurrentInputState;
			
			if(hit.collider.CompareTag("EraseTarget")) return new EraseState(hit.collider.GetComponent<EraseTarget>());

			return CurrentInputState;
		}
	}
}