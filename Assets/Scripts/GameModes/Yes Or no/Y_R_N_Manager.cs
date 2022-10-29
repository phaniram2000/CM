using DG.Tweening;
using UnityEngine;
using UnityEngine.Animations.Rigging;
public class Y_R_N_Manager : MonoBehaviour
{
    [Header("Left Hand")] 
    public GameObject Lefthand;
    [SerializeField] private GameObject Lefthand_child;
    [Header("Right Hand")]
    public GameObject Righthand;
    [SerializeField]
    private GameObject Righthand_child;
    [Header("Fan man ")] 
    public Animator Fatman_anim;
    public Rig Rightrig;
    public Rig Leftrig;
    private static readonly int jarlift = Animator.StringToHash("jarlift");
    [Header("Pranker ")] public Animator Pranker;
    private static readonly int Up = Animator.StringToHash("Up");
    private static readonly int Done = Animator.StringToHash("Done");
    private static readonly int Down = Animator.StringToHash("Down");
    private static readonly int idle = Animator.StringToHash("Idle");
    [Header("UI")]
    public GameObject ui;
    [Space(20)]
    public Transform G_food;
    public Transform Tomove;
    public bool next;
    public int count = 1;
    public GameObject unselectedFood;
    [Header("Jar")]
    public GameObject jar;
    public Transform glass;
    public bool glass_B;
    public Transform glassmove;
    public Animator jar_anim;
    public SkinnedMeshRenderer jar_blend;
    public SkinnedMeshRenderer Glass_blend;
    public GameObject jarinhand;
    public GameObject jarontable;
    [Header("Bool")] public bool Ritechoice1, Ritechoice2, Ritechoice3;
    public ParticleSystem vomit;
    private void OnEnable()
    {
        GameEvents.TapToPlay += OnTapToPlay;
        Y_R_N_Pranker.Onbuttons += Buttons;
        Y_R_N_Pranker.Onfood += food; 
        Y_R_N_Pranker.Onhands += onhandsdown;
        Y_R_N_Player.Onjar += Jarinhand; 
        Y_R_N_Player.Onvomit += _vomit;
    }
    private void OnDisable()
    {
        GameEvents.TapToPlay -= OnTapToPlay;
        Y_R_N_Pranker.Onbuttons -= Buttons;
        Y_R_N_Pranker.Onfood -= food;
        Y_R_N_Pranker.Onhands -= onhandsdown;
        Y_R_N_Player.Onjar -= Jarinhand; 
        Y_R_N_Player.Onvomit -= _vomit;
    }

    private void Start()
    {
        Vibration.Init();
    }

