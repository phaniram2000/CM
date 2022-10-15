using System.Collections;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class MetaShop : MonoBehaviour
{
	private enum MetaShopState { Locked, Unlocked };

	[Header("Shop Details")] public GameObject cam;
	[SerializeField] private MetaShopState buildingState;
	[SerializeField] private int myPrice, myRevenue;

	[SerializeField] private GameObject markerObj, shop;
	[SerializeField] private Transform mesh;
	[SerializeField] private TextMeshPro priceText, revenueText;
	[SerializeField] private float numb;
	[SerializeField] private bool isSelling, isUnlocked, once;

	private CustomerRotation _rotation;
	private float _timer;

	private void Start()
    {
		_rotation = GetComponent<CustomerRotation>();
		if(_rotation) _rotation.area.revenue = myRevenue;
		if(revenueText)
			revenueText.text = $"${myRevenue}/item";
		
		PlayerPrefs.SetInt(gameObject.name, PlayerPrefs.GetInt(gameObject.name, 0));
		if (PlayerPrefs.GetInt(gameObject.name + "Unlock", 0) <= 0)
			PlayerPrefs.SetInt(gameObject.name + "Unlock", PlayerPrefs.GetInt(gameObject.name + "Unlock", 0));
		else
		{
            LoadBuildings();
            buildingState = MetaShopState.Unlocked;
			if(_rotation) _rotation.UnlockShop();
			revenueText.gameObject.SetActive(true);
			if(revenueText)
				revenueText.text = $"${myRevenue}/item";
			once = true;
        }
        if (PlayerPrefs.GetInt(gameObject.name, 0) <= 0 && !isUnlocked && buildingState == MetaShopState.Locked) 
			PlayerPrefs.SetInt(gameObject.name, myPrice);
		myPrice = PlayerPrefs.GetInt(gameObject.name, 0);
    }

	private void Update()
    {
		if(priceText)
			priceText.text = $"Price : {myPrice} $";
        Buying();
        Unlocked();
    }

	private void Buying()
	{
		numb = MyPlayerPrefsSave.GetTotalMoney();
		if (!isSelling || !(numb > 0)) return;
		
		if (myPrice > 0)
		{
			myPrice -= 4;
			if (numb > 0)
			{
				numb -= 4;
				MyPlayerPrefsSave.SetTotalMoney(numb);
				PlayerPrefs.SetInt(gameObject.name, myPrice);
			}
		}

		if (myPrice > 0 && PlayerPrefs.GetInt(gameObject.name, 0) > 0) return;
		
		myPrice = 0;
		buildingState = MetaShopState.Unlocked;
		PlayerPrefs.SetInt(gameObject.name + "Unlock", PlayerPrefs.GetInt(gameObject.name + "Unlock", 0) + 1);
		PlayerMoneyCollect.player.moneySpendParticleFx.SetActive(false);
		isUnlocked = true;
	}

	private void Unlocked()
	{
		if (!isUnlocked || once) return;
		
		Debug.Log("Sold" + "-------------------" + "Item Unlock");
		CloudParticleEffect();
		if(_rotation)
			_rotation.UnlockShop();
		shop.SetActive(true);
		mesh.transform.DOScale(0f, 0.5f).From().SetEase(Ease.OutBack);
		markerObj.SetActive(false);
		priceText.gameObject.SetActive(false);
		revenueText.gameObject.SetActive(true);
		PlayerPrefs.SetInt(GameManager.instance.myIndex, PlayerPrefs.GetInt(GameManager.instance.myIndex, 0) + 1);
		GameManager.instance.once = false;
		UnlockSound();
		once = true;
	}

	private static void UnlockSound()
    {
        if (MetaAudioManager.instance) MetaAudioManager.instance.PlayBuildingUnlockSound();
		if (Application.platform == RuntimePlatform.Android) Vibration.Vibrate(10);
	}

	private void LoadBuildings()
    {
        shop.SetActive(true);
        mesh.DOScale(0f, 1).From().SetEase(Ease.OutBack);
        markerObj.SetActive(false);
        priceText.gameObject.SetActive(false);
        GameManager.instance.once = false;
    }

	private void CloudParticleEffect()
    {
        var cloudpar = GameManager.instance.cloudParticle;
        var pos = transform.position + new Vector3(0, 2, 0);
        var go = Instantiate(cloudpar, pos, Quaternion.identity).gameObject;
        StartCoroutine(InActivatingObj(go));
    }

	private IEnumerator InActivatingObj(GameObject cloudParticle)
    {
        yield return new WaitForSeconds(1.5f);
        cloudParticle.SetActive(false);
    }

	private void OnApplicationQuit()
    {
        myPrice = PlayerPrefs.GetInt(gameObject.name, 0);
    }

	private void OnTriggerStay(Collider other)
	{
		if (!other.gameObject.CompareTag("Player")) return;
		
		isSelling = true;
		switch (buildingState)
		{
			case MetaShopState.Locked when numb > 0:
			{
				if (MetaAudioManager.instance) MetaAudioManager.instance.TurnOnAS3();
				_timer += Time.deltaTime;

				if (Application.platform == RuntimePlatform.Android && _timer > 0.4f)
				{
					Vibration.Vibrate(5);
					_timer = 0;
				}

				break;
			}
			case MetaShopState.Locked:
			{
				if (MetaAudioManager.instance) MetaAudioManager.instance.TurnOFFAS3();
				break;
			}
		}
	}
    private void OnTriggerExit(Collider other)
	{
		if (!other.gameObject.CompareTag("Player")) return;
		
		isSelling = false;
		if (MetaAudioManager.instance) MetaAudioManager.instance.TurnOFFAS3();
	}
}