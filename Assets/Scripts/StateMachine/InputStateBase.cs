using UnityEngine;

namespace StateMachine
{
	public class InputStateBase
	{
		public const float RaycastDistance = 10f;
		protected static Camera Camera;
		protected static bool IsExitingCurrentState;

		public InputStateBase(Camera cam) => Camera = cam;
		protected InputStateBase() {}

		public virtual void OnEnter() => IsExitingCurrentState = false;

		public virtual void Execute() { }

		public virtual void FixedExecute() { }

		public virtual void OnExit() { }

		protected static void ExitState()
		{
			AInputHandler.AssignNewState(InputState.Idle);
			
			IsExitingCurrentState = true;
		}
		
		public static void print(object message) => Debug.Log(message);
		public static void print(object message, Object context) => Debug.Log(message, context);
	}
	
	public class IdleState : InputStateBase { }
	public sealed class DisabledState : InputStateBase { }
}