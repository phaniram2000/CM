using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Kart
{
	public class Fever : MonoBehaviour
	{
		[SerializeField] private Image fever;
		[SerializeField] private float feverFillMultiplier;
		[SerializeField] private float feverEmptyMultiplier;
		[SerializeField] private GameObject feverOverlayPanel;

		[SerializeField] private GameObject feverText;

		private float _feverShopMultiplier = 1f;
		private bool _toConsumeFever;

		private MainKartController _my;
		
		private void OnEnable()
		{
			GameEventsR.PlayerOnFever += OnFeverOverlay;
			GameEventsR.PlayerOnFever += PlayFeverHype;
			GameEventsR.PlayerOffFever += OffFeverOverlay;
		}
		
		private void OnDisable()
		{
			GameEventsR.PlayerOnFever -= OnFeverOverlay;
			GameEventsR.PlayerOnFever -= PlayFeverHype;
			GameEventsR.PlayerOffFever -= OffFeverOverlay;
		}

		private void Start()
		{
			_my = GetComponentInParent<MainKartController>();
			fever.fillAmount = 0f;
		}

		public void HandleFeverAmount()
		{
			if(_toConsumeFever)
				ConsumeFever();
			else
				IncreaseFeverAmount();
		}

		public void DecreaseFeverAmount()
		{
			if (fever.fillAmount <= 0f)
			{
				if (_toConsumeFever)
				{
					GameEventsR.InvokePlayerOffFever();
					DampCameraR.only.StopMediatingTarget();
				}
				return;
			}

			fever.fillAmount -= Time.deltaTime * feverEmptyMultiplier;
		}

		public void UpdateFeverShopMultiplier(float multiplier)
		{
			_feverShopMultiplier = 1 + 0.1f * Mathf.Clamp(multiplier, 0f, 1f);
		}

		private void ConsumeFever()
		{
			if (fever.fillAmount <= 0f)
			{
				_toConsumeFever = false;
				GameEventsR.InvokePlayerOffFever();
				DampCameraR.only.StopMediatingTarget();
				return;
			}
			//CameraFxController.only.UpdateRumblerRotation(fever.fillAmount);
			fever.fillAmount -= Time.deltaTime * feverFillMultiplier * 1.4f;
		}

		private void IncreaseFeverAmount()
		{
			if (fever.fillAmount >= 1f)
			{
				_toConsumeFever = true;
				GameEventsR.InvokePlayerOnFever(); 
				Vibration.Vibrate(20);
				return;
			}
			if(!_toConsumeFever)
				fever.fillAmount += Time.deltaTime * feverFillMultiplier * _feverShopMultiplier;
		}

		private void OnTriggerEnter(Collider other)
		{
			if (!_toConsumeFever) return;

			if (!other.CompareTag("Obstacle") && !other.CompareTag("ObstacleKart")) return;
			
			//Explode Obstacle karts
			if (other.TryGetComponent(out AdditionalKartController additionalKart))
				additionalKart.RemoveKartsFromHere(other.transform.position);
			
			_my.PlayExplosionParticle(other.ClosestPoint(transform.position));
			Vibration.Vibrate(10);
			
			CameraFxControllerR.only.ScreenShake(5f);
			TimeController.only.SlowDownTime();
			DOVirtual.DelayedCall(0.5f, () => TimeController.only.RevertTime());
		}

		private void OnFeverOverlay()
		{
			feverOverlayPanel.SetActive(true);
			feverText.SetActive(true);
		}

		private void OffFeverOverlay()
		{
			feverOverlayPanel.SetActive(false);
			feverText.SetActive(false);
		}
		
		private void PlayFeverHype() => AudioManagerR.instance.Play("Jump1" );
	}
}