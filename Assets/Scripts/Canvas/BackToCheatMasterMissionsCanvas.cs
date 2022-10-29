using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BackToCheatMasterMissionsCanvas : MonoBehaviour
{

	[SerializeField] private Button backToMissionsButton;

	private void OnEnable()
	{
		MemoryBetGameEvents.WrongAnswer += DisableBackButton;
	}

	private void OnDisable()
	{
		MemoryBetGameEvents.WrongAnswer -= DisableBackButton;
	}


	private void DisableBackButton()
	{
		backToMissionsButton.interactable = false;
	}


	public void OnBackToMissionsPressed()
	{
		DisableBackButton();
		
		if(AudioManager.instance)
			AudioManager.instance.Play("Button");
		
		Vibration.Vibrate(30);
		
		int previousMissionIndex = PlayerPrefs.GetInt("lastBuildIndex", 2);
		SceneManager.LoadScene(previousMissionIndex);
	}

	public void ResetValues()
	{
		MemoryBetGameEvents.InvokeOnResetValue();
	}
}