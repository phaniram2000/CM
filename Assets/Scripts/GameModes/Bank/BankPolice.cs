using UnityEngine;

[RequireComponent(typeof(Animator))]
public class BankPolice : MonoBehaviour
{
	private Animator _anim;
	private BankPlayer _player;

	private static readonly int TapBaton = Animator.StringToHash("tapBaton");
	private static readonly int Walk = Animator.StringToHash("walk");

	private void OnEnable()
	{
		GameEvents.GameLose += OnGameLose;
	}

	private void OnDisable()
	{
		GameEvents.GameLose -= OnGameLose;
	}

	private void Start()
	{
		_anim = GetComponent<Animator>();
		_player = GameObject.FindGameObjectWithTag("Player").GetComponent<BankPlayer>();
		_player.Police = this;
	}

	public void ArrestMe()
	{
		_anim.SetTrigger(TapBaton);
		_anim.SetTrigger(Walk);
	}
	
	private void OnGameLose(int _)
	{
		var dir = _player.transform.position - transform.position;
		transform.rotation = Quaternion.LookRotation(dir);
		
		//_anim.SetTrigger(TapBaton);
	}
}