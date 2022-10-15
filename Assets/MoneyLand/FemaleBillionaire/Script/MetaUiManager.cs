using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class MetaUiManager : MonoBehaviour
{
    public static MetaUiManager instance;
    public TextMeshProUGUI playerMoneyText,playerCapacityText,assistantCountText;
    public GameObject upGradePanel;
    [HideInInspector]
    public int playerSpeed,playerCapacityy,playerCollectingSpeed,assistantCount;
    public float aiSpeed;
    public Transform moneyParent;
    public int moneyPrefabCount;
	private GameManager gm;
    public Button[] buttons;
    public TextMeshProUGUI[] upgradeTexts,levelNumTexts;
    [HideInInspector]
    public int unlockingGrade1, unlockingGrade2, unlockingGrade3, unlockingGrade4, unlockingGrade5;
    public int defaultIncrementVal;
    [HideInInspector]
    public Image img1, img2, img3, img4, img5;
    public Color lockedColor, unLockedColor;
    private void Awake()
    {
        instance = this;
    }

	private void Start()
    {
        gm = GameManager.instance;
		playerMoneyText.text = MyPlayerPrefsSave.GetTotalMoney() + "$";
        playerSpeed = gm.GetPlayerSpeedData();
        playerCapacityy = gm.GetPlayerCapacityData();
        playerCollectingSpeed = gm.GetPlayerCollectingSpeedData();
        assistantCount = gm.GetAssistantData();
        aiSpeed = gm.GetAiSpeedData();
        moneyPrefabCount = moneyParent.childCount;
        for (int i = 0; i < moneyParent.childCount; i++) moneyParent.GetChild(i).gameObject.SetActive(false);
		CheckPrefs1();
        CheckPrefs2();
        CheckPrefs3();
        CheckPrefs4();
        CheckPrefs5();
        //assitant
        //capcity
        UpdateUpgradeText();
        SetIncrement("_MoveSpeed");
        SetIncrement("_CollectSpeed");
        SetIncrement("_Capaccity");
        SetIncrement("_Assistannt");
        SetIncrement("_AiSpeed");

        img1 = buttons[0].GetComponent<Image>();
        img2= buttons[1].GetComponent<Image>();
        img3= buttons[2].GetComponent<Image>();
        img4= buttons[3].GetComponent<Image>();
        img5= buttons[4].GetComponent<Image>();
        //playerMoneyText.text = (PlayerPrefs.GetFloat("Money", 0) * 20) + "$";
    }

	public void GoToMissions()
	{
		SceneManager.LoadScene(1);
	}
	
    public void UpdateUpgradeText()
    {
        upgradeTexts[0].text = unlockingGrade1 + "$";
        upgradeTexts[1].text = unlockingGrade2 + "$";
        upgradeTexts[2].text = unlockingGrade3 + "$";
        upgradeTexts[3].text = unlockingGrade4 + "$";
        upgradeTexts[4].text = unlockingGrade5 + "$";
    }
    public void CheckPrefs1()
    {
        if (PlayerPrefs.GetInt("_MoveSpeed", 1) <= 1)
        {
            unlockingGrade1 = PlayerPrefs.GetInt("_MoveSpeed", 1) * defaultIncrementVal;
        }
        else
        {
            unlockingGrade1 = PlayerPrefs.GetInt("speed");
        }
    }
    public void CheckPrefs2()
    {
        if (PlayerPrefs.GetInt("collectsspeed", 1) <= 1)
        {
            unlockingGrade2 = PlayerPrefs.GetInt("collectsspeed", 1) * defaultIncrementVal;
        }
        else
        {
            unlockingGrade2 = PlayerPrefs.GetInt("collectsspeed");
        }
    }
    public void CheckPrefs3()
    {
        if (PlayerPrefs.GetInt("capcity", 1) <= 1)
        {
            unlockingGrade3 = PlayerPrefs.GetInt("capcity", 1) * defaultIncrementVal;
        }
        else
        {
            unlockingGrade3 = PlayerPrefs.GetInt("capcity");
        }
    }
    public void CheckPrefs4()
    {
        if (PlayerPrefs.GetInt("assitant", 1) <= 1)
        {
            unlockingGrade4 = PlayerPrefs.GetInt("assitant", 1) * defaultIncrementVal;
        }
        else
        {
            unlockingGrade4 = PlayerPrefs.GetInt("assitant");
        }
    }
    public void CheckPrefs5()
    {
        if (PlayerPrefs.GetInt("aiSpeed", 1) <= 1)
        {
            unlockingGrade5 = PlayerPrefs.GetInt("aiSpeed", 1) * defaultIncrementVal;
        }
        else
        {
            unlockingGrade5 = PlayerPrefs.GetInt("aiSpeed");
        }
    }
    private void Update()
    {
        CheckForBuying(unlockingGrade1, img1);
        CheckForBuying(unlockingGrade2, img2);
        CheckForBuying(unlockingGrade3, img3);
        CheckForBuying(unlockingGrade4, img4);
        CheckForBuying(unlockingGrade5, img5);
        assistantCountText.text = assistantCount.ToString();
    }

	private void CheckForBuying(int number,Image img)
	{
		img.color = MyPlayerPrefsSave.GetTotalMoney() > number ? unLockedColor : lockedColor;
	}
	
    //upgrade with money buttons
    public void UpgradeMoveSpeed()
    {
        if(MyPlayerPrefsSave.GetTotalMoney() > unlockingGrade1)
        {
            var totalMoney = MyPlayerPrefsSave.GetTotalMoney();
            totalMoney -= unlockingGrade1;
			MyPlayerPrefsSave.SetTotalMoney(totalMoney);
			playerMoneyText.text = MyPlayerPrefsSave.GetTotalMoney()  + " $";
            playerSpeed++;
            unlockingGrade1 *= 2;
            upgradeTexts[0].text = unlockingGrade1+"$";
            LevelOfUpgrade(GetIncrement("_MoveSpeed"), levelNumTexts[0]);
            PlayerPrefs.SetInt("speed", unlockingGrade1);
            int incr = PlayerPrefs.GetInt("_MoveSpeed",1);
            incr++;
            PlayerPrefs.SetInt("_MoveSpeed", incr);
            PlayerPrefs.SetInt(gm.playerSpeedData, playerSpeed);
            Upgrade.upGradePanelActivated = false;
        }
    }
    public void Aispeed()
    {
        if (MyPlayerPrefsSave.GetTotalMoney() > unlockingGrade5)
        {
            float totalMoney = MyPlayerPrefsSave.GetTotalMoney();
            totalMoney = totalMoney - unlockingGrade5;
			MyPlayerPrefsSave.SetTotalMoney(totalMoney);
            playerMoneyText.text = MyPlayerPrefsSave.GetTotalMoney() + " $";
            aiSpeed++;
            unlockingGrade5 *= 2;
            upgradeTexts[4].text = unlockingGrade5 + "$";
            LevelOfUpgrade(GetIncrement("_AiSpeed"), levelNumTexts[4]);
            PlayerPrefs.SetInt("aiSpeed", unlockingGrade5);
            int incr = PlayerPrefs.GetInt("_AiSpeed", 1);
            incr++;
            PlayerPrefs.SetInt("_AiSpeed", incr);
            PlayerPrefs.SetFloat(gm.aiSpeedData, aiSpeed);
            Upgrade.upGradePanelActivated = false;
        }
    }
    public void SetIncrement(string sss)
    {
        PlayerPrefs.SetInt(sss, PlayerPrefs.GetInt(sss,1));
    }

    public int GetIncrement(string sss)
    {
        return PlayerPrefs.GetInt(sss,1);
    }
    public void UpgradeCollectSpeed()
    {
        if (MyPlayerPrefsSave.GetTotalMoney() > unlockingGrade2)
        {
            float totalMoney = MyPlayerPrefsSave.GetTotalMoney();
            totalMoney = totalMoney - unlockingGrade2;
			MyPlayerPrefsSave.SetTotalMoney(totalMoney);
            playerMoneyText.text = MyPlayerPrefsSave.GetTotalMoney() + " $";
            playerCollectingSpeed++;
            unlockingGrade2 *= 2;
            upgradeTexts[1].text = unlockingGrade2 + "$";
            LevelOfUpgrade(GetIncrement("_CollectSpeed"), levelNumTexts[1]);
            PlayerPrefs.SetInt("collectsspeed", unlockingGrade2);
            int incr = PlayerPrefs.GetInt("_CollectSpeed", 1);
            incr++;
            PlayerPrefs.SetInt("_CollectSpeed", incr);
            PlayerPrefs.SetInt(gm.playerCollectingSpeedData, playerCollectingSpeed);
            Upgrade.upGradePanelActivated = false;
        }
    }
    public void UpgradeCapacity()
    {
        if (MyPlayerPrefsSave.GetTotalMoney() > unlockingGrade3)
        {
            float totalMoney = MyPlayerPrefsSave.GetTotalMoney();
            totalMoney = totalMoney - unlockingGrade3;
			MyPlayerPrefsSave.SetTotalMoney(totalMoney);
            playerMoneyText.text = MyPlayerPrefsSave.GetTotalMoney() + " $";
            playerCapacityy++;
            unlockingGrade3 *= 2;
            upgradeTexts[2].text = unlockingGrade3 + "$";
            LevelOfUpgrade(GetIncrement("_Capaccity"), levelNumTexts[2]);
            PlayerPrefs.SetInt("capcity", unlockingGrade3);
            int incr = PlayerPrefs.GetInt("_Capaccity", 1);
            incr++;
            PlayerPrefs.SetInt("_Capaccity", incr);
            PlayerPrefs.SetInt(gm.playerCapacityData, playerCapacityy);
            Upgrade.upGradePanelActivated = false;
        }
    }
    public void UpgradeAssistant()
    {
        if (MyPlayerPrefsSave.GetTotalMoney() > unlockingGrade4)
        {
            float totalMoney = MyPlayerPrefsSave.GetTotalMoney();
            totalMoney = totalMoney - unlockingGrade4;
			MyPlayerPrefsSave.SetTotalMoney(totalMoney);
            playerMoneyText.text = MyPlayerPrefsSave.GetTotalMoney() + " $";
            assistantCount++;
            unlockingGrade4 *= 2;
            upgradeTexts[3].text = unlockingGrade4 + "$";
            LevelOfUpgrade(GetIncrement("_Assistannt"), levelNumTexts[3]);
            PlayerPrefs.SetInt("assitant", unlockingGrade4);
            int incr = PlayerPrefs.GetInt("_Assistannt", 1);
            incr++;
            PlayerPrefs.SetInt("_Assistannt", incr);
            PlayerPrefs.SetInt(gm.assistantData, assistantCount);
            Upgrade.upGradePanelActivated = false;
            gm.PlayerRobotSpawner();
        }
    }
    public void UpgradeMoveSpeedWithAd()
    {
        //ShowAd
        playerSpeed++;
        PlayerPrefs.SetInt(gm.playerSpeedData, playerSpeed);
    }
    public void LevelOfUpgrade(int increment,TextMeshProUGUI levelText)
    {
        switch (increment)
        {
            case 1:
                levelText.text = "L "+ 1;
                break;
            case 2:
                levelText.text = "L " + 2;
                break;
            case 3:
                levelText.text = "L " + 3;
                break;
        }
    }
    public void SetPPP(string ss)
    {
        PlayerPrefs.SetString(ss, PlayerPrefs.GetString(ss));
    }
    public string GetPPP(string ss)
    {
        return PlayerPrefs.GetString(ss);
    }
}