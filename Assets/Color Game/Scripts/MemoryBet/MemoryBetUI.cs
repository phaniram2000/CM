using UnityEngine;
using TMPro;
using UniversalScripts;

public class MemoryBetUI : UGS
{
    public static MemoryBetUI instance;
    public bool bombBlast;
    public int totalDollars {
        get => totalMoney;
    }

    public TMP_Text totalDollarsTxt;

    public WinUI winUI;
    public LooseUI looseUI;
    public GameObject betUI;
    public MBGameplayUI gameplayUI;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        totalDollarsTxt.text = (totalDollars).ToString();
    }

    public void ActivateBetUI()
    {
        betUI.SetActive(true);
        gameplayUI.gameObject.SetActive(false);
    }

    public void ActivateGameplayUI()
    {
        betUI.SetActive(false);
        gameplayUI.gameObject.SetActive(true);
    }

    public void ActivateLooseUI()
    {
        //winUI.gameObject.SetActive(false);
        //looseUI.gameObject.SetActive(true);
		GameEvents.InvokeGameLose(-1);
        gameplayUI.gameObject.SetActive(false);
    }

    public void ActivateWinUI()
    {
        //winUI.gameObject.SetActive(true);
        //gameplayUI.gameObject.SetActive(false);
		//GameEvents.InvokeGameWin();
       // looseUI.gameObject.SetActive(false);
    }

}