    void Update()
    {
        if (count >= 3&& !glass_B)
        {
            glass.DOMove(glassmove.position, .4f).SetEase(Ease.OutBack);
            BlendSequence();
           
            glass_B = true;
        }
    }
    public void Onleftbutonpress()
    {
        Lefthand_child.transform.SetParent(jar.transform);
        DOTween.To(() => Leftrig.weight, x => Leftrig.weight = x, 1, .4f)
            .OnComplete(()=>
        {
            FoodmoveSequence(Lefthand_child);
            DOTween.To(() => Leftrig.weight, x => Leftrig.weight = x, 0, 1f);
        });
        next = true;
        ui.transform.DOScale(Vector3.zero, .3f).SetEase(Ease.OutBounce);
        Pranker.SetTrigger(Done);
        count += 1;
        unselectedFood = Righthand_child.gameObject;
        if (count < 3)
        {
            Pranker.SetTrigger(Down);
        }
        else
        {
            Pranker.SetTrigger(idle);
        }

        if (count == 1)
        {
            Ritechoice1 = true;
        }
        if (count == 3)
        {
            Ritechoice3 = true;
        }
        AudioManager.instance.Play("Button");

       Vibration.Vibrate(100);
    }
    public void onrightbuttonpress()
    {
        Righthand_child.transform.SetParent(jar.transform);
        DOTween.To(() => Rightrig.weight, x => Rightrig.weight = x, 1, .4f).OnComplete(() =>
        {
            FoodmoveSequence(Righthand_child);
            DOTween.To(() => Rightrig.weight, x => Rightrig.weight = x, 0, 1f);
        });
        next = true;
        Pranker.SetTrigger(Done);
        ui.transform.DOScale(Vector3.zero, .3f).SetEase(Ease.OutBounce);
        count += 1;
        unselectedFood = Lefthand_child.gameObject;
        if (count < 3)
        {
            Pranker.SetTrigger(Down);
        }
        else
        {
            Pranker.SetTrigger(idle);
        }
        if (count == 2)
        {
            Ritechoice2 = true;
        }
        AudioManager.instance.Play("Button");
        Vibration.Vibrate(100);
    }
    public void FoodmoveSequence(GameObject hand)
    {
        hand.transform.parent = null;
        hand.transform.SetParent(jar.transform);
        var seq = DOTween.Sequence();
        AudioManager.instance.Play("Handup");
        seq.Append(hand.transform.DOMove(Tomove.transform.position, .3f).SetEase(Ease.OutBack));
        seq.AppendCallback(() =>
        {
            hand.transform.DOScale(hand.transform.localScale / 2, .5f);
            hand.GetComponent<Rigidbody>().useGravity = true;
         DOVirtual.DelayedCall(.5f, () => { hand.GetComponent<Rigidbody>().isKinematic = true;        AudioManager.instance.Play("pop");
         });
        });
    }
    public void BlendSequence()
    {
        var seq = DOTween.Sequence();
        seq.AppendInterval(2);
        seq.AppendCallback(() =>
        {
            jar_blend.gameObject.SetActive(true);
            G_food.DOScale(G_food.localScale /1.5f, 1f);
            G_food.DORotate(Vector3.right, .2f).SetLoops(-1,LoopType.Incremental).SetEase(Ease.Linear);
        });
        seq.Append(DOTween.To(() => jar_blend.GetBlendShapeWeight(0), x => jar_blend.SetBlendShapeWeight(0, x), 100,
            5f));
        
        seq.Append(G_food.DOScale(Vector3.zero, 1f));
        
        seq.AppendCallback(() =>
        {
            Fatman_anim.SetLayerWeight(1,1);
            Fatman_anim.SetTrigger(jarlift);
        });
    }
    private void OnTapToPlay()
    {
        Pranker.SetTrigger(Up);
        if (Righthand_child == null && count <= 3&& glass_B == false)
        {
            Righthand_child = Righthand.transform.GetChild(0).gameObject;
        }
         if (Lefthand_child == null && count <= 3&& glass_B == false)
        {
            Lefthand_child = Lefthand.transform.GetChild(0).gameObject;
        }
    }
    private void Buttons()
    {
        ui.transform.GetChild(count).gameObject.SetActive(true);
        ui.transform.DOScale(Vector3.one, .3f).SetEase(Ease.OutBounce);
    }
    private void food()
    {
        unselectedFood = null;
        Lefthand_child.SetActive(true);
        Righthand_child.SetActive(true);
    }
    private void onhandsdown()
    {
        unselectedFood.SetActive(false);
        unselectedFood.transform.parent = null;
        Lefthand_child = null;
        Righthand_child = null;
        if (Righthand_child == null && count <= 3&& glass_B == false)
        {
            Righthand_child = Righthand.transform.GetChild(0).gameObject;
        }
         if (Lefthand_child == null && count <= 3&& glass_B == false)
        {
            Lefthand_child = Lefthand.transform.GetChild(0).gameObject;
        }
    }
    public void Jarinhand()
    {
        jarinhand.SetActive(true);
        jarontable.SetActive(false);
    }
    public void _vomit()
    {
        Pranker.SetBool("mouth",true);
        vomit.Play();
        DOVirtual.DelayedCall(1, () =>
        {
            GameEvents.InvokeGameWin();
        });
        
        if(AudioManager.instance)
            AudioManager.instance.Play("vomit");
    }
}