using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class PlayerMoneyCollect : MonoBehaviour
{
	public static PlayerMoneyCollect player;
	
	[Header("ParticleEffect ")] public GameObject moneySpendParticleFx;
	[SerializeField] private Transform moneyParentTransform;
	[SerializeField] private List<Transform> moneyListParentTrans = new();
	[SerializeField] private RectTransform moneyIcon;

	private Tween _collectionTween;

	private static MetaUiManager _uiManager;
	private int _maxCoinsSpawnCount;
	private int _num;

	private void Awake()
	{
		if (!player) player = this;
		else Destroy(gameObject);
	}

	private void Start()
	{
		_uiManager = MetaUiManager.instance;
		
		AddMoneyToList();
		moneySpendParticleFx.SetActive(false);
	}

	private void AddMoneyToList()
	{
		for (var i = 0; i < moneyParentTransform.childCount; i++)
			moneyListParentTrans.Add(moneyParentTransform.GetChild(i));
	}

	private static void TapSound()
	{
		MetaAudioManager.instance.PlayTapSound();
		if (Application.platform == RuntimePlatform.Android) Vibration.Vibrate(15);
	}

	private static void ReceiveMoney(int amount) => MyPlayerPrefsSave.SetTotalMoney(
		MyPlayerPrefsSave.GetTotalMoney() + amount);

	private void PutAwayMoney(GameObject money, CollectableArea area)
	{
		var item = money;
		area.RemoveItemFromArray(item);
		item.transform.parent = transform;

		const float dur = 0.25f;
		item.transform.DOScale(0f, dur * 1.5f)
			.SetEase(Ease.InBack)
			.OnComplete(() => item.gameObject.SetActive(false));
		item.transform.DOLocalMove(Vector3.zero, dur)
			.SetEase(Ease.InOutBack)
			.OnComplete(() =>
			{
				item.transform.parent = null;
				MoneyEffect();
			});
	}
	
	private void MoneyEffect()
	{
		_maxCoinsSpawnCount = _uiManager.moneyPrefabCount;

		var index = _num++ % _maxCoinsSpawnCount;
		
		_uiManager.moneyParent.GetChild(index).gameObject.SetActive(true);
		var rect = (RectTransform)_uiManager.moneyParent.GetChild(index)
					.transform;
		rect.DOMove(moneyIcon.position, 0.5f)
			.SetEase(Ease.Linear)
			.OnComplete(() =>
			{
				_uiManager.moneyParent.GetChild(index).gameObject.SetActive(false);
				NormalizeRect((RectTransform)_uiManager.moneyParent.GetChild(index).transform);
			});
	}

	private static void NormalizeRect(RectTransform rect) => rect.anchoredPosition = Vector2.zero;

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag("Shopping")) 
			moneySpendParticleFx.SetActive(true);
		if(MyPlayerPrefsSave.GetTotalMoney()<=0)
			moneySpendParticleFx.SetActive(false);
		if (!other.gameObject.CompareTag("CollectibleMoney")) return;
		if(!other.TryGetComponent(out CollectibleMoney money)) return;
		if(!money.isCollectible) return;

		money.isCollectible = false;
		TapSound();
		ReceiveMoney(money.area.revenue);
		PutAwayMoney(other.gameObject, money.area);
	}
	
	private void OnTriggerExit(Collider other)
	{
		if (other.gameObject.CompareTag("Shopping")) 
			moneySpendParticleFx.SetActive(false);
	}
}