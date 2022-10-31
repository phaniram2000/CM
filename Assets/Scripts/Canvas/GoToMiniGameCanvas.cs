using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GoToMiniGameCanvas : MonoBehaviour
{
	[SerializeField] private Button miniGameButton;
	[SerializeField] private Image image,finger;
	[SerializeField] private int indexNumber;

	private readonly String MiniGame = "Memory Bet";

	private void OnEnable()
	{
		GameEvents.TapToPlay += OnTapToPlay;
	}

	private void OnDisable()
	{
		GameEvents.TapToPlay -= OnTapToPlay;
	}

	private void Start()
	{
		int index = SceneManager.GetActiveScene().buildIndex;
		if (index < indexNumber) return;

		miniGameButton.enabled = true;
		image.enabled = true;
		if (finger)
		{
			finger.enabled = true;
		}
	}


	public void OnGoToMiniGamePressed()
	{
		
		if(AudioManager.instance)
			AudioManager.instance.Play("Button");
		
		Vibration.Vibrate(30);
		
		miniGameButton.interactable = false;
		SceneManager.LoadScene(MiniGame);
		
	}
	
	private void OnTapToPlay()
	{
		miniGameButton.transform.DOScale(Vector3.zero, 0.1f).SetEase(Ease.InBack);
	}
}