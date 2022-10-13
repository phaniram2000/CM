using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManagerTruck : MonoBehaviour
{
    public static GameManagerTruck instance;
    public float truckSpeed;
    public bool gameOver;
    public bool gameStarted;
    public Image coin;

    public TextMeshProUGUI levelNumText;
    public TextMeshProUGUI coinText;
    public GameObject mainScreen;
    public GameObject help;

    [HideInInspector] public GameObject player;

    int currentLevel;
    private void Awake()
    {
        instance = this;
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void OnEnable()
    {
       
        GameEvents.TapToPlay += taptoplay;
    }

    private void OnDisable()
    {
        GameEvents.TapToPlay -= taptoplay;
    }

    private void taptoplay()
    {
        help.SetActive(false);
        gameStarted = true;
        TruckMovement.instance.speed = truckSpeed;
        KarenBike.instance.canMove = true;
        TruckMovement.instance.speedParticle.SetActive(true);
        //SoundsManager.instance.BGMusicSource.enabled = true;
        KarenCheater.instance.StartCoroutine(KarenCheater.instance.StandOnBike());
    }

    private void Start()
    {
        /*currentLevel = PlayerPrefs.GetInt("levelnumber", 1);
        levelNumText.SetText("Level " + currentLevel.ToString());

        if (currentLevel < 2)
        {
            help.SetActive(true);
        }
        Vibration.Init();*/
    }

    private void Update()
    {
        // if(Input.GetMouseButtonDown(0) && !gameStarted)
        // {
        //     help.SetActive(false);
        //     gameStarted = true;
        //     TruckMovement.instance.speed = truckSpeed;
        //     KarenBike.instance.canMove = true;
        //     TruckMovement.instance.speedParticle.SetActive(true);
        //     SoundsManager.instance.BGMusicSource.enabled = true;
        //     KarenCheater.instance.StartCoroutine(KarenCheater.instance.StandOnBike());
        // }
    }
    /*public IEnumerator LevelComplete()
    {
        if (!gameOver)
        {
            yield return new WaitForSeconds(1f);
            winpanel.SetActive(true); 
            //SoundsManager.instance.PlayOnce(SoundsManager.instance.win);
            TruckMovement.instance.speed = 0;
            gameOver = true;
        }
    }*/
    public IEnumerator LevelFailed(float timeToCall)
    {
        if (!gameOver)
        {
            gameOver = true;
            VirtualCameraManager.instance.vCamFollower.Follow = null;
            //SoundsManager.instance.BGMusicSource.enabled = false;
            TruckMovement.instance.ExplodeTruck();
            yield return new WaitForSeconds(timeToCall);
            TruckMovement.instance.speed = 0;
            GameEvents.InvokeGameLose(-1);
            //SoundsManager.instance.PlayOnce(SoundsManager.instance.fail);
            EventHandler.instance.GameOver();
        }
    }

    public IEnumerator UpdateCoins(float totalCoins)
    {
        yield return new WaitForSeconds(0.8f);
        //int num = (int)totalCoins;
        float timePerCoin = 1 / totalCoins;
        int i = 1;
        while(i <= totalCoins)
        {
            coinText.SetText(i.ToString());
            yield return new WaitForSeconds(timePerCoin);
            i++;
        }
    }
}
