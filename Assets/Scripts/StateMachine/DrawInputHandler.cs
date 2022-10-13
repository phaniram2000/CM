using GestureRecognizer;
using UnityEngine;

namespace StateMachine
{
	public class DrawInputHandler : AInputHandler
	{
		public static DrawMechanic DrawMechanic { get; private set; }
		//drawing
		private static DrawState _drawState;
		
		protected override void InitialiseDerivedState()
		{
			DrawMechanic = GetComponentInChildren<DrawMechanic>();
			_drawState = new DrawState(DrawMechanic);
		}

		protected override InputStateBase HandleInput()
		{
			if (HasNoInput()) return CurrentInputState;

			var ray = Camera.ScreenPointToRay(InputExtensions.GetInputPosition());
			if (!Physics.Raycast(ray, out var hit, InputStateBase.RaycastDistance)) return CurrentInputState;

			if(hit.collider.CompareTag("DrawArea")) return _drawState;

			return CurrentInputState;
		}
	}
}