using UnityEngine;

namespace StateMachine
{
	public class DrawPatternInputHandler : AInputHandler
	{
		public static DrawPatternMechanic DrawPatternMechanic { get; set; }
		
		private static DrawPatternState _drawPatternState;
		
		protected override void InitialiseDerivedState()
		{
			DrawPatternMechanic = GetComponentInChildren<DrawPatternMechanic>();
			_drawPatternState = new DrawPatternState(DrawPatternMechanic);
		}

		protected override InputStateBase HandleInput()
		{
			if (HasNoInput()) return CurrentInputState;

			var ray = Camera.ScreenPointToRay(InputExtensions.GetInputPosition());
			if (!Physics.Raycast(ray, out var hit, InputStateBase.RaycastDistance)) return CurrentInputState;
			
			if(hit.collider.CompareTag("PatternGridItem")) return _drawPatternState;
			
			return CurrentInputState;
		}
	}
}