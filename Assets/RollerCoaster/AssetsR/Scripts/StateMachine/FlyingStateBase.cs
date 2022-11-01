using Kart;

namespace StateMachine
{
	public class FlyingStateBase : InputStateBaseR
	{
		protected static KartFlyMovement Fly;

		protected FlyingStateBase() { }
		public FlyingStateBase(KartFlyMovement fly) => Fly = fly;

		public override void Execute()
		{
			Fly.CalculateForwardMovement();
			Fly.CalculateDownwardMovement();
			Fly.ApplyMovement();
		}
	}

	public sealed class FallingFlyingState : FlyingStateBase
	{
		public override void OnEnter()
		{
			Fly.SetDownwardOrientedValues();
			CameraFxControllerR.only.DoNormalFov();
		}
	}

	public sealed class ForwardFlyingState : FlyingStateBase
	{
		public override void OnEnter()
		{
			Fly.SetForwardOrientedValues();
			CameraFxControllerR.only.DoWideFov();
		}
	}
}