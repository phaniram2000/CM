using UnityEngine;

namespace StateMachine
{
	public class ToiletInputHandler : AInputHandler
	{
		public ToiletDoor doorRight, doorLeft;

		private static TappableToiletDoorState _tappableState;
		private bool _canTapOnDoor;
		
		private static DragMechanic DragMechanic { get; set; }
		
		private static DragState _dragState;
		
		protected override void OnEnable()
		{
			base.OnEnable();
			ToiletEvents.TimerExpired += OnTimerExpiry;
			ToiletEvents.GroupDone += OnGroupDone;
			

		}

		protected override void OnDisable()
		{
			base.OnDisable();
			ToiletEvents.TimerExpired -= OnTimerExpiry;
			ToiletEvents.GroupDone -= OnGroupDone;
			
		}

		protected override void InitialiseDerivedState()
		{
			DragMechanic = GetComponentInChildren<DragMechanic>();
			_dragState = new DragState(DragMechanic);
			_tappableState = new TappableToiletDoorState(this);
		}

		protected override InputStateBase HandleInput()
		{
			if (HasNoInput()) return CurrentInputState;

			var ray = Camera.ScreenPointToRay(InputExtensions.GetInputPosition());
			if (!Physics.Raycast(ray, out var hit, InputStateBase.RaycastDistance)) return CurrentInputState;

			switch (_canTapOnDoor)
			{
				case true:
				{
					if (hit.collider.CompareTag("Tappable") || hit.collider.CompareTag("Draggable")) return _tappableState;
					break;
				}
				default:
				{
					if(hit.collider.CompareTag("Draggable")) return _dragState;
					break;
				}
			}
			
			return CurrentInputState;
		}
		
		private void OnTimerExpiry() => _canTapOnDoor = true;

		private void OnGroupDone()
		{
			_canTapOnDoor = true;
			AssignNewState(InputState.Idle);
		}
		
		
	}
}