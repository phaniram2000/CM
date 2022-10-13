using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using DG.Tweening;
using Dreamteck.Splines.Primitives;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;


public class Multipul_opti : MonoBehaviour
{
    
    public Animator Cam_anim;
    public Animator playeranim;
    public Animator pranker_anim;

    
    
    
   
    //Cam anim
    private static readonly int TapToPlay = Animator.StringToHash("TapToPlay");
    private static readonly int Stay = Animator.StringToHash("stay");  
    private static readonly int scream = Animator.StringToHash("Scream");
    private static readonly int Can = Animator.StringToHash("Can");
    private static readonly int cam_canopen = Animator.StringToHash("Canopen");
    private static readonly int Pincam = Animator.StringToHash("Pinstage");
    private static readonly int Finalcam = Animator.StringToHash("Final");


   
    
    
    // player anim
    private static readonly int stopeating = Animator.StringToHash("Stopeating");
    private static readonly int eating = Animator.StringToHash("Eat");
    private static readonly int Playerscream = Animator.StringToHash("scream");
    private static readonly int canopen = Animator.StringToHash("Canopen");
    private static readonly int Pins = Animator.StringToHash("Pins");
    private static readonly int poop_eating = Animator.StringToHash("Poop"); 
    private static readonly int spider = Animator.StringToHash("Spider");

    
    
    public GameObject stage1,stage2,stage3;
    
    
    //stage 1 Chip
    public GameObject chip;
    public GameObject chipspacket;
    public bool DideatChilli;
    public Transform p1,p2,p3;
    //stage 1 Poop
    public GameObject poop;
    
    
        // Stage 2
        public GameObject drinkontable;
        public GameObject drinkinhand;
        public bool DidEatPoop;

        public bool B_canopen;
        // Stage 2(spiders)
        public Transform movepoint;
        public GameObject bugs;
        public GameObject hand;
        public bool B_spidercan;
        
        // Stage 3
        public GameObject pins;
        public GameObject balloone;
    private void OnEnable()
    {
        GameEvents.TapToPlay += OnTapToPlay;
        Hotchipsplayer.Onstage3 += stage3cam;
        Hotchipsplayer.drinkandvomit += stage3cam;
    }

    private void OnDisable()
     {
         GameEvents.TapToPlay -= OnTapToPlay;
         Hotchipsplayer.Onstage3 -= stage3cam;
         Hotchipsplayer.drinkandvomit -= stage3cam;

     }

    private void Start()
    {
        Vibration.Init();   
    }

    private void Update()
    {
        //If eat chilli in chips (stage 1)
        if (DideatChilli)
        {
            Cam_anim.SetTrigger(scream);
           
            DOVirtual.DelayedCall(1.2f, () =>
            {
                playeranim.SetTrigger(eating);
            });
            DOVirtual.DelayedCall(3.5f, () =>
            {
                playeranim.SetTrigger(stopeating);
                playeranim.SetTrigger(Playerscream);
                
               
                DOVirtual.DelayedCall(4f, () =>
                {
                    Cam_anim.SetTrigger(Can);
                    
                });
                DOVirtual.DelayedCall(5f, () =>
                {
                    stage2.gameObject.SetActive(true);
                    stage2.transform.DOScale(Vector3.one  , .1f).SetEase(Ease.OutBounce);
                });
                Debug.Log("scream");
            });
            DideatChilli = false;
            
        }
        //If eat Poop in chips (stage 1)
        else if(DidEatPoop)
        { 
            Cam_anim.SetTrigger(scream);
           
            DOVirtual.DelayedCall(1.2f, () =>
            {
                playeranim.SetTrigger(eating);
            });
            DOVirtual.DelayedCall(3.5f, () =>
            {
                playeranim.SetTrigger(stopeating);
                playeranim.SetTrigger(poop_eating);
                
                // DOVirtual.DelayedCall(1.5f, () =>
                // {
                //     if(AudioManager.instance)
                //         AudioManager.instance.Play("vomit");
                //     
                // });
               
                DOVirtual.DelayedCall(2f, () =>
                {
                    Cam_anim.SetTrigger(Can);
                    
                });
                DOVirtual.DelayedCall(3f, () =>
                {
                    stage2.gameObject.SetActive(true);
                    stage2.transform.DOScale(Vector3.one  , .1f).SetEase(Ease.OutBounce);
                });
            });
            DidEatPoop = false;
             
        }
        //If can is shaked  (stage 2)
        else if(B_canopen)
        {
            DOVirtual.DelayedCall(1.5f, () =>
            {
                Cam_anim.SetTrigger(cam_canopen);
            });
            DOVirtual.DelayedCall(1.8f, () =>
            {
                playeranim.SetTrigger(canopen);
            });
            B_canopen = false;
        }
        else if(B_spidercan)
        {
            DOVirtual.DelayedCall(1.5f, () =>
            {
                Cam_anim.SetTrigger(cam_canopen);
            });
            DOVirtual.DelayedCall(1.8f, () =>
            {
                playeranim.SetTrigger(spider);
            });
            B_spidercan = false;
        }
       
    }

