using UnityEngine;

public class InterviewGirlController : MonoBehaviour
{
   private Animator _anim;

   
   private static readonly int Win = Animator.StringToHash("win");

   private void OnEnable()
   {
      InterviewEvents.BonusGiven += OnBonusGiven;
   }

   private void OnDisable()
   {
      InterviewEvents.BonusGiven -= OnBonusGiven;
   }

  
   private void Start()
   {
      _anim = GetComponent<Animator>();
   }
   
   private void OnBonusGiven()
   {
      _anim.SetTrigger(Win);
   }

   
   
   
   
   
}
