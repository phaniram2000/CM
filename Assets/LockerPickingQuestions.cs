using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LockerPickingQuestions : MonoBehaviour
{
	[SerializeField] private int correctOptionsCount = 0;

	[SerializeField] private List<Button> options;

	[SerializeField] private GameObject questionPanel;

	private void OnEnable()
	{
		BlackmailingEvents.ToNextGamePhase += NextPhase;
	}

	private void OnDisable()
	{
		BlackmailingEvents.ToNextGamePhase -= NextPhase;
	}

	private void Start()
	{
		questionPanel.SetActive(false);
	}
	
	public void CorrectOptionButton()
	{
		correctOptionsCount += 1;
		// EventSystem.current.currentSelectedGameObject.GetComponent<Button>().interactable = false;
		if (correctOptionsCount >= 4)
		{
			BlackmailingEvents.InvokeFinalWin();
			GameEvents.InvokeGameWin();
			return;
		}
		// foreach (var t in options)
		// {
		// 	t.interactable = false;
		// }
	}

	public void WrongOptionButton()
	{
		Debug.Log("cry");
		GameEvents.InvokeGameLose(-1);
		foreach (var t in options)
		{
			t.interactable = false;
		}
	}

	private void NextPhase()
	{
		questionPanel.SetActive(true);
	}
}
