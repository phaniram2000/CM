using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class InterviewCanvasController : MonoBehaviour
{
    [SerializeField] private GameObject doneButton;

    private void OnEnable()
    {
        InterviewEvents.SignDone += OnSignDone;
    }

    private void OnDisable()
    {
        InterviewEvents.SignDone -= OnSignDone;
    }


    private void Start()
    {
        doneButton.SetActive(false);
    }

    private void OnSignDone()
    {
        doneButton.SetActive(true);
    }

    public void OnDoneButtonPressed()
    {
        
        DOVirtual.DelayedCall(0.1f,()=>doneButton.SetActive(false));
        doneButton.GetComponent<Button>().interactable = false;
        
        InterviewEvents.InvokeOnDoneButtonPressed();
        
        if(AudioManager.instance)
            AudioManager.instance.Play("Button");
        
        Vibration.Vibrate(30);
        
        
    }
}
