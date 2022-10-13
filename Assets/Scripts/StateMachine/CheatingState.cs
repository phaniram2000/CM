using StateMachine;

public class CheatingState : InputStateBase
{
	private readonly CheatingStudentController _cheatingStudent;
	
	public CheatingState(CheatingStudentController student)
	{
		_cheatingStudent = student;
	}
	
	public override void OnEnter()
	{
		//Play the Copying anim
		_cheatingStudent.PlayCopyAnim();
		
		if (AudioManager.instance)
		{
			AudioManager.instance.Play("Swipe");
		}
		
		Vibration.Vibrate(15);
	}

	public override void Execute()
	{
		if (InputExtensions.GetFingerHeld())
		{
			//Check of player touch 
			_cheatingStudent.canvas.FillTheImage();
		}
		
		if(InputExtensions.GetFingerUp())
		{
			_cheatingStudent.PlayCoveringAnim();
			ExitState();
			return;
		}
	}

	public override void OnExit()
	{
		//. . . 
		_cheatingStudent.PlayCoveringAnim();
		if (AudioManager.instance)
		{
			AudioManager.instance.Play("Swipe");
		}
	}
}
