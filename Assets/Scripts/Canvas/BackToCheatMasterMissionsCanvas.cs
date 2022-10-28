using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BackToCheatMasterMissionsCanvas : MonoBehaviour
{

	[SerializeField] private Button backToMissionsButton;
	public void OnBackToMissionsPressed()
	{
		backToMissionsButton.interactable = false;
		int previousMissionIndex = PlayerPrefs.GetInt("lastBuildIndex", 2);
		SceneManager.LoadScene(previousMissionIndex);
	}
}