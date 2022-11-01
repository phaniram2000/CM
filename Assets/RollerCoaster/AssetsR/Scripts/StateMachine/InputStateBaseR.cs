using UnityEngine;

namespace StateMachine
{
	public class InputStateBaseR
	{
		protected InputStateBaseR() { }

		public virtual void OnEnter() { }

		public virtual void Execute() { }

		public virtual void FixedExecute() { }

		public virtual void OnExit() { }

		public static void print(object message) => Debug.Log(message);
	}
	
	public sealed class DisabledStateR : InputStateBaseR { }
}