using UnityEngine;


namespace ShuffleCups
{
	public class InputStateBase
	{
		public static bool IsPersistent;
		protected static PaperPullerPlayer Player;
		private static float _decreaseMultiplier;

		protected InputStateBase() {}
		public InputStateBase(PaperPullerPlayer player, float decreaseMultiplier)
		{
			Player = player;
			_decreaseMultiplier = decreaseMultiplier;
		}

		public virtual void OnEnter() { }

		public virtual void Execute()
		{
			if (Player.myData.currentRpm > 0.1f)
				Player.myData.currentRpm -= Time.deltaTime * _decreaseMultiplier;
		}

		public virtual void FixedExecute() {}
	
		public virtual void OnExit() {}

		protected static void Print(object message)
		{
			Debug.Log(message);
		}

		public static void ReflectInGame()
		{
			PaperGameEvents.Singleton.InvokePullPaperStep();
			PaperGameEvents.Singleton.InvokePullPaperDelta(InputExtensions.GetInputDelta().y);
		}
	}
}


