using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;


namespace RPS
{

	public class HelmetCanvasController : MonoBehaviour
	{
		[SerializeField] private RectTransform helmetButtonRect;
		[SerializeField] private Button helmetButton;
		[SerializeField] private GameObject helmetTextGameObject;
		[SerializeField] private bool showTutorial,enableHelmet;

		private bool _helemtEnable;

		private void OnEnable()
		{
			RPSGameEvents.OnTapToPlay += OnTapToPlay;
			RPSGameEvents.PlayerStartGiveSlap += OnPlayerStartGiveSlap;
			RPSGameEvents.MoveSelectedByPlayer += OnMoveSelectedByPlayer;
			RPSGameEvents.CameraZoom += OnCameraZoom;
			RPSGameEvents.NewRound += OnNewRound;
			RPSGameEvents.GameWin += OnGameWin;
			RPSGameEvents.GameLose += OnGameLose;
		}

		private void OnDisable()
		{
			RPSGameEvents.OnTapToPlay -= OnTapToPlay;
			RPSGameEvents.PlayerStartGiveSlap -= OnPlayerStartGiveSlap;
			RPSGameEvents.MoveSelectedByPlayer -= OnMoveSelectedByPlayer;
			RPSGameEvents.CameraZoom -= OnCameraZoom;
			RPSGameEvents.NewRound -= OnNewRound;
			RPSGameEvents.GameWin -= OnGameWin;
			RPSGameEvents.GameLose -= OnGameLose;
		}

		private void Start()
		{
			helmetButtonRect.localScale = Vector3.zero;
			helmetTextGameObject.SetActive(false);
		}

		private void OnTapToPlay()
		{
			if (!enableHelmet) return;
			
			helmetButtonRect.DOScale(Vector3.one, 0.25f).SetEase(Ease.OutBack).OnComplete(EnableHelmetUiButton);
		}

		public void OnHelmetButtonPressed()
		{
			RPSGameEvents.InvokeOnPlayerHelmetEnable();
			HelmetButtonOutAnimation();
			_helemtEnable = true;

			if (showTutorial)
				EnableText();

			RPSAudioManager.instance.Play("MoveButtonPress");
		}

		private void EnableText()
		{
			helmetTextGameObject.SetActive(true);
		}


		private void HelmetButtonOutAnimation()
		{
			DisableHelmetUiButton();
			helmetButtonRect.DOScale(Vector3.zero, 0.25f).SetEase(Ease.OutBack);
		}


		private void EnableHelmetUiButton()
		{
			helmetButton.interactable = true;
		}

		private void DisableHelmetUiButton()
		{
			helmetButton.interactable = false;
		}


		private void OnPlayerStartGiveSlap()
		{
			helmetTextGameObject.SetActive(false);
		}

		private void OnMoveSelectedByPlayer(GameMoves obj)
		{
			helmetTextGameObject.SetActive(false);
		}

		private void OnCameraZoom()
		{
			HelmetButtonOutAnimation();
		}


		private void OnNewRound()
		{
			if (!enableHelmet) return;
			
			if (_helemtEnable) return;

			helmetButtonRect.DOScale(Vector3.one, 0.25f).SetEase(Ease.OutBack).OnComplete(EnableHelmetUiButton);

		}

		private void OnGameWin()
		{
			helmetButton.gameObject.SetActive(false);
		}

		private void OnGameLose()
		{
			helmetButton.gameObject.SetActive(false);
		}


	}

}
