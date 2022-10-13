using UnityEngine;
using UnityEngine.UI;

public class TFTLiftFloorButtonCanvasController : MonoBehaviour
{
	[SerializeField] private int totalFloorButtons;

	private int count = 0;
	
	public void OnFloorButtonsPressed(Button button)
	{
		button.interactable = false;
		button.GetComponent<Image>().color = Color.green;
		count++;
		
		if(count >= 4)
			TFTGameEvents.InvokeOnAllButtonsPressed();

		if (AudioManager.instance)
			AudioManager.instance.Play("Button");
		Vibration.Vibrate(30);
		
		if (count < totalFloorButtons) return;

		
	}
}