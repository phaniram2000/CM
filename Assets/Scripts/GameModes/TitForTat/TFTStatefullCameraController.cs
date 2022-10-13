using DG.Tweening;
using UnityEngine;

public class TFTStatefullCameraController : MonoBehaviour
{
    private Animator _anim;
    	
    	
    	private static readonly int Win = Animator.StringToHash("win");
        private static readonly int SecondSceneCam = Animator.StringToHash("secondscenecam");
        private static readonly int Gameplay = Animator.StringToHash("gameplay");
        private static readonly int LiftZoom = Animator.StringToHash("liftzoom");
    
    	private void OnEnable()
        {
	        TFTGameEvents.ActivateNextScene += OnActivateNextScene;
	        TFTGameEvents.LiftOpenDoorsDone += OnLiftDoorsOpen;
	        TFTGameEvents.ShowGameplayScreen += OnShowGameplayScreeen;
	        TFTGameEvents.DoneButtonPressed += OnDoneButtonPressed;
        }
    
    	private void OnDisable()
    	{
	        TFTGameEvents.ActivateNextScene -= OnActivateNextScene;
	        TFTGameEvents.LiftOpenDoorsDone -= OnLiftDoorsOpen;
	        TFTGameEvents.ShowGameplayScreen -= OnShowGameplayScreeen;
	        TFTGameEvents.DoneButtonPressed -= OnDoneButtonPressed;
    	}

        private void Start()
    	{
    		_anim = GetComponent<Animator>();
    		
    	}
        
        private void OnActivateNextScene() => _anim.SetTrigger(SecondSceneCam);


        private void OnGameWin() => _anim.SetTrigger(Win);
        
        
        private void OnLiftDoorsOpen(float obj)
        {
	        DOVirtual.DelayedCall(obj + 0.2F, () => OnLiftZoom());
        }

        private void OnLiftZoom()
        {
	        _anim.SetTrigger(LiftZoom);
        }
        
        private void OnShowGameplayScreeen()
        {
	        _anim.SetTrigger(Gameplay);
        }
        
        private void OnDoneButtonPressed()
        {
	        _anim.SetTrigger(SecondSceneCam);
        }


}
