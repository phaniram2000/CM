using UnityEngine;

public class JCStateFullCameraController : MonoBehaviour
{
    private Animator _anim;
   
    private static readonly int Win = Animator.StringToHash("win");

    private void OnEnable()
    {
        JCEvents.MakeLockerTransformToGirlHand += OnMakeLockerTransformToGirlHand;
        JCEvents.MakeGirlSad += OnMakeGirlSad;
    }

    private void OnDisable()
    {
        JCEvents.MakeLockerTransformToGirlHand -= OnMakeLockerTransformToGirlHand;
        JCEvents.MakeGirlSad -= OnMakeGirlSad;
    }

    
    private void Start()
    {
        _anim = GetComponent<Animator>();
    }
    
    private void OnMakeLockerTransformToGirlHand()
    {
        _anim.SetTrigger(Win);
    }
    
    private void OnMakeGirlSad()
    {
       _anim.SetTrigger(Win);
    }
    
    
    
}
