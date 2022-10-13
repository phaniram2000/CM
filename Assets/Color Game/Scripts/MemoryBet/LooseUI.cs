using UnityEngine;
using UnityEngine.UI;

public class LooseUI : MonoBehaviour
{
    public float rankYPos;
    public Button retryBtn, gameModeBtn;

    void Start()
    {
        int num2;
        var num = Random.Range(13, 16);

        if (num <= 13)
            num2 = 0;
        else if (num > 13 && num <= 14)
            num2 = 1;
        else
            num2 = 2;

        //if (ISManager.instance)
        //    ISManager.instance.Invoke("ShowInterstitialAds", 1);

        Invoke("AfteraSec", 1.2f);
    }

    void AfteraSec()
    {
        gameModeBtn.gameObject.SetActive(true);
        retryBtn.gameObject.SetActive(true);
    }

    public void Retry()
    {
        // retry
        /*if (GAScript.Instance)
            GAScript.Instance.EventTracking_MemoryBet(GameAnalyticsSDK.GAProgressionStatus.Fail, "MemoryBet", MBGameplayUI.levelNum.ToString(), MBGamePlayManager.instance.tilesCount.ToString());*/

        MBGameplayUI.levelNum = 1;
        GameEssentials.instance.sl.LoadSameScene();
    }

    public void GameMode()
    {
        /*if (GAScript.Instance)
            GAScript.Instance.EventTracking_MemoryBet(GameAnalyticsSDK.GAProgressionStatus.Fail, "MemoryBet", MBGameplayUI.levelNum.ToString(), MBGamePlayManager.instance.tilesCount.ToString());*/

        GameEssentials.gameModeVal = 1;
        GameEssentials.instance.sl.LoadSceneByInt(0);
    }
}
