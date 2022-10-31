using DG.Tweening;
using UnityEngine;

public class golddigger_Manager : MonoBehaviour
{
    public Animator Girl,
        boy,
        Cam,
        car,
        girlfriend;

    public float distance;
//    public Rig playerhead;
    public GameObject shovel, car_;
    private Rigidbody rb;

    [Header("Movepoints")] public Transform movetoboy,
        movetoboy2,
        movetoboy3,
        movetoboy4,
        movetoboy5,
        movetoboy6,
        movetoboy7,
        movetoboy8;

    [Header("UI")] public GameObject uistage1, uistage2, uistage3;

    [Header("Conv text")]
    public Transform Gotohell, Isthisurcar, Whatever, Love, loveu, golddigger, whatthehell, looser;

    public void OnEnable()
    {
        GameEvents.TapToPlay += OnTapToPlay;
    }
    private void OnDisable()
    {
        GameEvents.TapToPlay -= OnTapToPlay;
    }
    private void OnTapToPlay()
    {
        GirlSeqence();
    }

    void Start()
    {
        rb = shovel.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        distance = Vector3.Distance(Girl.transform.position, boy.transform.position);
        if (distance < 1.9f)
        {
          //  playerhead.weight += 2 * Time.deltaTime;
        }
    }

    public void GirlSeqence()
    {
        var seq = DOTween.Sequence();
        
        seq.AppendCallback(() =>
        {
            Girl.SetTrigger("Walk");
            if(AudioManager.instance)
                AudioManager.instance.Play("Walk");
        });
        seq.Append(Girl.transform.DOMove(movetoboy.transform.position, 3).SetEase(Ease.Linear)); 
        //seq.Append(DOTween.To(() => playerhead.weight, x => playerhead.weight = x, 1, 1f));  
        seq.Append(Girl.transform.DOLookAt(boy.transform.position, .5f));
        seq.Append(Girl.transform.DOMove(movetoboy2.transform.position, 2).SetEase(Ease.Linear));
        seq.AppendCallback(() =>
        {
            boy.SetTrigger("Idle"); 
            if(AudioManager.instance)
                AudioManager.instance.Pause("Walk");
        });
       // seq.Append(DOTween.To(() => playerhead.weight, x => playerhead.weight = x, 0, .2f));
        seq.Append(boy.transform.DORotate(new Vector3(0, -157.12f, 0), .4f).SetEase(Ease.OutBack));
        seq.AppendCallback(() => { Cam.SetTrigger("Boy"); });
        seq.AppendCallback(() => { Girl.SetTrigger("Attitude"); });
        seq.AppendCallback(() => { girlfriend.SetTrigger("Idle"); });
        seq.AppendCallback(() => { uistage1.transform.DOScale(Vector3.one, .4f).SetEase(Ease.OutBounce); });
    }

    public void Onbuttinpresslove()
    {
        Lovesequence();
        if(AudioManager.instance)
            AudioManager.instance.Play("Button");
        Vibration.Vibrate(30);
    }

    public void Lovesequence()
    {
        uistage1.transform.DOScale(Vector3.zero, .4f).SetEase(Ease.OutBounce);
        var seq = DOTween.Sequence();
        seq.AppendCallback(()=>
        {
            loveu.DOScale(Vector3.one, .4f).SetEase(Ease.OutBack);
            if(AudioManager.instance)
                AudioManager.instance.Play("Iloveyou");
        });
        seq.AppendCallback(() => Girl.SetTrigger("Talk"));
        seq.AppendInterval(1f);
        seq.AppendCallback(() =>
        {
            boy.SetTrigger("Angry");
            girlfriend.SetTrigger("Angry");
        });
        seq.Append(loveu.DOScale(Vector3.zero, .4f).SetEase(Ease.OutBack));
   //     seq.Append(whatthehell.DOScale(Vector3.one, .4f).SetEase(Ease.OutBack));
        seq.AppendCallback(() =>
        {
            Gotohell.DOScale(Vector3.one, .4f).SetEase(Ease.OutBack);
            if(AudioManager.instance)
                AudioManager.instance.Play("haveagf");
        });
        
        seq.AppendInterval(1.5f);
       // seq.Append(whatthehell.DOScale(Vector3.zero, .4f).SetEase(Ease.OutBack));
        seq.Append(Gotohell.DOScale(Vector3.zero, .4f).SetEase(Ease.OutBack));
        seq.AppendInterval(.8f);
        seq.AppendCallback(() =>
        {
            Girl.SetTrigger("Walk");
            if(AudioManager.instance)
                AudioManager.instance.Play("Walk");
        });
        seq.AppendCallback(() => Cam.SetTrigger("Car"));
        seq.Append(Girl.transform.DORotate(new Vector3(0, 180, 0), .4f).SetEase(Ease.OutBack));
        seq.AppendCallback(()=>
        {
            Whatever.DOScale(Vector3.one, .4f).SetEase(Ease.OutBack);
            if(AudioManager.instance)
                AudioManager.instance.Play("Whatever");
        });
        seq.Append(Girl.transform.DOMove(movetoboy3.transform.position, 2).SetEase(Ease.Linear));
        seq.AppendCallback(() =>
        {
            Girl.applyRootMotion = true;
            Girl.SetTrigger("Car");
            if(AudioManager.instance)
                AudioManager.instance.Pause("Walk");
        });
        seq.Append(Whatever.DOScale(Vector3.zero, .4f).SetEase(Ease.OutBack));
        seq.AppendInterval(2f);
        seq.Append(boy.transform.DORotate(new Vector3(0, 180, 0), .2f));
        seq.AppendCallback(() =>
        {
            boy.SetTrigger("Walk");
            if(AudioManager.instance)
                AudioManager.instance.Play("Walk");
        });
        seq.Append(boy.transform.DOMove(movetoboy4.position, 2f).SetEase(Ease.Linear));
        seq.AppendCallback(() =>
        {
            boy.SetTrigger("Idle"); 
            if(AudioManager.instance)
                AudioManager.instance.Pause("Walk");
        });
        seq.AppendCallback(() => { boy.SetTrigger("Tocar"); });
        seq.AppendInterval(1);
        seq.AppendCallback(()=>
        {
            
            Isthisurcar.DOScale(Vector3.one, .4f).SetEase(Ease.OutBack);
            if(AudioManager.instance)
                AudioManager.instance.Play("urcar");
        });
        seq.Append(uistage2.transform.DOScale(Vector3.one, .4f).SetEase(Ease.OutBounce));
    }

