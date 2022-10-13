using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace RPS
{

	public class GameMovesUIController : MonoBehaviour
	{
		[SerializeField] private RectTransform gameMovesUiPanel;
		[SerializeField] private Button rockButton, paperButton, scissorButton;

		private void OnEnable()
		{
			RPSGameEvents.CameraZoom += OnCameraZoom;
			RPSGameEvents.NewRound += OnNewRound;
			RPSGameEvents.OnTapToPlay += OnTapToPlay;
			RPSGameEvents.GameLose += OnGameLose;
			RPSGameEvents.GameWin += OnGameWin;

		}

		private void OnDisable()
		{
			RPSGameEvents.CameraZoom -= OnCameraZoom;
			RPSGameEvents.NewRound -= OnNewRound;
			RPSGameEvents.OnTapToPlay -= OnTapToPlay;
			RPSGameEvents.GameLose -= OnGameLose;
			RPSGameEvents.GameWin -= OnGameWin;
		}


		private void Start()
		{
			gameMovesUiPanel.localScale = Vector3.zero;
		}

		public void OnRockSelected()
		{
			RPSGameEvents.InvokeOnMoveSelectedByPlayer(GameMoves.Rock);
			DisableGameMovesUiButton();
			rockButton.transform.GetChild(0).GetComponent<Image>().color = Color.green;


			RPSAudioManager.instance.Play("MoveButtonPress");
			Vibration.Vibrate(30);
		}

		public void OnPaperSelected()
		{
			RPSGameEvents.InvokeOnMoveSelectedByPlayer(GameMoves.Paper);
			DisableGameMovesUiButton();
			paperButton.transform.GetChild(0).GetComponent<Image>().color = Color.green;

			RPSAudioManager.instance.Play("MoveButtonPress");
			Vibration.Vibrate(30);
		}

		public void OnScissorSelected()
		{
			RPSGameEvents.InvokeOnMoveSelectedByPlayer(GameMoves.Scissor);
			DisableGameMovesUiButton();
			scissorButton.transform.GetChild(0).GetComponent<Image>().color = Color.green;

			RPSAudioManager.instance.Play("MoveButtonPress");
			Vibration.Vibrate(30);
		}


		private void EnableGameMovesUiButton()
		{
			rockButton.interactable = true;
			paperButton.interactable = true;
			scissorButton.interactable = true;
		}

		private void DisableGameMovesUiButton()
		{
			rockButton.interactable = false;
			paperButton.interactable = false;
			scissorButton.interactable = false;
		}

		private void OnCameraZoom()
		{
			GameMovesUiDisableAnimation();
			DefaultColorOfUiButtons();
		}

		private void OnNewRound()
		{
			GameMovesUiEnableAnimation();
			DefaultColorOfUiButtons();
		}

		private void GameMovesUiEnableAnimation()
		{
			gameMovesUiPanel.DOScale(Vector3.one, 0.25f).SetEase(Ease.OutBack).OnComplete(EnableGameMovesUiButton);
		}

		private void GameMovesUiDisableAnimation()
		{
			gameMovesUiPanel.localScale = Vector3.one;
			gameMovesUiPanel.DOScale(Vector3.zero, 0.25f).SetEase(Ease.InBack);
		}

		private void OnTapToPlay()
		{
			GameMovesUiEnableAnimation();
		}

		private void OnGameLose()
		{
			gameMovesUiPanel.localScale = Vector3.zero;
			gameMovesUiPanel.gameObject.SetActive(false);
		}

		private void OnGameWin()
		{
			gameMovesUiPanel.localScale = Vector3.zero;
			gameMovesUiPanel.gameObject.SetActive(false);
		}

		private void DefaultColorOfUiButtons()
		{
			rockButton.transform.GetChild(0).GetComponent<Image>().color = Color.white;
			paperButton.transform.GetChild(0).GetComponent<Image>().color = Color.white;
			scissorButton.transform.GetChild(0).GetComponent<Image>().color = Color.white;

		}

	}

}
