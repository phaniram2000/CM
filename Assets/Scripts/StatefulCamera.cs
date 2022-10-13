using DG.Tweening;
using UnityEngine;

public class StatefulCamera : MonoBehaviour
{
	[SerializeField] private bool isPub;
	private Animator _anim;

	private static readonly int TapToPlay = Animator.StringToHash("tapToPlay");
	private static readonly int Win = Animator.StringToHash("win");
	private static readonly int FailUnderConfident = Animator.StringToHash("underconfident");
	private static readonly int FailOverConfident = Animator.StringToHash("overconfident");
	private static readonly int ViewGameplay = Animator.StringToHash("viewGameplay");
	private static readonly int HasPreDraw = Animator.StringToHash("hasPreDraw");
	private static readonly int HasSeenPreDraw = Animator.StringToHash("hasSeenPreDraw");

	private void OnEnable()
	{
		GameEvents.PreDraw += OnPreDraw;

		if (isPub)
			PubEvents.StartInput += OnTapToPlay;
		else
			GameEvents.TapToPlay += OnTapToPlay;

		GameEvents.GameWin += OnGameWin;
		GameEvents.GameLose += OnGameLose;

		PubEvents.DoneWithInput += OnWatchGameplay;
		BankEvents.DoneWithInput += OnWatchGameplay;
	}

	private void OnDisable()
	{
		GameEvents.PreDraw -= OnPreDraw;

		if (isPub)
			PubEvents.StartInput -= OnTapToPlay;
		else
			GameEvents.TapToPlay -= OnTapToPlay;

		GameEvents.GameWin -= OnGameWin;
		GameEvents.GameLose -= OnGameLose;
		
		PubEvents.DoneWithInput -= OnWatchGameplay;
		BankEvents.DoneWithInput -= OnWatchGameplay;
	}

	private void Start()
	{
		_anim = GetComponent<Animator>();
		_anim.SetBool(HasPreDraw, GameRules.Get.hasPreDrawCam);
	}

	private void OnPreDraw()
	{
		
		if (GameRules.Get.hasSeenPreDrawCam)
		{
			_anim.SetTrigger(ViewGameplay);
			return;
		}
		_anim.SetTrigger(TapToPlay);
		DOVirtual.DelayedCall(0.25f, () => _anim.SetBool(HasSeenPreDraw, true));
		GameRules.Get.hasSeenPreDrawCam = true;
	}

	private void OnTapToPlay()
	{
		if (GameRules.GetGameMode == GameMode.Bank)
		{
			_anim.SetBool(HasSeenPreDraw,false);
			_anim.SetBool(HasPreDraw,true);
			_anim.SetTrigger(TapToPlay);


			DOVirtual.DelayedCall(1.3f, () =>
			{
				_anim.SetTrigger(TapToPlay);
			});
			return;
		}


		_anim.SetTrigger(TapToPlay);
	}

	private void OnGameWin() => _anim.SetTrigger(Win);

	private void OnGameLose(int result) => _anim.SetTrigger(result < 0 ? FailUnderConfident : FailOverConfident);

	private void OnWatchGameplay() => _anim.SetTrigger(ViewGameplay);
}