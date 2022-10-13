using UnityEngine;

namespace StateMachine
{
	public class CheatingInputHandler : AInputHandler
	{
		private static CheatingState _copyState;
		protected override void InitialiseDerivedState()
		{
			_copyState = new CheatingState(GetComponentInChildren<CheatingStudentController>());
		}

		protected override InputStateBase HandleInput()
		{
			if (HasNoInput()) return CurrentInputState;

			var ray = Camera.ScreenPointToRay(InputExtensions.GetInputPosition());
			if (!Physics.Raycast(ray, out var hit, InputStateBase.RaycastDistance)) return CurrentInputState;
			
			if (hit.collider.CompareTag("CheatingCollider")) return _copyState;

			return CurrentInputState;
		}
	}
}