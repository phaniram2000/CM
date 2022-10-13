using UnityEngine;

namespace ShuffleCups
{
	public class FightAimTutorialController : MonoBehaviour
    {
    	//if input delta abs > 1f
    	[SerializeField] private GameObject[] holders;
    	[SerializeField] private float doneYDelta;
    
    	private float _currentCumulativeYDelta;
    	private bool _shouldCheck;
    
    	private void OnEnable()
    	{ 
    		PaperGameEvents.Singleton.tapToPlay += OnLevelStart;
    		
    		PaperGameEvents.Singleton.aiCrossFinishLine += OnLevelEnd;
    		PaperGameEvents.Singleton.playerCrossFinishLine += OnLevelEnd;
    		PaperGameEvents.Singleton.tearPaper += OnLevelEnd;

            global::GameEvents.TapToPlay += OnLevelStart;
        }
    
    	private void OnDisable()
    	{
    		PaperGameEvents.Singleton.tapToPlay -= OnLevelStart;
    		
    		PaperGameEvents.Singleton.aiCrossFinishLine -= OnLevelEnd;
    		PaperGameEvents.Singleton.playerCrossFinishLine -= OnLevelEnd;
    		PaperGameEvents.Singleton.tearPaper -= OnLevelEnd;
            
            global::GameEvents.TapToPlay -= OnLevelStart;
    	}
    
    	private void Start()
    	{
    		ToggleAnimations(false);
    	}
    
    	private void Update()
    	{
    		if(!_shouldCheck) return;
    
    		if (!InputExtensions.GetFingerHeld()) return;
    		
    		if(InputExtensions.GetInputDelta().y > 0f) return;
    		
    		_currentCumulativeYDelta += Mathf.Abs(InputExtensions.GetInputDelta().y);
    		
    		if (_currentCumulativeYDelta < doneYDelta) return;
    		
    		ToggleAnimations(false);
    	}
    
    	private void ToggleAnimations(bool status)
    	{
    		foreach (var held in holders)
    			held.SetActive(status);
    
    		_shouldCheck = status;
    	}
    
    	private void OnLevelStart()
    	{
    		ToggleAnimations(true);
    	}
    	
    	private void OnLevelEnd()
    	{
    		gameObject.SetActive(false);
    	}
    }
}

