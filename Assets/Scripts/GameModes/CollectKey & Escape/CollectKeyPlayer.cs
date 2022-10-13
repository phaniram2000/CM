using UnityEngine;
using DG.Tweening;
using UnityEngine.Serialization;

public class CollectKeyPlayer : MonoBehaviour
{
    public float x, y;
    public DynamicJoystick joystick;
    [FormerlySerializedAs("Sensitivity")] public float sensitivity;
    public Animator PlayerAnim;
    public GameObject police;
    public Animator policeanim;
    public Transform point1, point2;
    public bool idle;
    public bool walk;
    public bool _start;
    public bool _policemove;
    public GameObject Key;
    public int totalcount;
    public int collectcount;
    public GameObject Door;
    public bool _keycollect;
    public GameObject helptext;
    
    private Sequence seqpolice;
    private Sequence policewalk;
    private Sequence helpseqobj;
    
    private void OnEnable()
    {
        GameEvents.TapToPlay += taptoplay;
    }

    private void OnDisable()
    {
        GameEvents.TapToPlay -= taptoplay;

    }

    public void taptoplay()
    {
        _start = true;
        policeseq();
        joystick.gameObject.SetActive(true);
        PlayerAnim.SetBool("Idle",true);
        policeanim.SetTrigger("Walk");
        policefootseq();
        helptext.SetActive(true);
    }
    void Start()
    {
        PlayerAnim = GetComponent<Animator>();
        policeanim = police.GetComponent<Animator>();
        point1.position = police.transform.position;
        
        
    }

    public void helpseq()
    {
        helpseqobj = DOTween.Sequence();
        helpseqobj.AppendInterval(4f);
        helpseqobj.AppendCallback(() =>
        {
            helptext.SetActive(false);
        });
    }

    private bool _once;
    private void Update()
    {
        if (Input.GetMouseButton(0) && _start)
        {
            if (!_once)
            {
                helpseq();
                _once = true;
            }
            
           
            if (x == 0 && y == 0)
            {
                
                if (!idle)
                {
                    PlayerAnim.SetBool("Idle",true);
                    PlayerAnim.SetBool("Walk",false);
                    idle = true;
                    walk = false;
                }
                
            }
            else
            {
                if (!walk)
                {
                    PlayerAnim.SetBool("Walk",true);
                    PlayerAnim.SetBool("Idle",false);
                    walk = true;
                    idle = false;
                }
                
            }
            
            x = joystick.Horizontal;
            y = joystick.Vertical;
            var newPos = new Vector3(x, 0, y);
            var transform1 = transform;
            transform1.position += new Vector3(newPos.x, 0, newPos.z) * (sensitivity * Time.deltaTime);
            var singleStep = 5f * Time.deltaTime;
            var lookDir = new Vector3(-joystick.Horizontal, 0, -joystick.Vertical);
            var newDirection = Vector3.RotateTowards(transform1.forward, -lookDir, singleStep, 0.0f);
            transform.rotation = Quaternion.LookRotation(newDirection);
            
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (!idle && _start)
            {
                idle = true;
                walk = false;
                PlayerAnim.SetBool("Idle",true);
                PlayerAnim.SetBool("Walk",false);
            }
           
        }
   
    }

    public void keyseq(GameObject pickup)
    {
        var seq = DOTween.Sequence();
        if (pickup.gameObject.name == "key")
        {
            _keycollect = true;
        }
        seq.Append(pickup.transform.DOMoveY((pickup.transform.position.y + 1.5f), 0.4f));
        seq.AppendCallback(() =>
        {
            pickup.transform.DOMove(transform.position, 0.5f);
            pickup.transform.DOScale(0.01f, 0.5f);
        });
    }

    public void policefootseq()
    {
        policewalk  = DOTween.Sequence();
        policewalk.AppendCallback(() =>
        {
            if (AudioManager.instance)
            {
                AudioManager.instance.Play("Police Walk");
            }
        });
        policewalk.AppendInterval((1.4f));
        policewalk.SetLoops(-1);
    }
    public void Doorseq()
    {
        var seq = DOTween.Sequence();
        seq.Append(Door.transform.DOLocalRotate(new Vector3(0,0,80f),0.5f));
        seq.AppendCallback(() =>
        {
            if (AudioManager.instance)
            {
                AudioManager.instance.Play(("Door"));
            }
        });
        
    }

