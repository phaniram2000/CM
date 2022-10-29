using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class KissingCanvas : MonoBehaviour
{
	[SerializeField] private GameObject heartPanel;
	[SerializeField] private Image heartImage;
	[SerializeField] private TMP_Text heartFillText;
	[SerializeField] private float fillMultiplier = 0.001f;
	
	private void OnEnable()
	{
		KissingEvents.GotFoundKissing += DisableFillImage;
		KissingEvents.FooledFather += DisableFillImage;
	}

	private void OnDisable()
	{
		KissingEvents.GotFoundKissing -= DisableFillImage;
		KissingEvents.FooledFather -= DisableFillImage;
	}

	private void Start()
	{
		heartImage.fillAmount = 0f;
		heartFillText.text = "0" + "%";
	}
	public void StartFillingTheHeartText()
	{
		heartImage.fillAmount += (fillMultiplier * Time.deltaTime);
		heartFillText.text = (int)(heartImage.fillAmount * 100f) + "%";

		if (!(heartImage.fillAmount >= 1f)) return;
		heartFillText.text = "100%";
		KissingEvents.InvokeFooledFather();
		GameCanvas.game.MakeGameResult(0,0);
	}

	private void DisableFillImage()
	{
		heartPanel.SetActive(false);
	}
}
