using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;



public class CommonShopePanel : MonoBehaviour
{

    //public GameObject videoButton;  
    public GameObject randomButton;

    private int totalUnlocked;
    public int totalCoins;

    public Sprite ActiveButtonSprite;
    public Sprite inActiveButtonSprite;

    [Header("COMMON LIST")]
    public List<GameObject> commonList = new List<GameObject>();


    private showPlayersUI showPlayersUI;

    public void Start()
    {

        for (int i = 0; i < commonList.Count; i++)
        {
            if(PlayerPrefs.GetInt("UnLocked" + i) == i)
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
       
        if (totalCoins >= 250)
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
        //Plugins.instance.isCommonReward = true;
        //Plugins.instance.ShowRewardedAd();
        SuccessfullyUnlocked();
    } 

    public void SuccessfullyUnlocked()
    {
        StartCoroutine(selectingRandom());
    }

    public IEnumerator selectingRandom()
    {
        int j = 0;
        while (j < 7)
        {
            int ran = Random.Range(0, commonList.Count);
            if (ran != PlayerPrefs.GetInt("UnLocked" + ran))
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
        if (totalUnlocked < 6)
        {
            int ran = Random.Range(0, commonList.Count);
            if (ran != PlayerPrefs.GetInt("UnLocked" + ran))
            {
                PlayerPrefs.SetInt("UnLocked" + ran, ran);
                commonList[ran].transform.GetChild(1).gameObject.SetActive(true);
                commonList[ran].GetComponent<Button>().interactable = true;
                totalUnlocked++;
                //OnSelectPlayerButtonPress(ran);
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
        if (currentCoins >= 250)
        {
            currentCoins -= 250;
            PlayerPrefs.SetInt("totalCoins", currentCoins);
            PlayerPrefs.Save();
            SuccessfullyUnlocked();
        }
    }
   
   public int click;
    int temp=-1;
    public void OnSelectPlayerButtonPress(int selected)
    {
        showPlayersUI.SelectedCharacter("common",selected);
       

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
