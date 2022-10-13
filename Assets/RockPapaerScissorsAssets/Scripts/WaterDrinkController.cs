using DG.Tweening;
using UnityEngine;

namespace RPS
{



	public class WaterDrinkController : MonoBehaviour
	{
		[SerializeField] private float delayedTime, durationTime;
		private Transform _transform;

		private void Start()
		{
			DOVirtual.DelayedCall(delayedTime,
				() =>
				{
					transform.DOScaleZ(0, durationTime).SetEase(Ease.Linear).OnComplete(() =>
					{
						gameObject.SetActive(false);
					});
				});


		}
	}

}
