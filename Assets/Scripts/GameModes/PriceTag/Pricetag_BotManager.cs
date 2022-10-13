using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Pricetag_BotManager : MonoBehaviour
{
    public Animator bot1, bot2;
    public Transform billboy;
    public Transform movepoint1, movepoint2, movepoint3;
    public static event Action ondone;

    // Start is called before the first frame update
    void OnEnable()
    {
        GameEvents.TapToPlay += OnTapToPlay;
    }

    private void OnDisable()
    {
        GameEvents.TapToPlay -= OnTapToPlay;
    }

    private void OnTapToPlay()
    {
        bot1Seqence();
    }

    public void bot1Seqence()
    {
        var seq = DOTween.Sequence();
        seq.AppendCallback(() =>
        {
            bot1.SetTrigger("Walk"); 
           
            
        });
        seq.Append(bot1.transform.DOMove(movepoint1.position, 6).SetEase(Ease.Linear));
        seq.AppendCallback(() =>
        {
            bot1.SetTrigger("Think"); 
           
            
        });
        seq.AppendCallback(() =>
        {
            bot2.SetTrigger("Walk"); 
           
            
        });
        seq.Append(bot2.transform.DOLookAt(billboy.position, .2f).SetEase(Ease.Linear));
        seq.Append(bot2.transform.DOMove(movepoint2.position, 3).SetEase(Ease.Linear));
        seq.AppendCallback(() =>
        {
            bot2.SetTrigger("Idle"); 
           
            
        });
        seq.AppendInterval(3);
        seq.AppendCallback(() =>
        {
            bot2.SetTrigger("Walk"); 
           
            
        });
        seq.Append(bot2.transform.DOLookAt(movepoint3.position, .3f).SetEase(Ease.Linear));
        seq.Append(bot2.transform.DOMove(movepoint3.position, 4f).SetEase(Ease.Linear));
        seq.AppendCallback(() =>
        {
            bot2.gameObject.SetActive(false);
           
            
        });
        
        seq.AppendCallback(() =>
        {
            bot1.SetTrigger("Walk"); 
           
            
        });
        seq.Append(bot1.transform.DOLookAt(billboy.position, .2f).SetEase(Ease.Linear));
        seq.Append(bot1.transform.DOMove(movepoint2.position, 4).SetEase(Ease.Linear));
        seq.AppendCallback(() =>
        {
            bot1.SetTrigger("Idle"); 
           
            
        });
        seq.AppendInterval(2);
        seq.AppendCallback(() =>
        {
            bot1.SetTrigger("Walk"); 
           
            
        });
        seq.Append(bot1.transform.DOLookAt(movepoint3.position, .3f).SetEase(Ease.Linear));
        seq.Append(bot1.transform.DOMove(movepoint3.position, 4f).SetEase(Ease.Linear));
        seq.AppendCallback(() =>
        {
            bot1.gameObject.SetActive(false);
           
            
        });
      
    }

   

    // Update is called once per frame
    void Update()
    {
    }
}