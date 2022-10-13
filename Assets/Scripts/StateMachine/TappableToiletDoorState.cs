using UnityEngine;

namespace StateMachine
{
	public sealed class TappableToiletDoorState : InputStateBase
	{
		private readonly ToiletInputHandler _input;

		public TappableToiletDoorState(ToiletInputHandler input) => _input = input;
		
		public override void Execute()
		{
			base.Execute();
			
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
			if (!hit.collider.CompareTag("Tappable") && !hit.collider.CompareTag("Draggable"))
			{
				ExitState();
				return;
			}

			if (hit.transform == _input.doorLeft.transform || hit.transform.parent.parent == _input.doorLeft.transform)
			{
				if(_input.doorLeft.TryOpen())
					AInputHandler.AssignNewState(InputState.Disabled);
				
				if(AudioManager.instance)
					AudioManager.instance.Play("FemaleShouting");
			}
			else if (hit.transform == _input.doorRight.transform|| hit.transform.parent.parent == _input.doorRight.transform)
			{
				if(_input.doorRight.TryOpen())
					AInputHandler.AssignNewState(InputState.Disabled);
				if(AudioManager.instance)
					AudioManager.instance.Play("MaleShouting");
			}
		}
	}
}