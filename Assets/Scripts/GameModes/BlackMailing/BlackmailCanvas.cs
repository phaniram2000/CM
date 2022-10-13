using TMPro;
using UnityEngine;

public class BlackmailCanvas : MonoBehaviour
{
	[SerializeField] private TMP_Text countText;


	private void Start()
	{
		countText.text = "8";
	}
	
	private void UpdateCountText(int count)
	{
		countText.text = count.ToString();
	}
}
