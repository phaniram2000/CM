using GestureRecognizer;
using UnityEngine;

namespace StateMachine
{
	public sealed class DrawState : InputStateBase
	{
		private static DrawMechanic DrawMechanic;
		public DrawState(DrawMechanic mechanic) => DrawMechanic = mechanic;

		public override void OnEnter()
		{
			base.OnEnter();
			DrawMechanic.StartDrawing();
		}

		public override void Execute()
		{
			if(IsExitingCurrentState) return;
			
			if(InputExtensions.GetFingerUp())
			{
				ExitState();
				return;
			}
			
			var ray = Camera.ScreenPointToRay(InputExtensions.GetInputPosition());
			if (!Physics.Raycast(ray, out var hit, RaycastDistance))
			{
				ExitState();
				return;
			}
			if(!hit.collider.CompareTag("DrawArea")) 
			{
				ExitState();
				return;
			}

			DrawMechanic.Draw(hit);
		}

		public override void OnExit() => DrawMechanic.StopDrawing();
	}
}