using DG.Tweening;
using UnityEngine;

namespace RPS
{

	public class NpcAnimatorController : MonoBehaviour
	{
		[SerializeField] private float audioDelayDuration;


		private NpcRefBank _my;

		private Animator _anim;

		public static readonly int IdleIndex = Animator.StringToHash("IdleIndex");
		public static readonly int Rock = Animator.StringToHash("Rock");
		public static readonly int Paper = Animator.StringToHash("Paper");
		public static readonly int Scissor = Animator.StringToHash("Scissor");
		public static readonly int WinReaction = Animator.StringToHash("WinReaction");
		public static readonly int LoseReaction = Animator.StringToHash("LoseReaction");
		public static readonly int Slap = Animator.StringToHash("Slap");
		public static readonly int GetSlapped = Animator.StringToHash("GetSlapped");
		public static readonly int Tie = Animator.StringToHash("Tie");
		public static readonly int Lose = Animator.StringToHash("Lose");
		public static readonly int Victory = Animator.StringToHash("Victory");

		private void OnEnable()
		{
			RPSGameEvents.CameraZoomActionCompleted += OnCameraZoomActionCompleted;
			RPSGameEvents.NewRound += OnNewRound;
			RPSGameEvents.PlayerGaveSlap += OnPlayerGaveSlap;
			RPSGameEvents.NpcWin += OnNpcWin;
			RPSGameEvents.NpcLose += OnNpcLose;
			RPSGameEvents.GameTie += OnGameTie;
		}

		private void OnDisable()
		{
			RPSGameEvents.CameraZoomActionCompleted -= OnCameraZoomActionCompleted;
			RPSGameEvents.NewRound -= OnNewRound;
			RPSGameEvents.PlayerGaveSlap -= OnPlayerGaveSlap;
			RPSGameEvents.NpcWin -= OnNpcWin;
			RPSGameEvents.NpcLose -= OnNpcLose;
			RPSGameEvents.GameTie -= OnGameTie;
		}

		private void Start()
		{
			_my = GetComponent<NpcRefBank>();
			_anim = GetComponent<Animator>();
			_anim.SetInteger(IdleIndex, Random.Range(0, 2));
		}

		public void SetAnimatorStatus(bool status) => _anim.enabled = status;

		private void OnCameraZoomActionCompleted()
		{
			switch (_my.Controller.MyCurrentMove)
			{
				case GameMoves.Rock:
					_anim.SetBool(Rock, true);
					break;
				case GameMoves.Paper:
					_anim.SetBool(Paper, true);
					break;
				case GameMoves.Scissor:
					_anim.SetBool(Scissor, true);
					break;
			}
		}

		private void DisableMovesBoolean()
		{
			_anim.SetBool(Rock, false);
			_anim.SetBool(Paper, false);
			_anim.SetBool(Scissor, false);
		}



		public void OnWinReactionAnim()
		{
			_anim.SetBool(WinReaction, true);
			DisableMovesBoolean();

			if (_my.Controller.IsGirl)
				DOVirtual.DelayedCall(audioDelayDuration, () => RPSAudioManager.instance.Play("GirlWin"));

			if (_my.Controller.IsBoy)
				DOVirtual.DelayedCall(audioDelayDuration, () => RPSAudioManager.instance.Play("BoyWin"));

		}

		public void OnLoseReactionAnim()
		{
			_anim.SetBool(LoseReaction, true);

			if (_my.Controller.IsGirl)
				DOVirtual.DelayedCall(audioDelayDuration, () => RPSAudioManager.instance.Play("GirlLose"));

			if (_my.Controller.IsBoy)
				DOVirtual.DelayedCall(audioDelayDuration, () => RPSAudioManager.instance.Play("BoyLose"));
		}

		public void OnSlapAnim()
		{
			_anim.SetBool(Slap, true);

		}

		public void OnGetSlappedAnim()
		{
			_anim.SetBool(GetSlapped, true);
			if (_my.Controller.IsGirl)
				RPSAudioManager.instance.Play("GirlHurt");

			if (_my.Controller.IsBoy)
				RPSAudioManager.instance.Play("BoyHurt");
		}

		private void DisableAllBoolean()
		{
			_anim.SetBool(WinReaction, false);
			_anim.SetBool(LoseReaction, false);
			_anim.SetBool(Slap, false);
			_anim.SetBool(GetSlapped, false);
			_anim.SetBool(Tie, false);
			DisableMovesBoolean();
		}

		public void OnTieAnim()
		{
			_anim.SetBool(Tie, true);
			DisableMovesBoolean();
		}

		private void OnNewRound()
		{
			DisableAllBoolean();
		}

		private void OnPlayerGaveSlap()
		{
			OnGetSlappedAnim();
			DOVirtual.DelayedCall(1.5f, () => RPSGameEvents.InvokeOnNewRound());

			RPSAudioManager.instance.Play("Slap" + Random.Range(1, 7));
		}

		private void OnNpcLose()
		{
			OnLoseReactionAnim();
			_my.Controller.SadEmoji.SetActive(true);
		}

		private void OnNpcWin()
		{
			OnWinReactionAnim();
			_my.Controller.HappyEmoji.SetActive(true);
			DOVirtual.DelayedCall(2.3f, GiveSlap);
		}

		private void GiveSlap()
		{
			_my.Controller.EnableTortilla();
			print("Npc give slap");
			OnSlapAnim();
		}

		private void OnGameTie()
		{
			OnTieAnim();
		}

		public void LoseAnim()
		{
			_anim.SetTrigger(Lose);
		}

		public void VictoryAnim()
		{
			_anim.SetTrigger(Victory);
		}

	}

}
