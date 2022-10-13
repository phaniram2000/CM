using UnityEngine;


namespace ShuffleCups
{
	public class PullingState : InputStateBase
	{
		private readonly float _increaseMultiplier, _pullingSpeed;

		public PullingState(float increaseMultiplier, float pullingSpeed)
		{
			_increaseMultiplier = increaseMultiplier;
			_pullingSpeed = pullingSpeed;
		}
	
		public override void OnEnter()
		{
			IsPersistent = false;
			Player.myData.speed.Play();
			Player.myData.source.Play();
		}
	
		public override void Execute()
		{
			base.Execute();
		
			if(!InputExtensions.GetFingerHeld()) return;

			var delta = InputExtensions.GetInputDelta().y;

			if(delta < 0.01f)
				Player.myData.source.pitch = GameExtensions.RemapClamped(0, 5f, 0.8f, 1.2f, delta);
			else
			{
				Player.myData.source.pitch = 0f;
				return;
			}
		
			var old = Player.myData.currentRpm;
			Player.myData.currentRpm += Time.deltaTime * _increaseMultiplier * -delta;
		
			Player.myData.distanceFromZero -= (Player.myData.currentRpm - old) * _pullingSpeed;
		}

		public override void OnExit()
		{
			Player.myData.source.Stop();
			Player.myData.speed.Stop();
			Player.RevertBackToOriginalHandRotations();
		}
	}
}

