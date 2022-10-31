using DG.Tweening;
using UnityEngine;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

public class HandTutorial : MonoBehaviour
{
	private RectTransform _rect;
	private Image _image;

	private Tween _handTween;

	private void Start()
	{
		_rect = GetComponent<RectTransform>();
		_image = GetComponent<Image>();
		EnableHandTutorial();
	}

	private void OnEnable()
	{
		MemoryBetGameEvents.BetButtonPressed += OnBetButtonPressed;
		GameEvents.GameLose += OnGameLose;
	}

	private void OnDisable()
	{
		MemoryBetGameEvents.BetButtonPressed -= OnBetButtonPressed;
		GameEvents.GameLose -= OnGameLose;
	}

	
	private void EnableHandTutorial()
	{
		_handTween = _rect.DOAnchorPosX(203, 0.8f).SetLoops(-1, LoopType.Yoyo);
		
	}
	
	private void OnBetButtonPressed()
	{
		KillHandTween();
		_image.enabled = false;
		
	}
	
	private void OnGameLose(int obj)
	{
		KillHandTween();
		_image.enabled = false;
	}

	private void KillHandTween()
	{
		_handTween.Kill();
	}
}