    private void OnTapToPlay()
    {
        pranker_anim.enabled = true;
        Cam_anim.SetTrigger(Stay);
        DOVirtual.DelayedCall(3f, () => Cam_anim.SetTrigger(TapToPlay));
        DOVirtual.DelayedCall(4f, () =>
        {
            stage1.gameObject.SetActive(true);
            stage1.transform.DOScale(Vector3.one  , .1f).SetEase(Ease.OutBounce);
        });
        Vibration.Vibrate(100);
    }
// First Stage
    public void onchilli()
    {
        DOVirtual.DelayedCall(.3f, () =>
        {
            stage1.transform.DOScale(Vector3.zero, .3f).OnComplete(() => stage1.SetActive(false)).SetEase(Ease.OutBack);
          
        });
        playeranim.SetTrigger(stopeating);
        ChipSequence();
        Vibration.Vibrate(100);
        if(AudioManager.instance)
        AudioManager.instance.Play("Button");
    }

    public void Onpoop()
    {
        DOVirtual.DelayedCall(.3f, () =>
        {
            stage1.transform.DOScale(Vector3.zero, .3f).OnComplete(() => stage1.SetActive(false)).SetEase(Ease.OutBack);
           
        });    
        playeranim.SetTrigger(stopeating);

        PoopSequence();
        Vibration.Vibrate(100);
        if(AudioManager.instance)
        AudioManager.instance.Play("Button");

    }
    //Second Stage
    public void Onshake()
    {
        DOVirtual.DelayedCall(.3f, () =>
        {
            stage2.transform.DOScale(Vector3.zero, .3f).SetEase(Ease.OutBack).OnComplete(() => stage2.SetActive(false));
          
        });
      
      drinkontable.GetComponent<Animator>().Play("tin");
     
      Vibration.Vibrate(100);
          if(AudioManager.instance)
              AudioManager.instance.Play("shake");
              AudioManager.instance.Play("Button");
              B_canopen = true;
          
          

    }

    public void Onspiders()
    {
        DOVirtual.DelayedCall(.3f, () =>
        {
            stage2.transform.DOScale(Vector3.zero, .3f).SetEase(Ease.OutBack).OnComplete(() => stage2.SetActive(false));
        });
        HandSequence();
        B_spidercan = true;
        Vibration.Vibrate(100);
        if(AudioManager.instance)
        AudioManager.instance.Play("Button");
        AudioManager.instance.Play("Swipe");
        DOVirtual.DelayedCall(6, () =>
        {

            AudioManager.instance.Play("Drink");
        });

    }
    //stage three sit on pins
    public void Onpins()
    {
        DOVirtual.DelayedCall(.3f, () =>
        {
            stage3.transform.DOScale(Vector3.zero, .3f).SetEase(Ease.OutBack).OnComplete(() => stage3.SetActive(false));
          
        });
        pins.SetActive(true);
        pins.transform.DOScale(Vector3.one, .3f).SetEase(Ease.OutBounce);
        drinkinhand.SetActive(false);
        
        Cam_anim.SetTrigger(Finalcam);


        DOVirtual.DelayedCall(1, () => playeranim.SetTrigger(Pins));
        DOVirtual.DelayedCall(2, () =>
            {
                if (AudioManager.instance)
                {
                    AudioManager.instance.Play("Scream"); 
                    AudioManager.instance.Play("Cry");
                }
            }
        );
        
        DOVirtual.DelayedCall(3, () =>
        {
            GameEvents.InvokeGameWin();
        });
        Vibration.Vibrate(100);
        if(AudioManager.instance)
        {
            AudioManager.instance.Play("pins");
            AudioManager.instance.Play("Button");
        }


    }
    public void Onfartballon()
    {
        DOVirtual.DelayedCall(.3f, () =>
        {
            stage3.transform.DOScale(Vector3.zero, .3f).SetEase(Ease.OutBack).OnComplete(() => stage3.SetActive(false));
          
        });
        // balloone.SetActive(true);
        // balloone.transform.DOScale(Vector3.one, .3f).SetEase(Ease.OutBounce);
        Snakesequence();
        drinkinhand.SetActive(false);
        
        Cam_anim.SetTrigger(Finalcam);


        DOVirtual.DelayedCall(1, () => playeranim.SetTrigger(Pins));
        DOVirtual.DelayedCall(2, () =>
                    {
                        if (AudioManager.instance)
                        {
                            AudioManager.instance.Play("Scream");
                            AudioManager.instance.Play("Cry");

                        }
                    }
                );
        DOVirtual.DelayedCall(3, () =>
        {
            GameEvents.InvokeGameWin();
        });
        Vibration.Vibrate(50);
        if(AudioManager.instance)
        {
            AudioManager.instance.Play("pins");
            AudioManager.instance.Play("Button");
        }

    }

