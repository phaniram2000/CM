using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Dreamteck.Splines;
 
public class FlexRun_GameManager : MonoBehaviour
{
    public SwipeControl SingleSwipeControl;
    public SwipeControlsFortwo TwoSwipeControl;
    public float FeverIncreaseValue;
    public Image FeverBar,FeverBg;
    public ParticleSystem SpeedEffect;
    public float ExpWallSpeed = 10;
    public int ExpValue = 0;
    public int levelNo, levelShowNo = 1;
    public Text leveltext;
    [Header("Basic Level Controls")]
    public static FlexRun_GameManager Instance;
    public bool UsePowerupwall, islasershowended,isplayerDead;
    public GameObject PowerUpwalls,hiteffect,ShotDeadEffect,StarEffect,ExternalEffectPOs,BangEffect,DiamondEffect,LaserDeactivateeffect,BulletDestroyEffect,SmokeSlamEFfect;
    public GameObject ingamePanel,WinPanel, FailPanel;
    public wall[] ExPWall;
    public GameObject Fountain1, Fountain2,BGM;
    bool isExpWallDestroyed = false;
    float v = 1;
    public GameObject TutorialPanel;
    public SwipeControl SC;
    public ParticleSystem[] PerfectEFfect;
    float i=1, k=1;
    public bool isfeverModeStarted,feverwatcher=false;
    public bool isFeverBarFull = false;
    // Start is called before the first frame update
    private void Awake()
    {
        if (!Instance)
            Instance = this;
    }
    void Start()
    {
        BGM.SetActive(true);
        ExpValue = 0;
        Fountain1.SetActive(false);
        Fountain2.SetActive(false);
        PowerUpwalls.SetActive(false);
        isplayerDead = false;
        islasershowended = false;
        ingamePanel.SetActive(true);
        WinPanel.SetActive(false);
        FailPanel.SetActive(false);

        levelNo = PlayerPrefs.GetInt("levelno", 1);
        levelShowNo = PlayerPrefs.GetInt("levelshow", 1);
        leveltext.text = "LEVEL " + levelShowNo;
        for(int i = 0; i < ExPWall.Length; i++)
        {
            ExPWall[i].transform.GetChild(0).GetComponent<SplineFollower>().follow = true;
            ExPWall[i].transform.GetChild(0).GetComponent<SplineFollower>().followSpeed = ExpWallSpeed;
        }
        isExpWallDestroyed = false;
        if (levelShowNo == 1)
        {
            if (TutorialPanel != null)
            {
                TutorialPanel.SetActive(true);
            }
        }
        else if (levelShowNo == 3)
        {
            if (TutorialPanel != null)
            {
                TutorialPanel.SetActive(false);
            }
        }
        for(int i = 0; i < PerfectEFfect.Length; i++)
        {
            PerfectEFfect[i].Stop();
        }
        FeverBar.fillAmount = 0;
        FeverBar.transform.gameObject.SetActive(true);
        SpeedEffect.Stop();
        isFeverBarFull = false;
        feverwatcher = false;
        isfeverModeStarted = false;
        
        //pluginScript.Instance.LevelStart(""+levelShowNo);
    }

