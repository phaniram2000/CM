using DG.Tweening;
using UnityEngine;

public class StealNRun_SuiteGuard : MonoBehaviour
{
    private static Animator _myAnimator;
    public GameObject suiteCase, catchHer;
    private static readonly int GetRobbed = Animator.StringToHash("GetRobbed");
    private static readonly int Walk = Animator.StringToHash("WalkBriefCase");
   private void Awake()
   {
       _myAnimator = GetComponent<Animator>();
   }

   public void OnStealPoint()
   {
       catchHer.SetActive(true);
       suiteCase.SetActive(false);
       StealNRun_PlayerController.instance.suiteCase.SetActive(true);
       AudioManager.instance.Play("Hey");
   }

   public static void WalkWithBriefCase()
   {
       _myAnimator.transform.DOMoveZ(13f, 0.5f).SetEase(Ease.Flash);
       _myAnimator.SetTrigger(Walk);
   }
   
   public static void GetRobbedNow()
   {
       _myAnimator.SetTrigger(GetRobbed);
   }
}
