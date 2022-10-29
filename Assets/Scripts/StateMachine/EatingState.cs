using StateMachine;
using UnityEngine;

public class EatingState : InputStateBase
{
	private readonly EatingPerson _eatingPerson;

	private float _interval = 0.5f;

	public EatingState(EatingPerson eatingPerson)
	{
		_eatingPerson = eatingPerson;
	}
	
	public override void OnEnter()
	{
		_interval = 0.5f;
		//Play the Copying anim
		_eatingPerson.StartEating();
	}

	public override void Execute()
	{
		if(InputExtensions.GetFingerUp())
		{
			//_cheatingStudent.PlayCoveringAnim();
			ExitState();
			return;
		}
		
		if (InputExtensions.GetFingerHeld())
		{
			_interval -= Time.deltaTime;
			_eatingPerson.IncreaseFillAmount();
			_eatingPerson.MakeSkinRed();
			if (_interval <= 0f)
			{
				_eatingPerson.Eat();
				_interval = 0.5f;
				if(AudioManager.instance)
					AudioManager.instance.Play("Eat");
			}
		}
	}

	public override void OnExit()
	{
		//. . . 
		//if required amount of pics taken then move to blackmailState
		_interval = 0.5f;
		// _eatingPerson.exclamationEnabled = false;
	}
}
