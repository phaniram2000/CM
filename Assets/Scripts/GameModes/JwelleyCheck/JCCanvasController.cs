using DG.Tweening;
using UnityEngine;

public class JCCanvasController : MonoBehaviour
{
    [SerializeField] private GameObject buttonPanels,tapAndHoldInstructionGameObject;

    private bool tapAndHoldInstructionShownDone = false;
    
    private void OnEnable()
    {
        JCEvents.JwelleryMeterCheckDone += EnableButtonPanels;
        JCEvents.AllowToCheckJwellery += OnAllowToCheckJwellery;
        JCEvents.JwelleryMeterCheckDone += OnJwelleryMeterCheckDone;
    }

    private void OnDisable()
    {
        JCEvents.JwelleryMeterCheckDone -= EnableButtonPanels;
        JCEvents.AllowToCheckJwellery -= OnAllowToCheckJwellery;
        JCEvents.JwelleryMeterCheckDone -= OnJwelleryMeterCheckDone;
    }

    private void Start()
    {
        DisableButtonPanels();
        DisableTapAndHoldInstruction();
    }

    private void EnableButtonPanels()
    {
        if (buttonPanels.activeInHierarchy) return;
        
        buttonPanels.transform.localScale = Vector3.zero;
        buttonPanels.SetActive(true);
        buttonPanels.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.InBack);
    }
    
    private void DisableButtonPanels()
    {
        buttonPanels.SetActive(false);
    }

    public void OnFakeButtonPressed()
    {
        JCEvents.InvokeOnFakeSelected();

        DOVirtual.DelayedCall(0.2f, () => DisableButtonPanels());
        
        if(AudioManager.instance)
            AudioManager.instance.Play("Button");
        
        Vibration.Vibrate(30);
        
    }

    public void OnRealButtonPressed()
    {
        JCEvents.InvokeOnRealSelected();
        
        DOVirtual.DelayedCall(0.2f, () => DisableButtonPanels());
        
        if(AudioManager.instance)
            AudioManager.instance.Play("Button");
        
        Vibration.Vibrate(30);
    }

    private void EnableTapAndHoldInstruction()
    {
        tapAndHoldInstructionGameObject.SetActive(true);
    }

    private void DisableTapAndHoldInstruction()
    {
        tapAndHoldInstructionGameObject.SetActive(false);
    }
    
    private void OnAllowToCheckJwellery()
    {
        if (tapAndHoldInstructionShownDone) return;
        
        EnableTapAndHoldInstruction();
    }
    
    private void OnJwelleryMeterCheckDone()
    {

        tapAndHoldInstructionShownDone = true;
        DisableTapAndHoldInstruction();
    }


}
