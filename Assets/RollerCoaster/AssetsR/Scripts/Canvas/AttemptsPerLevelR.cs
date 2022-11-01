
using UnityEngine.SceneManagement;

public static class AttemptsPerLevelR
{
	public static int CurrentAttempts { get; private set; }
	private static int _currentBuildIndex = -1;

	public static void OnSceneLoaded(Scene scene, LoadSceneMode arg1)
	{
		if (scene.buildIndex == _currentBuildIndex)
			CurrentAttempts++;
		else
		{
			_currentBuildIndex = scene.buildIndex;
			CurrentAttempts = 0;
		}
	}
}