    public void stage3cam()
    {
        Cam_anim.SetTrigger(Pincam);
        DOVirtual.DelayedCall(1, () => stage3.transform.DOScale(Vector3.one, .3f).SetEase(Ease.OutBounce));

    }
    private void ChipSequence()
    {
        var seq = DOTween.Sequence();
        chip.SetActive(true);
        //seq.AppendCallback(() => chip.SetActive(true));
        seq.AppendCallback(() =>
        {
            if(AudioManager.instance)
            {
                AudioManager.instance.Play("Swipe");
             
            }
        });
        seq.Append(chip.transform.DOMove(p1.position, .8f).SetEase(Ease.OutBack));
       
        seq.AppendInterval(.6f);
       
        seq.Append(chip.transform.DOMove(chipspacket.transform.position, .8f).SetEase(Ease.InBack));
        seq.AppendCallback(() =>
        {
            if(AudioManager.instance)
            {
                AudioManager.instance.Play("Swipe");
             
            }
        });
        seq.Append(chip.transform.DOScale(new Vector3(1.7f,1.7f,1.7f), .2f).SetEase(Ease.OutBack));
        seq.AppendCallback(() =>
        {
            chip.SetActive(false);
            poop.SetActive(false);
            DideatChilli = true;
        });
        
    } 
    private void PoopSequence()
    {
        var seq = DOTween.Sequence();
        poop.SetActive(true);
        //seq.AppendCallback(() => chip.SetActive(true));
        seq.AppendCallback(() =>
        {
            if(AudioManager.instance)
            {
                AudioManager.instance.Play("Swipe");
             
            }
        });
        seq.Append(poop.transform.DOMove(p1.position, .8f));
        seq.AppendInterval(.3f);
        seq.AppendCallback(() =>
        {
            if(AudioManager.instance)
            {
                AudioManager.instance.Play("Swipe");
             
            }
        });
        seq.Append(poop.transform.DOMove(chipspacket.transform.position, .8f));
       
        seq.AppendCallback(() =>
        {
            poop.SetActive(false);
            chip.SetActive(false);
            DidEatPoop = true;
        });
        
    }
    private void HandSequence()
    {
        var seq = DOTween.Sequence();
        hand.SetActive(true);
        //seq.AppendCallback(() => chip.SetActive(true));
        seq.Append(hand.transform.DOMove(movepoint.position, .4f));
        seq.AppendInterval(.3f);
       
        seq.AppendCallback(() =>
        {
            hand.GetComponent<Animator>().enabled = true;
            DOVirtual.DelayedCall(.2f, () =>
                {
                    bugs.gameObject.SetActive(true);
                    bugs.transform.DOMoveY(drinkontable.transform.position.y, 1f).OnComplete(() =>
                    {
                        hand.SetActive(false);
                        bugs.SetActive(false);
                    });
                    
                }
            );

        });
        
        
    }
    private void Snakesequence()
    {
        var seq = DOTween.Sequence();
        balloone.SetActive(true);
        //seq.AppendCallback(() => chip.SetActive(true));
        seq.Append(balloone.transform.DOMove(p2.position, .4f).SetEase(Ease.OutBack)); 
        //   seq.Append(chip.transform.DOScale(new Vector3(2.1f,2.1f,2.1f), .2f).SetEase(Ease.OutBack));
        seq.Append(balloone.transform.DOMove(p3.transform.position, .4f).SetEase(Ease.InBack));
       
        
    } 
}
