using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlassUnlocking : MonoBehaviour
{
   // public List<GameObject> commonList = new List<GameObject>();
    //private int claimCharacter;
    //public int totalUnlocked;
    public GameObject videoButton;
    public GameObject claimButton;
    private showPlayersUI showPlayersUI;

    //public GlassFilling glassFilling;

    void Start()
    {
        showPlayersUI = GameObject.FindGameObjectWithTag("showPlayersUI").GetComponent<showPlayersUI>();
        //Genarate_RandomNewItem();
    }


    //public void Genarate_RandomNewItem()
    //{
    //    for (int i = 0; i < 6; i++)
    //    {
    //        if (PlayerPrefs.GetInt("UnLocked" + i) == i)
    //        {
    //            totalUnlocked++;
    //        }
    //    }
    //    if (totalUnlocked < 6)
    //    {
    //        int ran = Random.Range(0, commonList.Count);
    //        if (ran != PlayerPrefs.GetInt("UnLocked" + ran))
    //        {
    //            claimCharacter = ran;
    //            ActiveRandomItem(ran);
    //            totalUnlocked++;
    //        }
    //        else
    //        {
    //            Genarate_RandomNewItem();
    //        }
    //    }

    //}
    ////public void ActiveRandomItem(int ran)
    //{
    //    for (int i = 0; i < showPlayersUI.newItemUnlockCommon.transform.GetChild(0).childCount; i++)
    //    {
    //        showPlayersUI.newItemUnlockCommon.transform.GetChild(0).GetChild(i).gameObject.SetActive(false);
    //    }
    //    showPlayersUI.newItemUnlockCommon.transform.GetChild(0).GetChild(ran).gameObject.SetActive(true);
    //}


    public void OnVideoButtonPress()
    {
        //Plugins.instance.isGetitReward = true;
        //Plugins.instance.ShowRewardedAd();
         VideoSuccess();
    }
    public void VideoSuccess()
    {
        videoButton.SetActive(false);
        claimButton.SetActive(true);
    }

    public void OnClaimButtonPress()
    {
       // PlayerPrefs.SetInt("EpicUnLocked" + glassFilling.randomSelectedGlass, glassFilling.randomSelectedGlass);
        //showPlayersUI.SelectedCharacter("epic", glassFilling.randomSelectedGlass);
    }
    public void OnNothanksButtonPress()
    {
    
    }

}
