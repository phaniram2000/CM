using UnityEngine;

public class SlappingState : InputStateBaseTrain
{
	public override void OnEnter(PlayerControlTrain player)
	{
		player.StartSlapping();
		player.ShowPastry();
		if(AudioManager.instance)
			AudioManager.instance.Play("Hand");
	}

	public override void OnUpdate(PlayerControlTrain player)
	{
		if (InputExtensions.GetFingerUp())
		{
			player.SwitchState(player.idleState);
		}
		
		if (player.toSlap)
		{
			player.Slap();
			player.toSlap = false;
			Debug.Log("InSlap");
		}
		
	}

	public override void OnExit(PlayerControlTrain player)
	{
		player.StopSlapping();
		player.toSlap = false;
		player.HidePastry();
		//player.SwitchState(player.idleState);
	}
}
