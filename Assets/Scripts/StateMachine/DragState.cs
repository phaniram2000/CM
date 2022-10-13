using UnityEngine;

namespace StateMachine
{
	public sealed class DragState : InputStateBase
	{
		private static DragMechanic _dragMechanic;
		private static RaycastHit[] _hits;
		
		public DragState(DragMechanic mechanic)
		{
			_dragMechanic = mechanic;
			_hits = new RaycastHit[15];
		}

		public override void OnEnter()
		{
			//select draggable
			base.OnEnter();
			
			if(!_dragMechanic.isAllowedToDrag)
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
			if(!hit.collider.CompareTag("Draggable"))
			{
				ExitState();
				return;
			}
			_dragMechanic.StartDragging(hit.transform, ray.origin);
		}

		public override void Execute()
		{
			//move around
			if(IsExitingCurrentState) return;
			
			if(!_dragMechanic.isAllowedToDrag)
			{
				ExitState();
				return;
			}

			if(InputExtensions.GetFingerUp())
			{
				ExitState();
				return;
			}

			var ray = Camera.ScreenPointToRay(InputExtensions.GetInputPosition());
			Physics.RaycastNonAlloc(ray, _hits, RaycastDistance);

			foreach (var hit in _hits)
			{
				if(!hit.collider) continue;
				if(!hit.collider.CompareTag("Draggable")) continue;

				if(_dragMechanic.TrySetSwappable(hit.transform)) break;
			}

			_dragMechanic.Drag(ray.direction);
		}

		public override void OnExit()
		{
			print("drag state end");
			_dragMechanic.StopDragging();
		}
	}
}