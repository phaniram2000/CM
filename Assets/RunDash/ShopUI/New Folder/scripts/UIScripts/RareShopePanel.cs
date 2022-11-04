using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class RareShopePanel : MonoBehaviour
{
    //public GameObject videoButton;  
    public GameObject randomButton;

    private int totalUnlocked;
    public int totalCoins;

    public Sprite ActiveButtonSprite;
    public Sprite inActiveButtonSprite;




    [Header("Rare LIST")]
    public List<GameObject> commonList = new List<GameObject>();
   
    private showPlayersUI showPlayersUI;



    public void Start()
    {
        for (int i = 0; i < commonList.Count; i++)
        {
            if (PlayerPrefs.GetInt("RareUnLocked" + i,-1) == i)
            {
                commonList[i].transform.GetChild(1).gameObject.SetActive(true);
                commonList[i].GetComponent<Button>().interactable = true;

                totalUnlocked++;
            }
        }
        showPlayersUI = GameObject.FindObjectOfType<showPlayersUI>();

    }

    private void LateUpdate()
    {

        if (totalCoins >= 500)
        {
            randomButton.GetComponent<Image>().sprite = ActiveButtonSprite;
        }
        else
        {
            randomButton.GetComponent<Image>().sprite = inActiveButtonSprite;
        }
    }

    public void OnRandomUnlock_Video_ButtonPress()
    {
        //Plugins.instance.isRareReward = true;
        //Plugins.instance.ShowRewardedAd();
         GetCoinsByWatchingVideo();
    }
    public void GetCoinsByWatchingVideo()
    {
        int currentCoins = totalCoins;
            currentCoins += 200;
            PlayerPrefs.SetInt("totalCoins", currentCoins);
            PlayerPrefs.Save();
    }

    public void SuccessfullyUnlocked()
    {
        StartCoroutine(selectingRandom());
    }

    public IEnumerator selectingRandom()
    {
        int j = 0;
        while (j < 10)
        {
            int ran = Random.Range(0, commonList.Count);
            if (ran != PlayerPrefs.GetInt("RareUnLocked" + ran,-1))
            {
                commonList[ran].transform.GetChild(2).gameObject.SetActive(true);
                //SoundManager.instance.PlaySound(SoundManager.instance.click);
                yield return new WaitForSeconds(0.1f);
                commonList[ran].transform.GetChild(2).gameObject.SetActive(false);
            }
            j++;
        }
        UnLock();
    }
    private void UnLock()
    {
        if (totalUnlocked < 9)
        {
            int ran = Random.Range(0, commonList.Count);
            if (ran != PlayerPrefs.GetInt("RareUnLocked" + ran,-1))
            {
                PlayerPrefs.SetInt("RareUnLocked" + ran, ran);
                commonList[ran].transform.GetChild(1).gameObject.SetActive(true);
                commonList[ran].GetComponent<Button>().interactable = true;
                OnSelectPlayerButtonPress(ran);

                totalUnlocked++;
            }
            else
            {
                UnLock();
            }
        }

    }



    public void OnRandomUnlock_Coins_ButtonPress()
    {
        int currentCoins = totalCoins;
        if (currentCoins >= 500)
        {
            currentCoins -= 500;
            PlayerPrefs.SetInt("totalCoins", currentCoins);
            PlayerPrefs.Save();
            SuccessfullyUnlocked();
        }
    }
    private  int click;
    private int temp;
    public void OnSelectPlayerButtonPress(int selected)
    {
        //showPlayersUI.SelectedCharacter("rare", selected);
        if (temp != selected)
        {
            temp = selected;
            if (click == 1)
            {
                click = 0;
            }
            click++;
        }
        else
        {
            click++;
        }
        if (click == 2)
        {
            click = 0;
            GetComponent<ShopManager>().shop.SetActive(false);
        }
    }
}
