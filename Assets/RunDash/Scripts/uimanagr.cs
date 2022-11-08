using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;

public class uimanagr : MonoBehaviour
{
    public static uimanagr instance;
    public GameObject lostpanel;
    public GameObject winpannel;
    public int levelNo, levelShowNo = 1;
    public TextMeshProUGUI leveltext;
    public bool awake;
    public bool start;
    //public int coinsCount;
    public TextMeshProUGUI coinsTextWin;
    public TextMeshProUGUI coinsTextLose;
    public static int attempNum, previousLevel;
    public int failNumber;
    // public bool Purchased;
    // public List<GameObject> Price;
    // public GameObject Enable;
    public List<GameObject> Warning;
    public GameObject FailButton;
    void Awake()
    {
        instance = this;
        if (awake == true)
        {
            Player.instance.coinsCount = PlayerPrefs.GetInt("Coins");
        }
        if(levelShowNo == previousLevel)
        {
            attempNum++;
        }
        else
        {
            attempNum = 0;
            previousLevel = levelShowNo;
        }
    }
    private void Start()
    {
        levelNo = PlayerPrefs.GetInt("levelno", 1);
        levelShowNo = PlayerPrefs.GetInt("levelshow", 1);
      //  leveltext.text = "LEVEL " + levelShowNo;
        failNumber = PlayerPrefs.GetInt("FailNumber",0);
        if (start == true)
        {
            Player.instance.coinsCount = PlayerPrefs.GetInt("Coins");
        }
        //  SetPlayerData();
        // SetHeroData();
        // print(GetHeroData());
        //if (PlayerPrefs.GetInt("Purchased", 0) <= 0)
        //PlayerPrefs.SetInt("Purchased", PlayerPrefs.GetInt("Purchased", 0));
        for (int i = 0; i < Warning.Count; i++)
        {
         Warning[i].transform.DOScale(25,1f).SetEase(Ease.Linear).SetLoops(-1,LoopType.Yoyo);
        }

    }
    private void Update()
    {
        //if (PlayerPrefs.GetInt("Purchased", 0) > 0)
        //{
        //    //for (int i = 0; i < Price.Count; i++)
        //    //{
        //    //    Price[i].SetActive(false);
        //    //    Enable.SetActive(true);
        //    //}
        //}
//        coinsTextWin.text = Player.instance.coinsCount.ToString();
       // coinsTextLose.text = Player.instance.coinsCount.ToString();
    }
    public void lost_panel()
    {
       // StartCoroutine(delaypannel(lostpanel,1));
        GameEvents.InvokeGameLose(-1);
        // Player.instance.isControl = false;
    }
    public void win_panel()
    {
        //StartCoroutine(delaypannel(winpannel,0));
        GameEvents.InvokeGameWin();
    }
    IEnumerator delaypannel(GameObject panel,int value)
    {
        yield return new WaitForSeconds(1);
        panel.gameObject.SetActive(true);
        if (value == 1)
        {
         
        }
        else
        {
           
        }
        failNumber++;
        PlayerPrefs.SetInt("FailNumber", failNumber);
        if (failNumber >= 3)
        {
            FailButton.SetActive(true);
        }
    }
    public void restart()
    {
        Vibration.VibratePop();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void SkipButton()
    {
        Vibration.VibratePop();
        next();
        PlayerPrefs.SetInt("FailNumber", 0);
       
    }
    public void next()
    {
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        Vibration.VibratePop();
        levelShowNo++;
        if (levelShowNo > 45)
        {
            levelNo = Random.Range(22, 45);
        }
        else
        {
            levelNo++;
        }
        PlayerPrefs.SetInt("levelshow", levelShowNo);
        PlayerPrefs.SetInt("levelno", levelNo);
        PlayerPrefs.SetInt("Coins", Player.instance.coinsCount);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
      
    }
    public void nextLast()
    {
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        Vibration.VibratePop();
        levelShowNo++;
        if (levelShowNo > 45)
        {
            levelNo = Random.Range(22, 45);
        }
        else
        {
            levelNo++;
        }
        PlayerPrefs.SetInt("levelshow", levelShowNo);
        PlayerPrefs.SetInt("levelno", levelNo);
        PlayerPrefs.SetInt("Coins", Player.instance.coinsCount);
        SceneManager.LoadScene(levelNo);
    }
    public void OnSpecialCharacter()
    {
        if (Player.instance.coinsCount >= 200 /*&& Purchased == false*/)
        {
           // int rnd = Random.Range(20);
            SceneManager.LoadScene(26);
            //levelNo = 20;
            levelNo++;
            //PlayerPrefs.SetInt("HeroScenes", PlayerPrefs.GetInt("HeroScenes", 20)+1);
            //Debug.Log(levelNo);
            Player.instance.coinsCount -= 200;
            PlayerPrefs.SetInt("Coins", Player.instance.coinsCount);
            PlayerPrefs.SetInt("levelshow", levelShowNo);
            PlayerPrefs.SetInt("levelno", levelNo);
            Vibration.VibratePop();
            //  PlayerPrefs.SetInt("Purchased", PlayerPrefs.GetInt("Purchased", 0) +1);
            //  Purchased = true;
        }
        //else if (Purchased)
        //{
        //    SceneManager.LoadScene(23);
        //    //PlayerPrefs.SetInt("HeroScenes", PlayerPrefs.GetInt("HeroScenes", 20) + 1);
        //    //levelNo = 20;
        //    levelNo++;
        //    Debug.Log(levelNo);
        //    Player.instance.coinsCount -= 0;
        //}
    }
    public void OnOriginalCharacter()
    {
        SceneManager.LoadScene(4);
        //PlayerPrefs.SetInt("PlayerScenes", PlayerPrefs.GetInt("PlayerScenes", 1)+1);
        //levelNo = 20;
        levelNo++;
        Vibration.VibratePop();
        //Debug.Log(levelNo);
    }
   


    //public void OnRebackSpecialCharacter()
    //{
    //    SceneManager.LoadScene(20);
    //    levelNo = 20;
    //    levelNo++;
    //    Debug.Log(levelNo);
    //}
    //public void SetPlayerData()
    //{
    //    if(PlayerPrefs.GetInt("PlayerScenes", 0) <= 0)
    //    {
    //        PlayerPrefs.SetInt("PlayerScenes", PlayerPrefs.GetInt("PlayerScenes", 1));
    //    }
    //}
    //public int GetPlayerData()
    //{
    //    return PlayerPrefs.GetInt("PlayerScenes", 1);
    //}
    //public void SetHeroData()
    //{
    //    if (PlayerPrefs.GetInt("HeroScenes", 20) <= 20)
    //    {
    //        PlayerPrefs.SetInt("HeroScenes", PlayerPrefs.GetInt("HeroScenes", 20));
    //    }
    //}
    //public int GetHeroData()
    //{
    //    return PlayerPrefs.GetInt("HeroScenes", 20);
    //}
}
