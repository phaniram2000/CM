using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PhotosCanvas : MonoBehaviour
{
	[SerializeField] private Button shareButton;
	[SerializeField] private List<int> requiredSelected;

	private readonly List<bool> _buttonSelected = new(6);
	private const int MaxImagesSelected = 4;

	private void Start()
	{
		for (var i = 0; i < 6; i++) _buttonSelected.Add(false);
		shareButton.interactable = false;
	}

	public void ClickOnPhoto(Image check)
	{
		if(AudioManager.instance)
			AudioManager.instance.Play("Button");
		
		Vibration.Vibrate(30);
		
		var siblingIndex = check.transform.parent.parent.GetSiblingIndex();

		_buttonSelected[siblingIndex] = !_buttonSelected[siblingIndex];
		
		check.enabled = _buttonSelected[siblingIndex];

		shareButton.interactable = _buttonSelected.Count(isImageSelected => isImageSelected) == MaxImagesSelected;
	}

	public void ClickOnShare()
	{
		if(AudioManager.instance)
			AudioManager.instance.Play("Button");
		
		
		Vibration.Vibrate(30);
		
		var result = !_buttonSelected.Where((t, i) => t && !requiredSelected.Contains(i)).Any();
		
		if(result)
		{
			PasscodePictureEvents.InvokePicturesShared(result);
			GameEvents.InvokeGameWin();
		}
		else
			GameEvents.InvokeGameLose(-1);
	}
}