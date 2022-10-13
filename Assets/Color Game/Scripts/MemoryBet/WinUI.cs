using UnityEngine;
using UnityEngine.UI;
using UniversalScripts;

public class WinUI : MonoBehaviour
{
    public float rankYPos;
    public Button continueBtn, gameModeBtn;
    void Start()
    {
        int num2;
        var num = Random.Range(4, 8);

        if (num <= 4)
            num2 = 0;
        else if(num > 4 && num <= 6)
            num2 = 1;
        else
            num2 = 2;

        //if (ISManager.instance)
        //    ISManager.instance.Invoke("ShowInterstitialAds", 1);

        Invoke("AfteraSec", 1.2f);

        UGS.AddMoneyToTotalMoney(true, 10);
    }

    void AfteraSec()
    {
        gameModeBtn.gameObject.SetActive(true);
        continueBtn.gameObject.SetActive(true);
    }

    public void Continue()
    {
        /*if (GAScript.Instance)
            GAScript.Instance.EventTracking_MemoryBet(GameAnalyticsSDK.GAProgressionStatus.Complete, "MemoryBet", MBGameplayUI.levelNum.ToString(), MBGamePlayManager.instance.tilesCount.ToString());*/
        
        GameEssentials.instance.sl.LoadSameScene();
    }

    public void GameMode()
    {
        /*if (GAScript.Instance)
            GAScript.Instance.EventTracking_MemoryBet(GameAnalyticsSDK.GAProgressionStatus.Complete, "MemoryBet", MBGameplayUI.levelNum.ToString(), MBGamePlayManager.instance.tilesCount.ToString());*/

        GameEssentials.gameModeVal = 1;
        GameEssentials.instance.sl.LoadSceneByInt(0);
    }
}
