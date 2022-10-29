using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks.Sources;
using UnityEngine;
using Random = UnityEngine.Random;

public class MemoryBet : MonoBehaviour
{
    public static MemoryBet instance;

    public static int betAmount;

	public static int TotalAmount;

    public Bet bet;
    public GameObject help;
    public MBGamePlayManager mbGameplay;
    private void Awake()
    {
        instance = this;
    }

	private void OnEnable()
	{
		GameEvents.GameLose += OnGameLose;
		MemoryBetGameEvents.ResetValue += OnResetValue;
	}

	private void OnDisable()
	{
		GameEvents.GameLose -= OnGameLose;
		MemoryBetGameEvents.ResetValue -= OnResetValue;
	}

	
	private void Start()
    {
        betAmount = 0;
		TotalAmount = 0;
	}

    public void ActivateGameplay()
    {
        bet.gameObject.SetActive(false);
        mbGameplay.gameObject.SetActive(true);
        
        if (MBGameplayUI.levelNum == 1)
            StartCoroutine("DeactiveAfterASec");
    }

    public IEnumerator DeactiveAfterASec()
    {
        help.SetActive(true);
        yield return new WaitForSeconds(3);
        help.SetActive(false);
    }
	
	private void OnGameLose(int obj)
	{
		betAmount = 0;
		TotalAmount = 0;
	}
	
	private void OnResetValue()
	{
		betAmount = 0;
		TotalAmount = 0;
	}
    
}
