using UnityEngine;

namespace StateMachine
{
	public class EraseState : InputStateBase
	{
		private static EraseTarget _eraseMechanic;
		
		protected EraseState() { }
		public EraseState(EraseTarget mechanic) => _eraseMechanic = mechanic;

		public override void Execute()
		{
			if(IsExitingCurrentState) return;
			
			if(InputExtensions.GetFingerUp())
			{
				ExitState();
				return;
			}
			
			var ray = Camera.ScreenPointToRay(InputExtensions.GetInputPosition());
			if (!Physics.Raycast(ray.origin, ray.direction, out var hit, RaycastDistance))
			{
				//ExitState();
				return;
			}
			if (!hit.transform.CompareTag("EraseTarget") && !hit.transform.root.CompareTag("EraseTarget"))
			{
				//ExitState();
				return;
			}

			_eraseMechanic.EraseAt(hit.point);
		}

		public override void OnExit()
		{
			base.OnExit();
		}
	}
}