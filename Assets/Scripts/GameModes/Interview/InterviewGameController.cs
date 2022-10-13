using DG.Tweening;
using Meta;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InterviewGameController : MonoBehaviour
{
   [SerializeField] private GameObject cashBundle, cashParticleSystem;

   
   private static readonly string InterviewSceneShowedId = "interviewSceneShowed";
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
      cashBundle.SetActive(false);
      cashParticleSystem.SetActive(false);
   }

   private void OnBonusGiven()
   {
      if(AudioManager.instance)
         AudioManager.instance.Play("Correct");
      
      
      cashBundle.SetActive(true);
      cashParticleSystem.SetActive(true);
      DOVirtual.DelayedCall(0.1f, () =>
      {
         GameEvents.InvokeGameWin();
         DOVirtual.DelayedCall(3.6f,()=>
         {
            ShopStateController.AlterBankBalance(100, true);
            PlayerPrefs.SetInt(InterviewSceneShowedId,1);
            
         });
      });
   }
   int LevelData()
   {
      int val = PlayerPrefs.GetInt("levelNo", 1);

      if (PlayerPrefs.GetInt("interviewSceneShowed") == 0)
      {
         val = 1;
      }
      else val++;

      return val;
   }
   public void OnNextButtonPressed()
   {
      int index = SceneManager.GetActiveScene().buildIndex;
      SceneManager.LoadScene(index);
      if(GAScript.Instance)
      {
         GAScript.Instance.LevelCompleted((LevelData()-1).ToString());
      }
   }
}
