using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuScript : MonoBehaviour
{
	[SerializeField] private Image splash;
	[SerializeField] private float splashTime = 1.5f;
	
	private void Start()
    {
        if (!PlayerPrefs.HasKey("lastBuildIndex"))
        {
            PlayerPrefs.SetInt("lastBuildIndex", 2);
            PlayerPrefs.SetInt("levelNo", 1);
        }

		DOVirtual.DelayedCall(splashTime, () => 
			splash.DOColor(Color.black, 0.25f)
				.OnComplete(() => UnityEngine.SceneManagement.SceneManager.LoadScene(1)));
		
    }
}
