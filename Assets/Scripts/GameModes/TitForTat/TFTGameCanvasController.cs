using DG.Tweening;
using UnityEngine;

public class TFTGameCanvasController : MonoBehaviour
{
    [SerializeField] private GameObject circleTransitionGameObject,instructiongameObject,laterTextGameObject,doneButtonGameObject;
    [SerializeField] private float circleScaleValue,circleTransitionDuration;
    [SerializeField] private Ease circleTransitionEase;
    private void OnEnable()
    {
        TFTGameEvents.DoSceneTransition += OnDoSceneTransition;
        TFTGameEvents.ShowGameplayScreen += OnShowGamePlayScreen;
        TFTGameEvents.AllButtonsPressed += OnAllButtonsPressed;
    }

    private void OnDisable()
    {
        TFTGameEvents.DoSceneTransition -= OnDoSceneTransition;
        TFTGameEvents.ShowGameplayScreen -= OnShowGamePlayScreen;
        TFTGameEvents.AllButtonsPressed -= OnAllButtonsPressed;
    }

    private void Start()
    {
        circleTransitionGameObject.transform.localScale = Vector3.zero;
        instructiongameObject.SetActive(false);
        laterTextGameObject.SetActive(false);
        doneButtonGameObject.SetActive(false);
        InstructionsOut();
    }

    private void OnDoSceneTransition()
    {
        OutSceneTransition();
    }

    private void OutSceneTransition()
    {
        circleTransitionGameObject.transform.DOScale(Vector3.one * circleScaleValue, circleTransitionDuration)
            .SetEase(circleTransitionEase).OnComplete(() =>
            {
                laterTextGameObject.SetActive(true);
                DOVirtual.DelayedCall(1.2f, () =>
                {
                    laterTextGameObject.SetActive(false);
                    TFTGameEvents.InvokeOnActivateNextScene();
                    InSceneTransition();
                });
                
            });
    }

    private void InSceneTransition()
    {
        circleTransitionGameObject.transform.DOScale(Vector3.zero, circleTransitionDuration);
    }
    
    private void OnShowGamePlayScreen()
    {
        DOVirtual.DelayedCall(0.2f,()=>InstructionsIn());
    }

    private void InstructionsIn()
    {
        instructiongameObject.SetActive(true);
        instructiongameObject.GetComponent<RectTransform>().DOAnchorPosX(-3f, 0.6f).SetEase(Ease.InElastic);
    }

    private void InstructionsOut()
    {
        instructiongameObject.GetComponent<RectTransform>().DOAnchorPosX(-387f, 0.6f).SetEase(Ease.InElastic);
    }
    
    private void DoneButtonIn()
    {
        doneButtonGameObject.SetActive(true);
        doneButtonGameObject.GetComponent<RectTransform>().DOAnchorPosX(-27f, 0.6f).SetEase(Ease.InElastic);
    }

    private void DoneButttonOut()
    {
       doneButtonGameObject.GetComponent<RectTransform>().DOAnchorPosX(387f, 0.6f).SetEase(Ease.InElastic);
    }
    
    private void OnAllButtonsPressed()
    {
       DoneButtonIn();
    }

    public void OnDoneButtonPressed()
    {
        DoneButttonOut();
        InstructionsOut();
        TFTGameEvents.InvokeOnDoneButtonPressed();
        
        if(AudioManager.instance)
            AudioManager.instance.Play("Button");
        
        Vibration.Vibrate(30);
    }



}
