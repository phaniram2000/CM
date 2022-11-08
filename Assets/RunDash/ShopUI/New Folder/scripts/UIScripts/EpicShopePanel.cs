using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EpicShopePanel : MonoBehaviour
{
    [Header("Epic LIST")]
    public List<GameObject> epicList = new List<GameObject>();

    private int totalUnlocked;
    private int currentSelected;

    private showPlayersUI showPlayersUI;



   // public GlassFilling glassFilling;
    void Start()
    {
        for (int i = 0; i < epicList.Count; i++)
        {
            if (PlayerPrefs.GetInt("EpicUnLocked" + i, -1) == i)
            {
                epicList[i].transform.GetChild(1).gameObject.SetActive(false);
                epicList[i].GetComponent<Button>().interactable = true;
                totalUnlocked++;
            }
        }
        showPlayersUI = GameObject.FindObjectOfType<showPlayersUI>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnWatchVideoButtonPress(int selected)
    {
        currentSelected = selected;
        //Plugins.instance.isEpicReward = true;
        //Plugins.instance.ShowRewardedAd();
         VideoSuccessFullyWatched();
    }

    public void VideoSuccessFullyWatched()
    {
        string value1 = epicList[currentSelected].transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text;
        string value2 = epicList[currentSelected].transform.GetChild(1).GetChild(1).GetComponent<TextMeshProUGUI>().text;

        int increament = int.Parse(value1);
        increament += 1;

        if (increament == int.Parse(value2))
        {
            UnLocked();
        }
        else
        {

            epicList[currentSelected].transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = increament.ToString();

        }
    }

    public void UnLocked()
    {
        PlayerPrefs.SetInt("EpicUnLocked" + currentSelected, currentSelected);
        epicList[currentSelected].transform.GetChild(1).gameObject.SetActive(false) ;
        epicList[currentSelected].GetComponent<Button>().interactable = true;
        OnSelectPlayerButtonPress(currentSelected);

        //if (currentSelected == PlayerPrefs.GetInt("randomSelectedGlass"))
        //{
        //    glassFilling.Genarate_RandomNewItem();
        //}
        totalUnlocked++;
    }
    private int click;
    private int temp;
    public void OnSelectPlayerButtonPress(int selected)
    {
        //showPlayersUI.SelectedCharacter("epic", selected);
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