    // Update is called once per frame
    void Update()
    {
        if (SC != null)
        {
            if (levelShowNo == 1 && SC.IncreaseValue >=1)
            {
                if (TutorialPanel != null)
                {
                    TutorialPanel.SetActive(false);
                }
            }
            else if (SC.IncreaseValue == 1)
            {
                if (TutorialPanel != null)
                {
                    TutorialPanel.SetActive(true);
                }
            }
            else if (SC.IncreaseValue > 1)
            {
                TutorialPanel.SetActive(false);
            }
        }
      
        if (UsePowerupwall && islasershowended&&!isplayerDead)
        {
            PowerUpwalls.SetActive(true);
        }
        else if (islasershowended&&!isplayerDead)
        {
            //print("GameWin");
            StartCoroutine(WaitWinPanel(0.5f));
        }
        if (isplayerDead)
        {
            //print("GameFail");
            StartCoroutine(WaitFailPanel(0.5f));
        }
        if (islasershowended == true&&isplayerDead==false)
        {
            for(int i = 0; i < ExPWall.Length; i++)
            {
                if(ExPWall[i].isWallDestroyed == true)
                {
                    isExpWallDestroyed = true;
                }
            }
            FeverBar.fillAmount = 0;
            SpeedEffect.Stop();
        }
        if (isplayerDead)
        {
            for (int i = 0; i < ExPWall.Length; i++)
            {
                ExPWall[i].transform.GetChild(0).GetComponent<SplineFollower>().follow = false;
                ExPWall[i].transform.GetChild(0).GetComponent<SplineFollower>().followSpeed = 0;
            }
        }

        if (isFeverBarFull==true)
        {
            isfeverModeStarted = true;
            SpeedEffect.Play();
            FeverBar.fillAmount -= Time.deltaTime * 0.1f;
            if (FeverBar.fillAmount == 0)
            {
                isfeverModeStarted = false;
                isFeverBarFull = false;
                feverwatcher = true;
            }
        }
        else if (isFeverBarFull==false)
        {
            SpeedEffect.Stop();
        }

        if (isExpWallDestroyed)
        {
            for(int i = 0; i < ExPWall.Length; i++)
            {
                ExPWall[i].transform.GetChild(0).GetComponent<SplineFollower>().follow = true;
                ExPWall[i].transform.GetChild(0).GetComponent<SplineFollower>().followSpeed = 0;
            }
            StartCoroutine(WaitWinPanel(0.5f));
        }
        if (ExpValue == 10)
        {
            StartCoroutine(WaitWinPanel(0.5f));
        }

        if (islasershowended&v==1)
        {
            Instantiate(StarEffect, ExternalEffectPOs.transform.position, ExternalEffectPOs.transform.rotation);
            v += 1;     
        }
        if (FeverBar.fillAmount == 0.2f)
        {
            FeverBar.transform.gameObject.SetActive(true);
            FeverBg.transform.gameObject.SetActive(true);
        }
        else if (FeverBar.fillAmount == 1)
        {
            FeverBar.transform.gameObject.SetActive(true);
            isFeverBarFull = true;
        }

    }

    public void Retry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void NextLevel()
    {
        levelShowNo++;
        if (levelShowNo > 12)
        {
            levelNo = Random.Range(4, 12);
        }
        else
        {
            levelNo++;
        }
        PlayerPrefs.SetInt("levelshow", levelShowNo);
        PlayerPrefs.SetInt("levelno", levelNo);
        SceneManager.LoadScene(levelNo);
        print("LevelNO= " + levelNo);
        print("LevelShow= " + levelShowNo);
    }

    IEnumerator WaitWinPanel(float t)
    {
        yield return new WaitForSeconds(t);
        GameCanvas.game.MakeGameResult(0,0);
        //ingamePanel.SetActive(false);
        //WinPanel.SetActive(true);
        Fountain1.SetActive(true);
        Fountain2.SetActive(true);
        BGM.SetActive(false);
        if (TutorialPanel != null)
        {
            TutorialPanel.SetActive(false);
        }
        if (i == 1)
        {
            print("ii");
            //pluginScript.Instance.LevelCompleted("" + levelShowNo);
            i += 1;
        }
    }
    IEnumerator WaitFailPanel(float m)
    {
        yield return new WaitForSeconds(m);
        GameCanvas.game.MakeGameResult(1,1);
        // ingamePanel.SetActive(false);
        // FailPanel.SetActive(true);
        BGM.SetActive(false);
        if (TutorialPanel != null)
        {
            TutorialPanel.SetActive(false);
        }
        if (k == 1)
        {
            print("jj");
            //pluginScript.Instance.LevelFail("" + levelShowNo);
            k += 1;
        }
    }
}
