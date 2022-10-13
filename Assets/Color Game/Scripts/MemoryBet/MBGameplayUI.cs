using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class MBGameplayUI : MonoBehaviour
{
    public bool alreadyReveal;

    public TMP_Text betAmountTxt, levelNumTxt, getBtnTxt;
    public GameObject continuePanel;
    public Button tapToRevealBtn, backBtn;

    public static int levelNum = 1;

	GameEssentials gameEssentials;

    private void Awake()
    {
        gameEssentials = GameEssentials.instance;
    }
    private void Start()
    {
        levelNumTxt.text = "Level "+(levelNum);
        
    }
    public void TapToReveal()
    {
        var GO = EventSystem.current.currentSelectedGameObject.GetComponent<Button>();
        GO.interactable = false;
        MBGamePlayManager.instance.RevealTheTiles();
        gameEssentials.shm.PlayButtonPress();

        /*if (!GAScript.Instance)
            return;

        GAScript.Instance.EventTracking_MemoryBet(GameAnalyticsSDK.GAProgressionStatus.Start, "MemoryBet", levelNum.ToString(), MBGamePlayManager.instance.tilesCount.ToString());*/
    }

    public void ActivateSemiCompletePanel()
    {
        MemoryBet.betAmount *= 2;
        betAmountTxt.text = Extensions.ScoreShow(MemoryBet.betAmount).ToString();
        getBtnTxt.text = betAmountTxt.text;
        continuePanel.SetActive(true);
        tapToRevealBtn.gameObject.SetActive(false);
        MemoryBet.instance.help.SetActive(false);
    }

    public void ContinuesBtnPressed()
    {
        // Reactive Game
        gameEssentials.shm.PlayButtonPress();
        var gameplay = MemoryBet.instance.mbGameplay;
        int[] nums = new int[] { 6, 9, 12, 16 };
        gameplay.tilesCount = gameplay.tilesCount switch
        {
            4 => 6,
            6 => 9,
            9 => 12,
            12 => 16,
            16 => Extensions.GetOneFromArray(nums),
            _ => 4
        };

        /*if (GAScript.Instance)
            GAScript.Instance.EventTracking_MemoryBet(GameAnalyticsSDK.GAProgressionStatus.Complete, "MemoryBet", levelNum.ToString(), MBGamePlayManager.instance.tilesCount.ToString());*/
        levelNum++;
        tapToRevealBtn.interactable = true;
        continuePanel.SetActive(false);
        tapToRevealBtn.gameObject.SetActive(true);
        MemoryBet.instance.mbGameplay.ResetAllTiles();
        MemoryBet.instance.mbGameplay.ActivateGameplay();
        gameEssentials.shm.Vibrate(15);
        levelNumTxt.text = "Level " + levelNum;
       
    }

    public void GetCash()
    {
        // show Win Panel
        levelNum = 1; 
        MemoryBetUI.instance.ActivateWinUI();
        gameEssentials.shm.PlayButtonPress();
        MemoryBetUI.AddMoneyToTotalMoney(true,MemoryBet.betAmount);
        gameEssentials.sd.SetMBTotalDollars(gameEssentials.sd.GetMBTotalDollars() + MemoryBet.betAmount);
        MemoryBetUI.instance.totalDollarsTxt.text = Extensions.ScoreShow(MemoryBetUI.instance.totalDollars);
        gameEssentials.shm.Vibrate(12);
    }

    public void Back()
    {
        gameEssentials.shm.PlayButtonPress();
        gameEssentials.sl.LoadSameScene();
    }
}
