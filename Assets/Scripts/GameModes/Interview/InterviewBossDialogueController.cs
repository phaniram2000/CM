using System;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class InterviewBossDialogueController : MonoBehaviour
{
    [Multiline] [SerializeField] private List<String> interviewBossDialoguesBeforeJoining;
    [Multiline] [SerializeField] private String afterJoiningBossDialogue;
    [SerializeField] private TextMeshPro dialogueText;
    [SerializeField] private GameObject dialogueGameObject;
    [SerializeField] private float dialogueDuration,dialogueGameobjectScale;
    private int count=0;
    private bool canLoopDialogue;

    private InterviewBossController _bossController;


    private void OnEnable()
    {
        InterviewEvents.BossRotationDone += OnBossRotationDone;
        InterviewEvents.DoneButtonPressed += OnDoneButtonPressed;
    }

    private void OnDisable()
    {
        InterviewEvents.BossRotationDone -= OnBossRotationDone;
        InterviewEvents.DoneButtonPressed -= OnDoneButtonPressed;
    }

    
    private void Start()
    {
        count = 0;
        DisableDialogueGameObject();
        _bossController = GetComponent<InterviewBossController>();
        canLoopDialogue = true;

    }


    private void DisableDialogueGameObject()
    {
        dialogueGameObject.SetActive(false);
    }

    private void EnableDialogueGameObject()
    {
        print("Dialoges enabled");
        dialogueGameObject.transform.localScale = Vector3.zero;
        dialogueGameObject.SetActive(true);
        dialogueGameObject.transform.DOScale(Vector3.one * dialogueGameobjectScale, 0.6f).SetEase(Ease.InBack)
            .OnComplete(()=>
            {
                if (!canLoopDialogue) return;

                if (AudioManager.instance)
                {
                    AudioManager.instance.Play("Welcome");
                    dialogueDuration =  AudioManager.instance.GetClipLength("Welcome");
                }
                    
                
                _bossController.SetWelcomeTrigger();
                InterviewBeforeJoining();
            });
    }



    private void GetDialogueForInterviewBeforeJoining()
    {
        String dial = interviewBossDialoguesBeforeJoining[count];
        dialogueText.text = dial;
    }

    private void InterviewBeforeJoining()
    {
        
        GetDialogueForInterviewBeforeJoining();

        if (count == 1)
        {
            _bossController.SetTalkingTrigger();
            if (AudioManager.instance)
            {
                AudioManager.instance.Play("Thanks");
                dialogueDuration= AudioManager.instance.GetClipLength("Thanks");
            }
                
        }

        if (count == 2)
        {
            if (AudioManager.instance)
            {
                AudioManager.instance.Play("OfferLetter");
                dialogueDuration= AudioManager.instance.GetClipLength("OfferLetter");
                print("Dialoges done");
            }
        }


        DOVirtual.DelayedCall(dialogueDuration, () =>
        {

            count++;
            if (count >= interviewBossDialoguesBeforeJoining.Count)
            {
                DisableDialogueGameObject();
                _bossController.SetGiveTrigger();
                DOVirtual.DelayedCall(1.3f, () => InterviewEvents.InvokeOnMoveCheckToPlayer());
                canLoopDialogue = false;
                return;
            }
            
            InterviewBeforeJoining();
                
        });


    }
    
    private void OnBossRotationDone()
    {
        DOVirtual.DelayedCall(0.5f,()=>EnableDialogueGameObject());
        GetDialogueForInterviewBeforeJoining();
    }

    private void CongratsDialogue()
    {
        dialogueText.text = afterJoiningBossDialogue;
        EnableDialogueGameObject();
    }
    
    private void OnDoneButtonPressed()
    {
        DOVirtual.DelayedCall(0.5f, () =>
        {
            
            
            CongratsDialogue();
            PlayCongratsDialogue();
            
            
        });
    }

    private void PlayCongratsDialogue()
    {
        DOVirtual.DelayedCall(0.7f, () =>
        {
            if (AudioManager.instance)
            {
                float duration = 0;
                AudioManager.instance.Play("Congrats");
                duration = AudioManager.instance.GetClipLength("Congrats");

                DOVirtual.DelayedCall(duration, () =>
                {
                    InterviewEvents.InvokeOnBonusGiven();
                    
                   
                });
            }
        });
    }
}
