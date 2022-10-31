using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;


namespace RPS
{
    public class RPSPowerSlapController : MonoBehaviour
    {
        [SerializeField] private bool enablePowerSlap;
        [SerializeField] private GameObject powerSlapButton;
       

        private void OnEnable()
        {
            RPSGameEvents.AllowPlayerToSlap += OnAllowPlayerToSlap;
            RPSGameEvents.PlayerStartGiveSlap += OnPlayerStartGiveSlap;
            RPSGameEvents.NewRound += OnNewRound;
        }

        private void OnDisable()
        {
            RPSGameEvents.AllowPlayerToSlap -= OnAllowPlayerToSlap;
            RPSGameEvents.PlayerStartGiveSlap -= OnPlayerStartGiveSlap;
            RPSGameEvents.NewRound -= OnNewRound;
        }

        private void Start()
        {
            powerSlapButton.transform.localScale = Vector3.zero;
        }

        private void OnAllowPlayerToSlap()
        {
            if (enablePowerSlap)
                powerSlapButton.transform.DOScale(Vector3.one, 0.25f).SetEase(Ease.OutBack)
                    .OnComplete(EnablePowerSlapUiButton);
        }
        
        private void EnablePowerSlapUiButton()
        {
            powerSlapButton.GetComponent<Button>().interactable = true;
        }
        
        public void PowerSlapButtonOutAnimation()
        {
            DisablePowerSlapUiButton();
            powerSlapButton.transform.DOScale(Vector3.zero, 0.25f).SetEase(Ease.OutBack);
        }
        
        private void DisablePowerSlapUiButton()
        {
            powerSlapButton.GetComponent<Button>().interactable = false;
        }
        
        private void OnPlayerStartGiveSlap()
        {
           PowerSlapButtonOutAnimation();
        }
        
        private void OnNewRound()
        {
            PowerSlapButtonOutAnimation();
        }

        public void OnPowerButtonPressed()
        {
            RPSGameEvents.InvokeOnPowerSlapGiven();
            RPSGameEvents.InvokeOnPlayerStartGiveSlap();
        }
    }
}


