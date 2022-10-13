using UnityEngine;

public class BlackmailStatefulCamera : MonoBehaviour
{
	private Animator _anim;
	
	private static readonly int TapToPlay = Animator.StringToHash("tapToPlay");
	private static readonly int Win = Animator.StringToHash("win");
	private static readonly int InitialLossHash = Animator.StringToHash("Lose_1");
	private static readonly int FinalLossHash = Animator.StringToHash("Lose_2");
	private static readonly int ToNextGamePhase = Animator.StringToHash("ToNextPhase");

	private void OnEnable()
	{
		GameEvents.TapToPlay += OnTapToPlay;

		GameEvents.GameWin += OnGameWin;
		GameEvents.GameLose += OnGameLose1;
		GameEvents.GameLose += OnGameLose2;

		BlackmailingEvents.FoundTakingPictures += OnInitialLoss;
		BlackmailingEvents.ToNextGamePhase += OnNextGamePhase;
	}

	private void OnDisable()
	{
		GameEvents.TapToPlay -= OnTapToPlay;
		
		GameEvents.GameWin -= OnGameWin;
		GameEvents.GameLose -= OnGameLose1;
		GameEvents.GameLose -= OnGameLose2;
		
		BlackmailingEvents.FoundTakingPictures -= OnInitialLoss;
		BlackmailingEvents.ToNextGamePhase -= OnNextGamePhase;
	}

	private void Start()
	{
		_anim = GetComponent<Animator>();
	}

	private void OnTapToPlay()
	{
		_anim.SetTrigger(TapToPlay);
	}

	private void OnGameWin() => _anim.SetTrigger(Win);

	private void OnGameLose1(int result) => _anim.SetTrigger(InitialLossHash);
	private void OnGameLose2(int result) => _anim.SetTrigger(FinalLossHash);

	private void OnInitialLoss()
	{
		_anim.SetTrigger(InitialLossHash);
	}

	private void OnNextGamePhase()
	{
		_anim.SetTrigger(ToNextGamePhase);
	}

	private void FinalLoss()
	{
		_anim.SetTrigger(FinalLossHash);
	}

}
