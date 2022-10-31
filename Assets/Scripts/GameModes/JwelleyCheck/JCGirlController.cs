using DG.Tweening;
using UnityEngine;

public class JCGirlController : MonoBehaviour
{
    private Animator _anim;
    
    private static readonly int Lose = Animator.StringToHash("lose");
    private static readonly int Win = Animator.StringToHash("win");
    private static readonly int Hold = Animator.StringToHash("hold");
    private static readonly int Run = Animator.StringToHash("run");
    [SerializeField] private Transform girlWalkEndPoint;
    [SerializeField] private float walkDuration;

    private void OnEnable()
    {
        JCEvents.CloseLocker += GirlHoldPosition;
        JCEvents.MakeGirlRunWithLocker += OnMakeGirlRunWithLocker;
        JCEvents.MakeGirlSad += OnMakeGirlSad;
    }

    private void OnDisable()
    {
        JCEvents.CloseLocker -= GirlHoldPosition;
        JCEvents.MakeGirlRunWithLocker -= OnMakeGirlRunWithLocker;
        JCEvents.MakeGirlSad -= OnMakeGirlSad;
    }

    private void Start()
    {
        _anim = GetComponent<Animator>();
    }

    private void GirlHoldPosition()
    {
        _anim.SetTrigger(Hold);
    }

    private void GirlRun()
    {
        _anim.SetTrigger(Run);
        transform.DOLookAt(girlWalkEndPoint.position, 0.1f).SetEase(Ease.Linear).OnComplete(() =>
        {
            transform.DOMove(girlWalkEndPoint.position, walkDuration).SetEase(Ease.Linear).OnComplete(()=>
            {
                
                
            });
        });
    }
    
    private void OnMakeGirlRunWithLocker()
    {
        GirlRun();

        GameEvents.InvokeGameWin();
    }
    
    private void OnMakeGirlSad()
    {
        DOVirtual.DelayedCall(0.2f, () =>
        {
            _anim.SetTrigger(Lose);
            GameEvents.InvokeGameLose(-1);
            
        });
    }


}
