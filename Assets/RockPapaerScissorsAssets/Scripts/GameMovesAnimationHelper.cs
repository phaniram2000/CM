using DG.Tweening;
using UnityEngine;

namespace RPS
{

	public class GameMovesAnimationHelper : MonoBehaviour
	{
		private CharacterRefBank _characterRefBank;
		private NpcRefBank _npcRefBank;

		private bool _playerSpit, _npcSpit;

		private void OnEnable()
		{
			RPSGameEvents.PlayerGaveSlap += OnPlayerGaveSlap;
			RPSGameEvents.NpcGaveSlap += OnNpcGaveSlap;
		}

		private void OnDisable()
		{
			RPSGameEvents.PlayerGaveSlap -= OnPlayerGaveSlap;
			RPSGameEvents.NpcGaveSlap -= OnNpcGaveSlap;
		}


		private void Start()
		{
			_characterRefBank = GameObject.FindWithTag("PlayerCharacter").GetComponent<CharacterRefBank>();
			_npcRefBank = GameObject.FindWithTag("NpcCharacter").GetComponent<NpcRefBank>();
		}

		public void OnSpitWaterAnimation()
		{
			if (_npcSpit)
				OnNpcSpit();

			if (_playerSpit)
				OnPlayerSpit();

		}

		private void OnNpcSpit()
		{
			_npcRefBank.Controller.OnNpcSpit();
		}

		private void OnPlayerSpit()
		{
			_characterRefBank.Controller.OnPlayerSpit();
		}


		public void OnMoveAnimation()
		{
			if (!gameObject.CompareTag("PlayerCharacter")) return;

			RPSGameEvents.InvokeOnGameMovesTextEnable();
			RPSAudioManager.instance.Play("MovesDone");
			DOVirtual.DelayedCall(0.7f, () => GameMoveCheck.only.CheckMoves());
		}


		private void OnPlayerGaveSlap()
		{
			_npcSpit = true;
			_playerSpit = false;
		}

		private void OnNpcGaveSlap()
		{
			_npcSpit = false;
			_playerSpit = true;
		}

	}
}
