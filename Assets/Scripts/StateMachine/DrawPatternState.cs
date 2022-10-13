using UnityEngine;

namespace StateMachine
{
	public sealed class DrawPatternState : InputStateBase
	{
		private static DrawPatternMechanic _drawPatternMechanic;

		public DrawPatternState(DrawPatternMechanic mechanic)
		{
			_drawPatternMechanic = mechanic;
			
		}
		
		public override void OnEnter()
		{
			base.OnEnter();

			//clear previous drawing
			ResetPattern();
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
			if (!hit.collider.CompareTag("PatternGridItem"))
			{
				var point = hit.point;
				var peek = _drawPatternMechanic.PeekPos();
				if (peek == Vector3.zero) 
					point.z = peek.z;
				_drawPatternMechanic.AddTempPos(point);
				
				return;
			}
			if (_drawPatternMechanic.IsAlreadyTraversed(hit.collider))
			{
				_drawPatternMechanic.AddTempPos(hit.point);
				return;
			}

			_drawPatternMechanic.TraverseGridItem(hit.collider, hit.transform.position);
		
		}

		public override void OnExit()
		{
			base.Execute();

			/*
			foreach (var coll in _drawPatternMechanic.GetCurrentPattern()) 
				print(coll.name);
			*/

			_drawPatternMechanic.PopTempPos();
			if (GameCanvas.game.CheckGameResult(_drawPatternMechanic.GetNodesTraversed) == 0)
			{
				PatternMoneyEvents.InvokeCompletePatternStage();
				
				GameEvents.InvokeDoneWithRuleSet();
				GameRules.Get.SetGameMode();
				
				_drawPatternMechanic.FindRightPattern();	
				return;
			}

			_drawPatternMechanic.FindWrongPattern();
		}

		private static void ResetPattern() => _drawPatternMechanic.ClearPattern();
	}
}