using UnityEngine;

public class DetectionCone : MonoBehaviour
{
	private bool _foundTheCheater;
	private Animator _anim;
	private static readonly int FoundCheaterHash = Animator.StringToHash("FoundCheater");

	private CheatingClassroomTeacher _teacher;
	
	private Transform _rootTransform;
	private void Start()
	{
		_teacher = transform.root.GetComponent<CheatingClassroomTeacher>();
		
		_anim = _teacher.transform.GetComponent<Animator>();
		_rootTransform = _teacher.transform;
	}
	private void OnTriggerEnter(Collider other)
	{
		print("Caught");

		if(other.CompareTag("Player"))
			CheckForCheaters();	
	}

	private void OnTriggerStay(Collider other)
	{
		if(other.CompareTag("Player"))
			CheckForCheaters();	
	}

	private void OnTriggerExit(Collider other)
	{
		if(other.CompareTag("Player"))
			CheckForCheaters();	
	}

	private void CheckForCheaters()
	{
		if (_foundTheCheater) return;

		if (!CheatingStudentController.IsCheating) return;

		CheatingEvents.InvokeCheaterFound();
		GameCanvas.game.MakeGameResult(1);
		_foundTheCheater = true;
		if (AudioManager.instance)
		{
			AudioManager.instance.Play("GirlCry");
		}
	}
}