    public void onyes()
    {
        uistage2.transform.DOScale(Vector3.zero, .4f).SetEase(Ease.OutBounce);
        Girl.transform.DOScale(Vector3.zero, .2f);
        yesSequence();
        if(AudioManager.instance)
            AudioManager.instance.Play("Button");
           
        Vibration.Vibrate(30);
    }

    public void yesSequence()
    {
        var seq = DOTween.Sequence();
        seq.Append(Isthisurcar.DOScale(Vector3.zero, .4f).SetEase(Ease.OutBack));
        seq.AppendCallback(()=>
        {
            
            Love.DOScale(Vector3.one, .3f).SetEase(Ease.OutBack);
            if(AudioManager.instance)
                AudioManager.instance.Play("iloveutoo");
        });
        seq.Append(uistage3.transform.DOScale(Vector3.one, .4f).SetEase(Ease.OutBounce));
        //seq.Append(golddigger.DOScale(Vector3.one, .4f).SetEase(Ease.OutBack));
      
    }
    public void OnNo()
    {
        uistage2.transform.DOScale(Vector3.zero, .4f).SetEase(Ease.OutBounce);
        NoSequence();
        if(AudioManager.instance)
            AudioManager.instance.Play("Button");
        Vibration.Vibrate(30);
        
    }
    public void onnotgolddigger()
    {
        uistage3.transform.DOScale(Vector3.zero, .4f).SetEase(Ease.OutBounce);
        NoSequence();
        if(AudioManager.instance)
            AudioManager.instance.Play("Button");
        Vibration.Vibrate(30);
    }
    public void NoSequence()
    {
        var seq = DOTween.Sequence();
        seq.Append(Love.DOScale(Vector3.zero, .3f).SetEase(Ease.OutBack));
        seq.Append(Isthisurcar.DOScale(Vector3.zero, .4f).SetEase(Ease.OutBack));
        seq.AppendCallback(() =>
        {
            boy.SetTrigger("Walk");
            boy.transform.DOLookAt(girlfriend.transform.position, .3f).SetEase(Ease.Linear);
            Cam.SetTrigger("NO");
        });
        seq.Append(boy.transform.DOMove(movetoboy8.position, 4f).SetEase(Ease.Linear));  
        seq.Append(boy.transform.DORotate(new Vector3(0,-93.048f,0), .4f));
        seq.AppendCallback(() => { boy.SetTrigger("Idle"); });
        seq.AppendCallback(() => { girlfriend.SetTrigger("Slap"); });
        seq.Append(looser.DOScale(Vector3.one, .3f).SetEase(Ease.OutBack));
        seq.AppendCallback(() => { GameEvents.InvokeGameLose(-1); });
        // seq.Append(golddigger.DOScale(Vector3.one, .4f).SetEase(Ease.OutBack));
    }

    public void ongolddigger()
    {
        shovelsequence();
        if(AudioManager.instance)
            AudioManager.instance.Play("Button");
        AudioManager.instance.Play("Thug");
        Vibration.Vibrate(30);
    }

    public void shovelsequence()
    {
        var seq = DOTween.Sequence();
        seq.Append(uistage3.transform.DOScale(Vector3.zero, .4f).SetEase(Ease.OutBounce));
        //seq.Append(shovel.transform.DOScale(Vector3.one, .3f).SetEase(Ease.OutBounce));
        seq.Append(Love.DOScale(Vector3.zero, .3f).SetEase(Ease.OutBack));
        seq.AppendCallback(() =>
        {
            shovel.transform.DOScale(Vector3.one, .3f).SetEase(Ease.OutBounce);
            rb.isKinematic = false;
            shovel.transform.DOMove(movetoboy7.position, .2f).OnComplete(() => { boy.SetTrigger("Fall"); });
        });
        seq.AppendInterval(3);
        seq.AppendCallback(() =>
        {
            Cam.SetTrigger("final");
            car.transform.DOMove(movetoboy6.position, 4).SetEase(Ease.Linear);
            boy.transform.DOLookAt(movetoboy5.position, .3f).SetEase(Ease.Linear);
            boy.transform.DOMove(movetoboy5.position, 6).SetEase(Ease.Linear);
            golddigger.DOScale(Vector3.one, .3f).SetEase(Ease.OutBack);
            looser.DOScale(Vector3.one, .3f).SetEase(Ease.OutBack);
        });
        seq.AppendCallback(() => { GameEvents.InvokeGameWin(); });
        if(AudioManager.instance)
            AudioManager.instance.Play("Golddigger");
        
    }
}