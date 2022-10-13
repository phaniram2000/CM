using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class CheatingCanvas : MonoBehaviour
{
	[SerializeField] private GameObject cheatingCanvas;
	[SerializeField] private Image cheatingFillImage;
	[SerializeField] private float fillMultiplier;
	[SerializeField] private TMP_Text gradingText;
	private int _fillAmount;

	private bool _traversed;
	private int _totalFills = 3;
	private string[] _grades = { "A+", "A", "B" };

	private void OnEnable()
	{
		CheatingEvents.CheaterFound += HideTheCanvas;
		CheatingEvents.DoneCheating += HideTheCanvas;
		GameEvents.TapToPlay += OnTapToPlay;
	}

	private void OnDisable()
	{
		CheatingEvents.CheaterFound -= HideTheCanvas;
		CheatingEvents.DoneCheating -= HideTheCanvas;
		GameEvents.TapToPlay -= OnTapToPlay;
	}
	
	private void Start()
	{
		gradingText.text = _grades[2];
	}
	
	public void FillTheImage()
	{
		cheatingFillImage.fillAmount += fillMultiplier * Time.deltaTime;
		if (_traversed) return;
		if (!(cheatingFillImage.fillAmount >= 1)) return;
		ResetFillAmount();
		
		if (_totalFills > 0) return;
		
		GameCanvas.game.MakeGameResult(0,0);
		CheatingEvents.InvokeDoneCheating();
		_traversed = true;
		if (AudioManager.instance)
		{
			AudioManager.instance.Play("NiceFemale");
		}

	}

	private void HideTheCanvas()
	{
		cheatingCanvas.SetActive(false);
	}

	private void OnTapToPlay()
	{
		cheatingCanvas.SetActive(false);
	}

	private void ResetFillAmount()
	{
		cheatingFillImage.fillAmount = 0f;
		_totalFills--;
		if (_totalFills == 0) return;
		gradingText.text = _grades[_totalFills - 1];
		Vibration.Vibrate(10);
	}

}
