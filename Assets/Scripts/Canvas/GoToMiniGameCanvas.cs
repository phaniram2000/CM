using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GoToMiniGameCanvas : MonoBehaviour
{
	[SerializeField] private Button miniGameButton;

	private readonly String MiniGame = "Memory Bet";

	private void OnEnable()
	{
		GameEvents.TapToPlay += OnTapToPlay;
	}

	private void OnDisable()
	{
		GameEvents.TapToPlay -= OnTapToPlay;
	}

	
	public void OnGoToMiniGamePressed()
	{
		miniGameButton.interactable = false;
		SceneManager.LoadScene(MiniGame);
	}
	
	private void OnTapToPlay()
	{
		miniGameButton.transform.DOScale(Vector3.zero, 0.1f).SetEase(Ease.InBack);
	}
}