using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using Button = UnityEngine.UI.Button;

public class BailManager : MonoBehaviour
{
    public GameObject Player;
    public GameObject PoliceMan;
    public GameObject Chair;
    public GameObject Laptop;
    public GameObject Virtualcam;
    public GameObject BailButton;
    public GameObject Prisioner;
    public GameObject PrisionDoor;
    public GameObject Prisionerdata;
    public GameObject PinPanel;
    public List<GameObject> CorrectPattren;
    public List<GameObject> ClickPattren;
    public TextMeshPro Hinttext;
    public GameObject BailHimImage;

    public GameObject PlayerMovePosition;
    public GameObject PoliceMovePosition;
    public GameObject PrisionerMovePosition;
    public GameObject VictoryPosition;
    [SerializeField] Animator Playeranim;
    [SerializeField] Animator Policeanim;

    public TextMeshProUGUI PinText;
    private RaycastHit hitinfo;
    private Tween lookat;
    public bool _Once;
    public bool pinlevel;
    public int correctcount;


    public List<Button> options;

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
        Playeranim.SetTrigger("walk");
        Policeanim.SetTrigger("Typing");

        OnMoveSeq();
    }

    void Start()
    {
        Playeranim = Player.GetComponent<Animator>();
        Policeanim = PoliceMan.GetComponent<Animator>();
    }

    // Update is called once per frame
    private bool _rayhit;

    void Update()
    {
        if (pinlevel)
        {
            if (ClickPattren.Count == 6 && !_Once)
            {
                _Once = true;
                PattrenCheck(ClickPattren);
                if (PattrenCheck(ClickPattren))
                {
                    BailButton.GetComponent<Collider>().enabled = true;
                    _rayhit = true;
                    PinPanel.SetActive(false);
                    Prisionerdata.SetActive(true);
                    Hinttext.enabled = false;
                    BailHimImage.SetActive(true);
                    BailHimImage.transform.DOShakeScale(0.3f, 0.4f, 8, 0, true);
                }
                else
                {
                    print("false");
                    for (int i = 0; i < ClickPattren.Count; i++)
                    {
                        ClickPattren[i].GetComponent<Button>().interactable = true;
                    }

                    ClickPattren.Clear();
                    _Once = false;
                    _rayhit = false;
                    PinText.text = "";
                }
            }
        }


        if (_rayhit)
        {
            Bailbutton();
        }
    }


    public void Choosecorrect()
    {
        correctcount += 1;
        if (correctcount == 5)
        {
            BailButton.GetComponent<Collider>().enabled = true;
            _rayhit = true;
            //  PinPanel.SetActive(false);
            //  Prisionerdata.SetActive(true);
            //  Hinttext.enabled = false;
            //  BailHimImage.SetActive(true);
            //  BailHimImage.transform.DOShakeScale(0.3f, 0.4f, 8, 0, true);
            Virtualcam.GetComponent<Animator>().SetTrigger("Pic");
            for (int i = 0; i < options.Count; i++)
            {
                options[i].interactable = false;
            }
        }
    }

    public void wrongans()
    {
        Debug.Log("cry");
        GameEvents.InvokeGameLose(-1);
        for (int i = 0; i < options.Count; i++)
        {
            options[i].interactable = false;
        }
    }

    public void OnMoveSeq()
    {
        var seq = DOTween.Sequence();
        if (AudioManager.instance)
        {
            AudioManager.instance.Play("Sneak Walk");
        }

        seq.Append(Player.transform.DOMove(PlayerMovePosition.transform.position, 2f).SetEase(Ease.Linear));
        seq.AppendCallback(() =>
        {
            if (AudioManager.instance)
            {
                AudioManager.instance.Pause("Sneak Walk");
            }

            Playeranim.SetTrigger("Sneak");
            PlayerMovePosition.transform.position = Chair.transform.position;
        });
        seq.AppendInterval(1.5f);
        seq.AppendCallback(() =>
        {
            Policeanim.SetTrigger("Stand");

            PoliceMan.transform.DOMoveX(PoliceMan.transform.position.x - 0.3f, 0.3f);
            if (AudioManager.instance)
            {
                AudioManager.instance.Play("Chair");
            }

            Chair.transform.DOMoveX(Chair.transform.position.x - 0.3f, 0.3f).OnComplete(() =>
            {
                if (AudioManager.instance)
                {
                    AudioManager.instance.Pause("Chair");
                }
            });

            Playeranim.SetTrigger("Idle");
        });
        seq.Append(PoliceMan.transform.DOLookAt(PoliceMovePosition.transform.position, 0.6f));

        seq.AppendCallback(() =>
        {
            //PoliceMan.transform.DOLookAt(PoliceMovePosition.transform.position, 3f);
            Policeanim.SetTrigger("Walk");
            if (AudioManager.instance)
            {
                AudioManager.instance.Play("Police Walk");
            }

            PoliceMan.transform.DOMove(PoliceMovePosition.transform.position, 2.5f).SetEase(Ease.Linear).OnComplete(
                () =>
                {
                    PoliceMan.SetActive(false);
                    if (AudioManager.instance)
                    {
                        AudioManager.instance.Pause("Police Walk");
                    }
                });
        });
        seq.AppendInterval(1f);
        seq.AppendCallback(() =>
        {
            Player.transform.DOLookAt(PlayerMovePosition.transform.position, 0.1f);
            Playeranim.SetTrigger("Sneakwalk");
            if (AudioManager.instance)
            {
                AudioManager.instance.Play("Sneak Walk");
            }

            Player.transform.DOMove(PlayerMovePosition.transform.position, 2f).SetEase(Ease.Linear).OnComplete(() =>
            {
                Player.transform.DOLookAt(Laptop.transform.position, 0.1f);
                Player.transform.DORotate(new Vector3(0, 90, 0), 0.3f).OnComplete(() =>
                {
                    Player.transform.SetParent(Chair.transform);
                });
            });
        });
        seq.AppendInterval(2f);
        seq.AppendCallback(() =>
        {
            if (AudioManager.instance)
            {
                AudioManager.instance.Pause("Sneak Walk");
            }

            Playeranim.SetTrigger("Sitting");
            Player.transform.DOMoveX(Player.transform.position.x - 0.2f, 0.3f).SetEase(Ease.Linear);
        });
        seq.AppendInterval(0.7f);
        seq.AppendCallback(() =>
        {
            Player.transform.DOMoveY(Player.transform.position.y + 0.03f, 0.7f);
            if (AudioManager.instance)
            {
                AudioManager.instance.Play("Chair");
            }
        });

        seq.Append(Chair.transform.DOMoveX(Chair.transform.position.x + 0.1f, 1f)).OnComplete(() =>
        {
            if (AudioManager.instance)
            {
                AudioManager.instance.Pause("Chair");
            }
        });
        seq.AppendInterval(0.2f);
        seq.AppendCallback(() => { Virtualcam.GetComponent<Animator>().SetTrigger(("Chair")); });
        seq.AppendInterval(0.1f);
        seq.AppendCallback(() => { Playeranim.SetTrigger("Typing"); });
        seq.AppendInterval(1f);
        seq.AppendCallback(() => { Virtualcam.GetComponent<Animator>().SetTrigger(("Laptop")); });
    }

    public void Prisionerseq()
    {
        var seq = DOTween.Sequence();
        seq.AppendCallback(() => { Virtualcam.GetComponent<Animator>().SetTrigger("Prisioner"); });
        seq.AppendInterval(2f);
        seq.AppendCallback(() =>
        {
            if (AudioManager.instance)
            {
                AudioManager.instance.Play("Prision Gate");
            }
        });
        seq.Append(PrisionDoor.transform.DOLocalRotate(new Vector3(0, -150, 0), 0.4f).OnComplete(() =>
        {
            if (AudioManager.instance)
            {
                AudioManager.instance.Pause("Prision Gate");
            }

            Prisioner.GetComponent<Animator>().SetTrigger("Run");
            if (AudioManager.instance)
            {
                AudioManager.instance.Play("Running");
            }
        }));
        seq.AppendInterval(0.2f);
        seq.Append(Prisioner.transform.DOMove(VictoryPosition.transform.position, 1.5f).SetEase(Ease.Linear).OnComplete(
            () =>
            {
                if (AudioManager.instance)
                {
                    AudioManager.instance.Pause("Running");
                }

                Prisioner.GetComponent<Animator>().SetTrigger("Victory");
            }));
        //seq.Append(Prisioner.transform.DOLookAt(VictoryPosition.transform.position, 0.2f));
        seq.AppendInterval(0.5f);
        seq.AppendCallback(() =>
        {
            if (AudioManager.instance)
            {
                AudioManager.instance.Play("Running");
            }

            Prisioner.transform.DOLookAt(PrisionerMovePosition.transform.position, 0.1f);
            Prisioner.GetComponent<Animator>().SetTrigger("Run");
        });
        seq.Append(Prisioner.transform.DOMove(PrisionerMovePosition.transform.position, 3f).SetEase(Ease.Linear)
            .OnComplete(
                () =>
                {
                    if (AudioManager.instance)
                    {
                        AudioManager.instance.Pause("Running");
                    }
                }));
        seq.AppendCallback(() => { GameEvents.InvokeGameWin(); });
    }

    public void Bailbutton()
    {
        if (Input.GetMouseButton(0))
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hitinfo) && hitinfo.transform.name == "Bail Button")
            {
                if (AudioManager.instance)
                {
                    AudioManager.instance.Play("Bail Button");
                }

                Vibration.Vibrate(30);
                BailButton.transform.DOMoveY(BailButton.transform.position.y - 0.05f, 0.1f).SetEase(Ease.Linear)
                    .OnComplete(() =>
                    {
                        BailButton.transform.DOMoveY(BailButton.transform.position.y + 0.05f, 0.1f).SetEase(Ease.Linear)
                            .OnComplete(
                                () => { Prisionerseq(); });
                    });
                _rayhit = false;
            }
        }
    }

    public bool PattrenCheck(List<GameObject> Name)
    {
        for (int i = 0; i < Name.Count; i++)
        {
            if (Name[i] != CorrectPattren[i])
            {
                return false;
            }
        }

        return true;
    }

    public void Buttonpress(GameObject Obje)
    {
        if (!ClickPattren.Contains(Obje))
        {
            PinText.text += " *";
            Obje.transform.GetComponent<Button>().interactable = false;
            if (AudioManager.instance)
            {
                AudioManager.instance.Play("Button");
            }

            Vibration.Vibrate(30);
            ClickPattren.Add(Obje);
        }
    }
}