    public void winningseque()
    {
        var seq = DOTween.Sequence();
        joystick.gameObject.SetActive(false);
        seq.Append(transform.DOLookAt(new Vector3(0,0,0),0.2f).SetEase((Ease.Linear)));
        seq.AppendCallback(() =>
        {
            PlayerAnim.SetBool("Idle", false);
            PlayerAnim.SetBool("Walk", false);
            PlayerAnim.SetTrigger("Run");
            transform.DOMoveZ(transform.position.z + 10f, 1f).SetEase(Ease.Linear);

        });
        seq.AppendInterval(0.2f);
        seq.AppendCallback(() =>
        {
            GameEvents.InvokeGameWin();
            policewalk.Kill();
            seqpolice.Kill();
            
        });
        
    }
    public void policeseq()
    {
        seqpolice = DOTween.Sequence();
        seqpolice.AppendCallback(() =>
        {
            policewalk.Play();
            policeanim.SetTrigger("Walk");
        });
        seqpolice.Append(police.transform.DOMove(point2.position, 8f).SetEase(Ease.Linear));
        seqpolice.AppendCallback(() =>
        {
            policewalk.Pause();
            policeanim.SetTrigger("Idle");
        });
        seqpolice.AppendInterval(1f);
        seqpolice.Append(police.transform.DORotate(new Vector3(0,-90f,0f), .4f)).SetEase(Ease.Linear);
        seqpolice.AppendCallback(() =>
        {
            policewalk.Play();
            policeanim.SetTrigger("Walk");
        });
        seqpolice.Append(police.transform.DOMove(point1.position, 8f).SetEase(Ease.Linear));
        seqpolice.AppendCallback(() =>
        {
            policewalk.Pause();
            policeanim.SetTrigger("Idle");
        });
        seqpolice.AppendInterval(1f);
        seqpolice.Append(police.transform.DOLookAt(point2.position, .4f)).SetEase(Ease.Linear);
        seqpolice.SetLoops(-1);
        
        
    }

    public void Policeshoot()
    {
        var seq = DOTween.Sequence();
        seq.AppendCallback(() =>
        {
            if (AudioManager.instance)
            {
                AudioManager.instance.Play("Police shout");
                AudioManager.instance.Play("Player shout");
            }
            if (AudioManager.instance)
            {
                AudioManager.instance.Play("Player shout");
            }
        });
        seq.Append(police.transform.DOLookAt(transform.position, 0.1f));
        seq.Append(transform.DOLookAt(police.transform.position, 0.1f));
        seq.AppendCallback(() =>
        {
            policeanim.SetTrigger("Shoot");
            if (AudioManager.instance)
            {
                AudioManager.instance.Play("Gun Shoot");
            }
            
        });
        seq.AppendCallback(() =>
        {
            PlayerAnim.SetBool("Walk", false);
            PlayerAnim.SetBool("Idle", false);
        });
        seq.AppendInterval(0.5f);
        seq.AppendCallback(() =>
        {
            PlayerAnim.SetTrigger("Fall");
        });
        seq.AppendInterval(1f);
        seq.AppendCallback(() =>
        {
            GameEvents.InvokeGameLose(-1);
        });
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Cone"))
        {
            Vibration.Vibrate(30);
            seqpolice.Kill();
            print("fail");
            _start = false;
            Policeshoot();
            joystick.gameObject.SetActive(false);
            point1 = police.transform;
            point2 = police.transform;
            gameObject.GetComponent<Collider>().enabled = false;

        }
        if (other.gameObject.name == "Winning")
        {
            Vibration.Vibrate(30);
            if (AudioManager.instance)
            {
                AudioManager.instance.Play("Key");
            }
            winningseque();
            print("key");
        }

        if (other.gameObject.CompareTag("pickups"))
        {
            other.gameObject.GetComponent<Collider>().enabled = false;
            collectcount++;
            keyseq(other.gameObject);
            if (other.gameObject.name == "key")
            {
                Vibration.Vibrate(30);
                if (AudioManager.instance)
                {
                    AudioManager.instance.Play("Key");
                }
                _keycollect = true;
            }
            
            if (other.gameObject.name == "Money")
            {
                Vibration.Vibrate(30);
                if (AudioManager.instance)
                {
                    AudioManager.instance.Play("Money");
                }
            }
            if (other.gameObject.name == "Gold")
            {
                Vibration.Vibrate(30);
                if (AudioManager.instance)
                {
                    AudioManager.instance.Play("Iteam");
                }
            }

            if (other.gameObject.name == "Tab")
            {
                Vibration.Vibrate(30);
                if (AudioManager.instance)
                {
                    AudioManager.instance.Play("Iteam");
                }
            }
           
            
        }

        if (other.gameObject.CompareTag("Door"))
        {
            if (_keycollect)
            {
                Vibration.Vibrate(30);
                other.gameObject.SetActive(false);
                Doorseq();
            }
        }
    }
}
