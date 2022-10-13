using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ATMButtonAndSliderCanvasController : MonoBehaviour
{
    [SerializeField] private GameObject buttonPanels,moneySlider,moneyTextGameObject,withdrawlGameObject,instructionGameObject;
    [SerializeField] private TextMeshProUGUI moneyText;
    [SerializeField] private Slider withdrawlSlider;


    private void OnEnable()
    {
        ATMEvents.EnterAtmPinGamePlay += OnEnterAtmPinGamePlay;
        ATMEvents.EnableMoneySlider += EnableSliderPanel;
    }

    private void OnDisable()
    {
        ATMEvents.EnterAtmPinGamePlay -= OnEnterAtmPinGamePlay;
        ATMEvents.EnableMoneySlider -= EnableSliderPanel;
    }

    
    private void Start()
    {
        DisableButtonPanel();
        DisableSliderPanel();
        moneyTextGameObject.SetActive(false);
        withdrawlGameObject.SetActive(false);
        instructionGameObject.SetActive(false);
       
    }

    private void EnableButtonsPanel()
    {
        buttonPanels.transform.localScale=Vector3.zero;
        buttonPanels.SetActive(true);
        buttonPanels.transform.DOScale(Vector3.one, 0.7f).SetEase(Ease.InBack);

    }
    
    private void EnableSliderPanel()
    {
        moneySlider.transform.localScale=Vector3.zero;
        moneySlider.SetActive(true);
        moneySlider.transform.DOScale(Vector3.one, 0.7f).SetEase(Ease.InBack);
        
        moneyTextGameObject.SetActive(true);
        withdrawlGameObject.SetActive(true);
        instructionGameObject.SetActive(true);
       

    }
    
    private void DisableSliderPanel()
    {
        moneySlider.SetActive(false);
        
    }

    private void DisableButtonPanel()
    {
        buttonPanels.SetActive(false);
    }
    
    private void OnEnterAtmPinGamePlay()
    {
       EnableButtonsPanel();
    }

    public void OnRightAnswerPressed()
    {
        ATMEvents.InvokeOnRightAnswerPressed();
        DisableButtonPanel();
        
        AudioManager.instance.Play("Button");
        
        Vibration.Vibrate(30);
    }

    public void OnWrongAnswerPressed()
    {
        ATMEvents.InvokeOnWorngAnswerPressed();
        DisableButtonPanel();

        DOVirtual.DelayedCall(0.8f, () => GameEvents.InvokeGameLose(-1));
        
        AudioManager.instance.Play("Button");
        
        Vibration.Vibrate(30);
    }

    public void MoneyValue(float value)
    {
        moneyText.text = value.ToString();
    }

    public void WithDrawlButtonPressed()
    {
        withdrawlGameObject.SetActive(false);
        moneyTextGameObject.SetActive(false);
        instructionGameObject.SetActive(false);
        DisableSliderPanel();
        
        ATMEvents.InvokeOnWithDrawlButtonPressed();

        DOVirtual.DelayedCall(1, () => GameEvents.InvokeGameWin());
        
        AudioManager.instance.Play("Button");
        Vibration.Vibrate(30);
    }


}
