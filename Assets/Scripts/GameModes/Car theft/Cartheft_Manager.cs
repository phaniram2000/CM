using System;
using DG.Tweening;
using RPS;
using UnityEngine;

enum Stages
{
    Default,
    locker,
    wireattach,
    win,
    lost
}

public class Cartheft_Manager : MonoBehaviour
{
    public static event Action Onmeater;
    Stages E_Stages;
    public Transform nailpicker,  movepoint,door;
    public Transform Ui_Help;
    [Header("Barmeater")] public Transform _transform;
    [SerializeField] private Transform  arrowHolder;
    [SerializeField] private float arrowRotationDuration, rotationInitialPos, rotateEndPos, scale;
    private Tween arrowHolderTween;

    [Header("Animators")] public Animator camera,
        car;

    public Animator police;
    public Animator player;
    public int WireAttachcount;

    private void OnEnable()
    {
        GameEvents.TapToPlay += Ontaptoplay;
        Cartheft_Car.Ondoordidnotopen += OnDoorDidNotOpen;
        Cartheft_Car.OnDooropen += OndooropenSequence;
        WireEndCube.Oncount += OnwireAttachcount;
    }

    private void OnDisable()
    {
        GameEvents.TapToPlay -= Ontaptoplay;
        Cartheft_Car.Ondoordidnotopen -= OnDoorDidNotOpen;
        Cartheft_Car.OnDooropen -= OndooropenSequence;
        WireEndCube.Oncount -= OnwireAttachcount;
    }

    void Start()
    {
        _transform.localScale = Vector3.zero;

        if (!_transform.root.TryGetComponent(out CharacterRefBank refBank)) return;
    }

    private void Update()
    {
        switch (E_Stages)
        {
            case Stages.Default:
                DefaultUpdate();
                break;
            case Stages.locker:
                LockerUpdate();
                break;
            case Stages.wireattach:

                break;
            case Stages.win:
                DooropenxEcute();
                break;
            case Stages.lost:
                DoordidnotopenExecute();
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }


    private void DefaultUpdate()
    {
    }

    private void LockerUpdate()
    {
        if (Input.GetMouseButtonDown(0))
        {
            arrowHolderTween.Kill();
            _transform.DOScale(Vector3.zero, .3f).SetEase(Ease.InBounce);
            
            var arrowValue = arrowHolder.transform.localEulerAngles.z;
            if (arrowValue > 33f)
                arrowValue -= 360f;

            arrowValue = Mathf.Abs(arrowValue);
            print("arrow value: " + arrowValue);
            if (arrowValue > 20)
            {
                E_Stages = Stages.lost;
            }
            else
            {
                E_Stages = Stages.win;
            }

            if (AudioManager.instance)
                AudioManager.instance.Pause("tick"); 
            AudioManager.instance.Play("pop");
            Vibration.Vibrate(30);
        }
    }

    private void DooropenxEcute()
    {
        car.SetTrigger("Win");
        /// StartCoroutine(Delaydoorcar());
    }

    private void DoordidnotopenExecute()
    {
        //temp
        car.SetTrigger("Lost");
    }

    public void OndooropenSequence()
    {
        var seq = DOTween.Sequence();
        seq.AppendCallback(() =>
        {
            camera.SetTrigger("Win");
            nailpicker.DOScale(Vector3.zero, .3f).SetEase(Ease.InBounce);
            police.transform.DOScale(Vector3.zero, .3f);
            player.transform.DOScale(Vector3.zero, .3f);

        });
        seq.AppendInterval(1);
        seq.Append(Ui_Help.DOScale(Vector3.one, .3f).SetEase(Ease.OutBounce));
        seq.AppendInterval(15);
        seq.Append(Ui_Help.DOScale(Vector3.zero, .3f).SetEase(Ease.OutBounce));
    }

    public void ONwireattachdoneSequence()
    {
        var seq = DOTween.Sequence();
        car.enabled = false;
        seq.AppendInterval(2f);
        seq.AppendCallback(() => { camera.SetTrigger("Carmove"); });
        seq.AppendInterval(1.5f);
        seq.Append(door.transform.DOLocalRotate(new Vector3(0,0,0), .1f));
        seq.AppendCallback(()=>
        {
            
            car.transform.DOMove(movepoint.position, 4f);
            if(AudioManager.instance)
                AudioManager.instance.Play("Car");
            
            
        });
         seq.AppendCallback(() => { GameEvents.InvokeGameWin(); });
    }

    private void Ontaptoplay()
    {
        camera.SetTrigger("Lock");
        E_Stages = Stages.Default;
        DOVirtual.DelayedCall(1.2f, () =>
        {
            _transform.DOScale(Vector3.one * scale, 0.7f).SetEase(Ease.OutBack).OnComplete(() =>
            {
                arrowHolder.localRotation = Quaternion.Euler(0, 0, rotationInitialPos);
                arrowHolderTween = arrowHolder.DOLocalRotate(new Vector3(0, 0, rotateEndPos), arrowRotationDuration)
                    .SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);
                E_Stages = Stages.locker;
            });
            if (AudioManager.instance)
                AudioManager.instance.Play("tick");
        });
    }

    public void OnDoorDidNotOpen()
    {
        FailSequence();
        DOVirtual.DelayedCall(.4f, () =>
        {
            if (AudioManager.instance)
                AudioManager.instance.Play("Police");
            GameEvents.InvokeGameLose(-1);

        });
    }

    public void FailSequence()
    {
        var seq = DOTween.Sequence();
        seq.AppendCallback(() => camera.SetTrigger("Lost"));
        seq.AppendInterval(1f);
        seq.AppendCallback(() => player.SetTrigger("Run"));
        seq.AppendInterval(2f);
        seq.Append(player.transform.DORotate(new Vector3(0, 180, 0), .3f));
        seq.AppendCallback(() => police.SetTrigger("Run"));
        seq.Append(police.transform.DOLookAt(player.transform.position, .3f));
    }

    public void OnwireAttachcount()
    {
        WireAttachcount += 1;
        if (WireAttachcount == 8)
        {
         
            ONwireattachdoneSequence();
        }
    }
}