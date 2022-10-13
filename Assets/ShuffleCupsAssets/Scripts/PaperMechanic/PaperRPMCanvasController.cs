using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;


namespace ShuffleCups
{
	public class PaperRPMCanvasController : MonoBehaviour
    {
    	[SerializeField] private Color colorTo, colorFrom;
    	private float _remapMin, _remapMax;
    	
    	[SerializeField] private Image image, emoji, triangle;
    	[SerializeField] private Sprite[] emojis;
    	
    	private void OnEnable()
    	{
    		PaperGameEvents.Singleton.pullPaperStep += OnPullPaperStep;
    		
    		PaperGameEvents.Singleton.tearPaper += OnGameOver;
    		PaperGameEvents.Singleton.playerCrossFinishLine += OnGameOver;
    		PaperGameEvents.Singleton.aiCrossFinishLine += OnGameOver;
    	}
    
    	private void OnDisable()
    	{
    		PaperGameEvents.Singleton.pullPaperStep -= OnPullPaperStep;
    		
    		PaperGameEvents.Singleton.tearPaper -= OnGameOver;
    		PaperGameEvents.Singleton.playerCrossFinishLine -= OnGameOver;
    		PaperGameEvents.Singleton.aiCrossFinishLine -= OnGameOver;
    	}
    	
    	private void Start()
    	{
    		_remapMin = PaperLevelFlowController.only.Player.GetMaxRpm * PaperLevelFlowController.only.warningPercentageRpm;
    		_remapMax = PaperLevelFlowController.only.Player.GetMaxRpm * PaperLevelFlowController.only.deadPercentageRpm;
    	}
    
    	private void OnPullPaperStep() => UpdateSlider();
    	
    	private void UpdateSlider()
    	{
    		image.color = GameExtensions.RemapColor(_remapMin, _remapMax, colorTo, colorFrom, PaperLevelFlowController.only.Player.GetCurrentRpm);
    		var value = Mathf.InverseLerp(0, PaperLevelFlowController.only.Player.GetMaxRpm, PaperLevelFlowController.only.Player.GetCurrentRpm);
    
    		image.fillAmount = value;
    		triangle.rectTransform.anchorMin = Vector2.right * (value - 0.005f);
    		triangle.rectTransform.anchorMax = Vector2.right * (value + 0.005f);
    		
    		var emojiVal = GameExtensions.Remap(_remapMin, _remapMax, 0f, 1f,
    			PaperLevelFlowController.only.Player.GetCurrentRpm);
    
    		if (emojiVal < 0)
    			emoji.sprite = emojis[0];
    		else if (emojiVal < 1f)
    			emoji.sprite = emojis[1];
    		else
    			emoji.sprite = emojis[2];
    	}
    
    	private void OnGameOver()
    	{
    		DOTween.Sequence().AppendInterval(5f).AppendCallback(() => gameObject.SetActive(false));
    	}
    }
}


