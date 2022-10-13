using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class MemoryBet : MonoBehaviour
{
    public static MemoryBet instance;

    public static int betAmount;

    public Bet bet;
    public GameObject help;
    public MBGamePlayManager mbGameplay;
    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        betAmount = 0;
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
    
}
