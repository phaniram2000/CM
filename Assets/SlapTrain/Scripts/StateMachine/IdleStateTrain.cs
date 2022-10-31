public class IdleStateTrain : InputStateBaseTrain
{
	public override void OnEnter(PlayerControlTrain player)
	{
		
	}

	public override void OnUpdate(PlayerControlTrain player)
	{
		if (InputExtensions.GetFingerDown())
		{
			player.SwitchState(player.slappingState);
		}
	}

	public override void OnExit(PlayerControlTrain player)
	{
		//player.SwitchState(player.slappingState);
	}
}
