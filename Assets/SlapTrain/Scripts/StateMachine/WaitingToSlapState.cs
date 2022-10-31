using UnityEngine;

public class WaitingToSlapState : InputStateBaseTrain
{
	public override void OnEnter(PlayerControlTrain player)
	{
		player.StartSlapping();
		Debug.Log("Slap Enter");
	}

	public override void OnUpdate(PlayerControlTrain player)
	{
		if(player.toSlap) 
			player.SwitchState(player.slappingState);
		if (InputExtensions.GetFingerUp())
		{
			Debug.Log("Idle Update");
			player.SwitchState(player.idleState);
		}
	}

	public override void OnExit(PlayerControlTrain player)
	{
		player.StopSlapping();
		//player.SwitchState(player.idleState);
	}
}
