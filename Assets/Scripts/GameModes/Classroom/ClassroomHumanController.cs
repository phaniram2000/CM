using UnityEngine;

public class ClassroomHumanController : MonoBehaviour
{
	[SerializeField] private Transform spankWorldTransform;
	
	private Animator _anim;

	private static readonly int PlayerWin = Animator.StringToHash("playerWin");
	private static readonly int PlayerLoseUnderflow = Animator.StringToHash("playerLoseUnderflow");
	private static readonly int PlayerLoseOverflow = Animator.StringToHash("playerLoseOverflow");

	private void OnEnable()
	{
		GameEvents.GameWin += OnGameWin;
		GameEvents.GameLose += OnGameLose;
	}

	private void OnDisable()
	{
		GameEvents.GameWin -= OnGameWin;
		GameEvents.GameLose -= OnGameLose;
	}

	private void Start()
	{
		_anim = GetComponent<Animator>();
		spankWorldTransform.parent = null;
	}

	private void OnGameWin()
	{
		_anim.SetTrigger(PlayerWin);
		if (AudioManager.instance)
		{
			AudioManager.instance.Play("NiceFemale");
		}
	}

	private void OnGameLose(int result)
	{
		if (result < 0)
		{
			_anim.SetTrigger(PlayerLoseUnderflow);
			if (AudioManager.instance)
			{
				AudioManager.instance.Play("GirlCry");
			}
			return;
		}
		if (AudioManager.instance)
		{
			AudioManager.instance.Play("GirlCry");
		}
		_anim.SetTrigger(PlayerLoseOverflow);
		transform.position = spankWorldTransform.position;
		transform.rotation = spankWorldTransform.rotation;
	}
}