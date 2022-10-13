using DG.Tweening;
using Unity.Collections;
using UnityEngine;

namespace RPS
{

	public class CharacterAnimationController : MonoBehaviour
	{
		private CharacterRefBank _my;

		private Animator _anim;

		public Animator Anim => _anim;

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
			RPSGameEvents.PlayerStartGiveSlap += OnPlayerStartGiveSlap;
			RPSGameEvents.NewRound += OnNewRound;
			RPSGameEvents.PlayerWin += OnPlayerWin;
			RPSGameEvents.PlayerLose += OnPlayerLose;
			RPSGameEvents.GameTie += OnGameTie;
			RPSGameEvents.NpcGaveSlap += OnNpcGaveSlap;
		}


		private void OnDisable()
		{
			RPSGameEvents.CameraZoomActionCompleted -= OnCameraZoomActionCompleted;
			RPSGameEvents.PlayerStartGiveSlap -= OnPlayerStartGiveSlap;
			RPSGameEvents.NewRound -= OnNewRound;
			RPSGameEvents.PlayerWin -= OnPlayerWin;
			RPSGameEvents.PlayerLose -= OnPlayerLose;
			RPSGameEvents.GameTie -= OnGameTie;
			RPSGameEvents.NpcGaveSlap -= OnNpcGaveSlap;
		}

		private void Start()
		{
			_my = GetComponent<CharacterRefBank>();
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
				RPSAudioManager.instance.Play("GirlWin");

			if (_my.Controller.IsBoy)
				RPSAudioManager.instance.Play("BoyWin");

		}

		public void OnLoseReactionAnim()
		{
			_anim.SetBool(LoseReaction, true);

			if (_my.Controller.IsGirl)
				RPSAudioManager.instance.Play("GirlLose");

			if (_my.Controller.IsBoy)
				RPSAudioManager.instance.Play("BoyLose");
		}

		public void OnSlapAnim()
		{
			_anim.SetBool(Slap, true);
			DOVirtual.DelayedCall(0.6f, DisableAllBoolean);
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


		private void OnPlayerStartGiveSlap()
		{
			DOVirtual.DelayedCall(0.2f, OnSlapAnim);
		}

		private void OnNewRound()
		{
			DisableAllBoolean();
		}

		private void OnPlayerLose()
		{
			OnLoseReactionAnim();
			_my.Controller.SadEmoji.SetActive(true);
		}

		private void OnPlayerWin()
		{
			OnWinReactionAnim();
			_my.Controller.HappyEmoji.SetActive(true);
		}

		private void OnGameTie()
		{
			OnTieAnim();
		}

		private void OnNpcGaveSlap()
		{
			OnGetSlappedAnim();
			DOVirtual.DelayedCall(1.5f, () => RPSGameEvents.InvokeOnNewRound());

			RPSAudioManager.instance.Play("Slap" + Random.Range(1, 7));
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
