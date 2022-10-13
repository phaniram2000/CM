using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Dreamteck.Splines.Primitives;
using UnityEditor;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class PriceTagManager : MonoBehaviour
{
    [Header(("UI"))] 
    public GameObject Ui_stage;
    [SerializeField] private GameObject Erase;

    [Header("Movepoints")] 
    public Transform movepoint1;
    public Transform movepoint2;
    public Transform movepoint3;
    public Transform movepoint4;
    public Transform movepoint5;

    [Header("Player")] 
    public Transform Pranker;
    public Rig handweight, lefthand, head, dresstakerig;
    private Animator pranker_anim;
    public Animator billboy;
    public Rig billboyrig;
    public Transform baginhand, righthand;
    public Transform billcounterposition;
    [Header("Camera")] public Animator Cam;

    [Header("Erase and draw Game objects")] [SerializeField]
    public GameObject erase;

    public GameObject draw;


    public Transform shoppingbag;
    public Transform shoppingbagtrandform;


    public GameObject police;
    public Transform policemovepoint;

    [Header("messagebox")]
    public GameObject beautifuldress, itsgettinglate, givemeasec,police_catchher;
    public void OnEnable()
    {
        GameEvents.TapToPlay += OnTapToPlay;
        EraseTarget.Ondoneerasing += OnEraseEnd;
        GameEvents.GameWin += OnGamewin;
        GameEvents.GameLose += OnGamelost;
        PricetagEvents.DoneWithInput += DonwithDrawing;
    }
    private void OnDisable()
    {
        GameEvents.TapToPlay -= OnTapToPlay;
        EraseTarget.Ondoneerasing -= OnEraseEnd;
        GameEvents.GameWin -= OnGamewin;
        GameEvents.GameLose -= OnGamelost;
        PricetagEvents.DoneWithInput -= DonwithDrawing;
    }
    void Start()
    {
        pranker_anim = Pranker.GetComponent<Animator>();
    }

    private void OnTapToPlay()
    {
      //  Ui_stage.transform.DOScale(Vector3.one, .3f).SetEase(Ease.OutBounce);
        MovetoshopSequence();
        if(AudioManager.instance)
            AudioManager.instance.Play("Welcome");
    }

    private void DonwithDrawing()
    {
        AfterdrawSequence();
    }

    private void OnGamelost(int obj)
    {
    }

    private void OnGamewin()
    {
        Debug.Log("Win");
    }

    public void AfterdrawSequence()
    {
        var seq = DOTween.Sequence();
        var result = GameCanvas.game.CheckGameResult(GameCanvas.game.GetLastResult());

        seq.AppendCallback(() => { Cam.SetTrigger("Draw"); });
        seq.Append(DOVirtual.DelayedCall(1, () => Cam.SetTrigger("Finalcam")));
        seq.Append(Pranker.transform.DORotate(new Vector3(0, -27.96f, 00), .5f));
        seq.AppendInterval(1.2f);
        seq.AppendCallback(() =>
        {
            pranker_anim.SetTrigger("Walk"); 
            if(AudioManager.instance)
                AudioManager.instance.Play("Walk");
        });
        seq.Append(Pranker.transform.DOMove(billcounterposition.position, 2f).SetEase(Ease.Linear));
        seq.AppendCallback(() =>
        {
            pranker_anim.SetTrigger("Blowair"); 
            if(AudioManager.instance)
                AudioManager.instance.Pause("Walk");
        });
        seq.AppendCallback(() => { billboy.SetTrigger("Typing"); });
        seq.AppendInterval(2f);
        switch (result)
        {
            case 0: // win
                seq.AppendCallback(winsequence);
                return;
            case -1:
                seq.AppendCallback(lostsequence);
                return;
        }
    }

    public void lostsequence()
    {
        police.SetActive(true);
        var seq = DOTween.Sequence();

        seq.AppendCallback(() =>
        {
            billboy.transform.DOLookAt(Pranker.transform.position, .5f);
            billboy.SetTrigger("angry");
            police_catchher.transform.DOScale(Vector3.one, .3f).SetEase(Ease.OutBounce);
            if(AudioManager.instance)
                AudioManager.instance.Play("Police");
            //   Cam.SetTrigger("Dancecam");
        });
        seq.AppendInterval(2f);
        seq.Append(police.transform.DOMove(policemovepoint.position, .4f));
        seq.AppendCallback(() => Cam.SetTrigger("Dancecam"));
        seq.AppendInterval(1f);
        seq.Append(DOTween.To(() => handweight.weight, x => handweight.weight = x, 1, .3f));
        seq.Append(DOTween.To(() => lefthand.weight, x => lefthand.weight = x, 1, .3f));
        seq.Append(DOTween.To(() => head.weight, x => head.weight = x, 1, .3f));
        seq.Append(Pranker.transform.DORotate(new Vector3(0, 185.508f, 0), 1f));
        seq.Append(police.transform.DORotate(new Vector3(0, 185.508f, 0), 1f));

        seq.AppendCallback(() =>
        {
            pranker_anim.SetTrigger("Walk");
            police.GetComponent<Animator>().SetTrigger("Walk");
            Pranker.transform.DOMove(movepoint4.position, 6f).SetEase(Ease.Linear);
            police.transform.DOMove(movepoint4.position, 10f).SetEase(Ease.Linear);
            if(AudioManager.instance)
                AudioManager.instance.Pause("Walk");
        });
        seq.AppendCallback(() => { GameCanvas.game.MakeGameResult(); });
    }

    public void winsequence()
    {
        var seq = DOTween.Sequence();
        seq.AppendInterval(1);
        seq.AppendCallback(() =>
        {
            itsgettinglate.transform.DOScale(Vector3.one, .3f).SetEase(Ease.OutBounce);
            if(AudioManager.instance)
                AudioManager.instance.Play("Late");
        });
        seq.AppendInterval(2);
        seq.AppendCallback(() =>
        {
            itsgettinglate.transform.DOScale(Vector3.zero, .3f).SetEase(Ease.OutBounce); 
            givemeasec.transform.DOScale(Vector3.one, .3f).SetEase(Ease.OutBounce);
            if(AudioManager.instance)
                AudioManager.instance.Play("Min");
        });
        seq.AppendInterval(1);
        seq.AppendCallback(() =>
        {
            billboy.SetTrigger("idle");
        });
        seq.Append(shoppingbag.transform.DOScale(Vector3.one * 5.667058f, .4f).SetEase(Ease.OutBounce));
        seq.Append(billboy.transform.DORotate(
            new Vector3(billboy.transform.rotation.x, 174.945f, billboy.transform.rotation.z), .3f));
        seq.Append(billboy.transform.DOMove(movepoint5.position, 1f).SetEase(Ease.Linear));
        seq.AppendCallback(() =>
        {
            DOTween.To(() => billboyrig.weight, x => billboyrig.weight = x, 1, .3f);
            shoppingbag.parent = null;
        });
        seq.Append(shoppingbag.transform.DORotate(
            new Vector3(shoppingbagtrandform.rotation.x, shoppingbagtrandform.rotation.y,
                shoppingbagtrandform.rotation.z), .3f));
        seq.Append(shoppingbag.transform.DOMove(shoppingbagtrandform.position, .4f).SetEase(Ease.OutBounce));
        seq.Append(DOTween.To(() => billboyrig.weight, x => billboyrig.weight = x, 0, .3f));
        seq.Append(DOTween.To(() => dresstakerig.weight, x => dresstakerig.weight = x, 1, .3f));
        seq.AppendCallback(() =>
        {
            shoppingbag.SetParent(righthand);
            shoppingbag.transform.DOLocalRotate(
                new Vector3(-105, 0, 0), .3f);
            shoppingbag.transform.DOLocalMove(baginhand.localPosition, .4f).SetEase(Ease.OutBounce); 
            shoppingbag.transform.DOScale(Vector3.one* 3,.4f).SetEase(Ease.OutBounce);
        });
       
        seq.Append(DOTween.To(() => dresstakerig.weight, x => dresstakerig.weight = x, 0, .3f));
       
        seq.Append(Pranker.transform.DORotate(new Vector3(0, 185.508f, 0), 1f));
        seq.AppendCallback(() =>
        {
            pranker_anim.SetTrigger("Walk");
            Cam.SetTrigger("Dancecam");
            if(AudioManager.instance)
                AudioManager.instance.Play("Walk");
        });
        seq.Append(Pranker.transform.DOMove(movepoint4.position, 6f).SetEase(Ease.Linear));
        seq.AppendCallback(() =>
        {
            pranker_anim.SetTrigger("Dance");
            GameCanvas.game.MakeGameResult();
            if(AudioManager.instance)
                AudioManager.instance.Pause("Walk");
        });
    }

    public void MovetoshopSequence()
    {
        var seq = DOTween.Sequence();
        seq.AppendInterval(2);
        seq.AppendCallback(() =>
        {
            pranker_anim.SetTrigger("Walk");
            Ui_stage.transform.DOScale(Vector3.zero, .3f).SetEase(Ease.OutBounce);
            if(AudioManager.instance)
                AudioManager.instance.Play("Walk");
        });
        seq.Append(Pranker.transform.DOMove(movepoint1.position, 2f).SetEase(Ease.Linear));
        seq.Append(Pranker.transform.DOMove(movepoint2.position, 2f).SetEase(Ease.Linear));
        seq.AppendCallback(() =>
        {
            Cam.SetTrigger("Inside");
            pranker_anim.SetTrigger("Look");
            if(AudioManager.instance)
                AudioManager.instance.Pause("Walk");
        });
        seq.AppendInterval(3);
        seq.AppendCallback(() =>
        {
            pranker_anim.SetTrigger("Walk");
            beautifuldress.transform.DOScale(Vector3.one, .3f).SetEase(Ease.OutBounce);
            if(AudioManager.instance)
                AudioManager.instance.Play("Dress");
                AudioManager.instance.Play("Walk");
        });

        seq.Append(Pranker.DOLookAt(movepoint3.position, .5f));
        seq.Append(Pranker.transform.DOMove(movepoint3.position, 2f).SetEase(Ease.Linear));
        seq.AppendCallback(() =>
        {
            pranker_anim.SetTrigger("Idle");
            Cam.SetTrigger("Tagview");
            beautifuldress.transform.DOScale(Vector3.zero, .3f).SetEase(Ease.OutBounce);
            if(AudioManager.instance)
                AudioManager.instance.Pause("Walk");
        });
        seq.AppendInterval(2);
        seq.AppendCallback(() => { Cam.SetTrigger("Tag"); });
        seq.AppendCallback(() =>
        {
            Erase.transform.DOScale(Vector3.one, .3f).SetEase(Ease.OutBounce);
            if(AudioManager.instance)
                AudioManager.instance.Play("pop");
        });
    }

    public void onerase()
    {
        Erase.transform.DOScale(Vector3.zero, .5f).SetEase(Ease.InBounce);
        erase.transform.tag = "EraseTarget";
        if(AudioManager.instance)
            AudioManager.instance.Play("Button");
        Vibration.Vibrate(30
        );
    }

    public void OnEraseEnd()
    {
        erase.SetActive(false);
        draw.SetActive(true);
        draw.transform.tag = "DrawArea";
    }


    // Update is called once per frame
    void Update()
    {
    }
}