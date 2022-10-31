using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Banana_manager : MonoBehaviour
{
    [Header("Cam ")] public Animator Maincam;
    public static readonly int Finalcam = Animator.StringToHash("Finalcam");
    public static readonly int Dogcam = Animator.StringToHash("Dogcam");
    [Header("Pranker ")] public Animator Pranker;
    [Header("Player ")] public Animator Player;
    private static readonly int Walk = Animator.StringToHash("Move");
    private static readonly int Walkidle = Animator.StringToHash("Moveidle");
    private static readonly int Banana = Animator.StringToHash("Banana");
    private static readonly int Leaf = Animator.StringToHash("Leaf");
    public Transform Player_movepoint, bananamove;
    private Tween playermovemnt;
    public Transform movebackimpact;
    [Header("Ui")] public GameObject ui;
    public GameObject ui_stage2;
    [Space(20)] public SkinnedMeshRenderer banana;

    public GameObject leafrake;
    public ParticleSystem manhole;
    public ParticleSystem manhole1, player_fell;

    public Animator mainholeanim;

    public Color startColor = Color.green;
    public Color endColor = Color.black;
    public Material RedMat;

    [Header("Manhole ")] public Transform movepoint1,
        movepoint2,
        movepoint3;

    [Header("Dog ")] public List<Animator> Dog;

    public GameObject dog;
    public Transform Dogmovepoint1, Dogvepoint2;
    private static readonly int Fall = Animator.StringToHash("Fall");

    private void Start()
    {
        Vibration.Init();
    }

    private void OnEnable()
    {
        GameEvents.TapToPlay += OnTapToPlay;
    }

    private void OnDisable()
    {
        GameEvents.TapToPlay -= OnTapToPlay;
        RedMat.color = startColor;
    }

    private void OnTapToPlay()
    {
        Player.SetTrigger(Walk);
        Pranker.SetBool("Idle", false);
        playermovemnt = Player.transform.DOMove(Player_movepoint.transform.position, 30f).SetEase(Ease.Linear);
        if (AudioManager.instance)
        {
            AudioManager.instance.Play("walk");
        }

        Vibration.Vibrate(100);
    }

    public void OnBananaPress()
    {
        ui.transform.DOScale(Vector3.zero, .3f).SetEase(Ease.OutBounce);
        Pranker.SetTrigger("throw");
        DOVirtual.DelayedCall(.4f, () =>
        {
            BananaSequence();
            // Player.transform.DOMove(movebackimpact.position, .2f);
            DOTween.To(() => banana.GetBlendShapeWeight(0), x => banana.SetBlendShapeWeight(0, x), 100,
                5f);
        });
        Vibration.Vibrate(100);
        if (AudioManager.instance)
            AudioManager.instance.Play("Button");
    }


    public void Onleafrake()
    {
        ui.transform.DOScale(Vector3.zero, .3f).SetEase(Ease.OutBounce);
        leafrake.transform.DOScale(Vector3.one, .4f).SetEase(Ease.OutBounce).OnComplete(() =>
        {
            Player.SetTrigger(Leaf);
            DOVirtual.DelayedCall(.8f, () =>
            {
                leafrake.GetComponent<Animator>().enabled = true;
                if (AudioManager.instance)
                    AudioManager.instance.Play("Leaf");
            });
        });
        if (AudioManager.instance)
            AudioManager.instance.Play("Button");
        Vibration.Vibrate(100);
    }

    public void OnMainhole()
    {
        ui_stage2.SetActive(false);
        ui_stage2.transform.DOScale(Vector3.zero, .3f).SetEase(Ease.OutBounce);
        Pranker.SetTrigger("Dance");
        ManholeSequence();
        if (AudioManager.instance)
            AudioManager.instance.Play("Button");
        Vibration.Vibrate(100);
    }

    public void ondogs()
    {
        ui_stage2.SetActive(false);
        ui_stage2.transform.DOScale(Vector3.zero, .3f).SetEase(Ease.OutBounce);
        Player.SetTrigger("Idle");
        Maincam.SetTrigger(Dogcam);
        Pranker.SetTrigger("Dance");

        Dogsequence();
        if (AudioManager.instance)
            AudioManager.instance.Play("Button");
        Vibration.Vibrate(100);
    }

    public void ManholeSequence()
    {
        mainholeanim.enabled = true;
        var seq = DOTween.Sequence();
        seq.AppendCallback(() =>
        {
            mainholeanim.SetTrigger(Fall);
            manhole.Play();
            if (AudioManager.instance)
                AudioManager.instance.Play("Mainholeopen");
        });
        seq.Append(Player.transform.DOMove(movepoint1.position, .4f).SetEase(Ease.Linear));

        RedMat.color = startColor;
        seq.AppendCallback(() =>
        {
            manhole1.Play();
            if (AudioManager.instance)
                AudioManager.instance.Play("Manhole");
        });
        seq.Append(RedMat.DOColor(endColor, 1f));
        seq.AppendCallback(() =>
        {
            Player.SetTrigger("Jump");
            if (AudioManager.instance)
                AudioManager.instance.Play("Swipe");
        });
        seq.Append(Player.transform.DORotate(Vector3.zero, .2f).SetEase(Ease.Linear));
        seq.Append(Player.transform.DOMove(movepoint2.position, .4f).SetEase(Ease.Linear));
        seq.Append(Player.transform.DOMove(movepoint3.position, .4f).SetEase(Ease.Linear));
        // seq.Append(); 
        DOVirtual.DelayedCall(3, () => { GameEvents.InvokeGameWin(); });
        if (AudioManager.instance)
            AudioManager.instance.Play("Button");
    }

    public void BananaSequence()
    {
        var seq = DOTween.Sequence();
        seq.Append(banana.transform.DOScale(Vector3.one, .4f).SetEase(Ease.OutBounce));
        seq.Append(banana.transform.DOMove(bananamove.position, .3f));
        seq.AppendCallback(() =>
        {
            Player.SetTrigger(Banana);
            DOVirtual.DelayedCall(1f, () =>
            {
                if (AudioManager.instance)
                    AudioManager.instance.Play("Banana");
            });
        });
        if (AudioManager.instance)
            AudioManager.instance.Play("Swipe");
    }

    public void Dogsequence()
    {
        var seq = DOTween.Sequence();
        seq.Append(transform.DORotate(new Vector3(0, -165, 0), .3f));
        //seq.AppendInterval(.5f);
        seq.Append(transform.DORotate(new Vector3(0, 90, 0), .3f));
        seq.AppendCallback(() =>
        {
            for (int i = 0; i < Dog.Count; i++)
            {
                Dog[i].SetTrigger("Run");
            }
        });
        seq.AppendCallback(() =>
        {
            Player.applyRootMotion = true;
            Player.SetTrigger("Dog");
            seq.Append(dog.transform.DOMove(Dogvepoint2.position, 15f));
        });
        seq.Append(dog.transform.DOMove(Dogmovepoint1.position, 3f)).SetEase(Ease.InQuint);
        ;
        seq.Append(dog.transform.DORotate(new Vector3(0, 90, 0), .3f)).SetEase(Ease.Linear);
        if (AudioManager.instance)
            AudioManager.instance.Play("dog");
        DOVirtual.DelayedCall(3, () => { GameEvents.InvokeGameWin(); });
    }

    public void pausemovement()
    {
        playermovemnt.Kill();
        if (AudioManager.instance)
        {
            AudioManager.instance.Pause("walk");
        }
    }

    public void startmoveing()
    {
        playermovemnt = Player.transform.DOMove(Player_movepoint.transform.position, 30f).SetEase(Ease.Linear);
        Pranker.SetBool("Idle", false);
        if (AudioManager.instance)
        {
            AudioManager.instance.Play("walk");
        }
    }

    public void partical()
    {
        player_fell.Play();
        if (AudioManager.instance)
            AudioManager.instance.Play("star");
    }

    public void movebackinpact()
    {
        Player.transform.DOMove(movebackimpact.position, .2f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("stage2"))
        {
            DOVirtual.DelayedCall(.5f, () => ui_stage2.transform.DOScale(Vector3.one, .3f).SetEase(Ease.OutBounce));
            playermovemnt.Pause();
            Player.SetTrigger(Walkidle);
            Maincam.SetTrigger(Finalcam);
            Pranker.SetBool("Idle", true);
            if (AudioManager.instance)
            {
                AudioManager.instance.Pause("walk");
            }
        }

        if (other.gameObject.CompareTag("stage1"))
        {
            Pranker.SetBool("Idle", true);


            ui.transform.DOScale(Vector3.one, .3f).SetEase(Ease.OutBounce);
            Player.SetTrigger(Walkidle);
            playermovemnt.Pause();

            other.gameObject.SetActive(false);
            if (AudioManager.instance)
            {
                AudioManager.instance.Pause("walk");
            }
        }
    }

    public void leafrake_()
    {
        leafrake.transform.DOScale(Vector3.zero, .3f);
    }

    public void banana_()
    {
        banana.transform.DOScale(Vector3.zero, .3f);
    }
}