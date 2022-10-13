using DG.Tweening;
using TMPro;
using UnityEngine;

namespace Meta
{
	public class MetaCanvas : MonoBehaviour
	{
		[SerializeField] private TextMeshProUGUI cash;

		private void OnEnable()
		{
			MetaEvents.AlterBankBalance += OnAlterBankBalance;
		}

		private void OnDisable()
		{
			MetaEvents.AlterBankBalance -= OnAlterBankBalance;
		}

		private void Start() => SetCashText();

		private void SetCashText(int newAmount = -1)
		{
			if (newAmount == -1)
				cash.text = $"$ {ShopStateController.CurrentState.GetBankBalance}";
			else
				cash.text = $"$ {newAmount}";
		}

		private void TweenCashText(int oldAmount, int newAmount)
		{
			var temp = oldAmount;
			DOTween.To(() => temp, value => temp = value, newAmount, 1f)
				.SetEase(Ease.OutCubic)
				.OnUpdate(() => cash.text = $"$ {temp}");
		}

		private void OnAlterBankBalance(int oldAmount, int newAmount) => TweenCashText(oldAmount, newAmount);
	}
}