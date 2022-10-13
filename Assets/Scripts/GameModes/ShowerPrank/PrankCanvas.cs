using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class PrankCanvas : MonoBehaviour
{
	[SerializeField] private GameObject fillPanel;
	[SerializeField] private Image prankFillImage;
	[SerializeField] private float fillMultiplier = 1f;

	private void OnEnable()
	{
		ShowerPrankEvents.DonePranking += DisableFillPanel;
		ShowerPrankEvents.GotFoundPranking += DisableFillPanel;
		GameEvents.TapToPlay += OnTapToPlay;
	}

	private void OnDisable()
	{
		ShowerPrankEvents.DonePranking -= DisableFillPanel;
		ShowerPrankEvents.GotFoundPranking -= DisableFillPanel;
		GameEvents.TapToPlay -= OnTapToPlay;
	}

	private void OnTapToPlay()
	{
		fillPanel.SetActive(true);
	}

	private void Start()
	{
		prankFillImage.fillAmount = 0f;
	}

	public void FillTheImage()
	{
		if (prankFillImage.fillAmount >= 1)
		{
			print("inv");
			ShowerPrankEvents.InvokeDonePranking();
			DOVirtual.DelayedCall(2f, () => GameCanvas.game.MakeGameResult(0,0));
			// GameCanvas.game.MakeGameResult(0,0);
			return;
		}
		prankFillImage.fillAmount += Time.deltaTime * fillMultiplier;
	}

	private void DisableFillPanel()
	{
		fillPanel.SetActive(false);
	}
	
}
