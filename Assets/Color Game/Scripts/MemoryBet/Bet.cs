using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class Bet : MonoBehaviour
{
    public Slider slider;
    public TMP_Text amountTxt, minAmountTxt, maxAmountTxt;
    public int maxMoneyCount, currentCount, minMoney, maxMoney;
    public GameObject moneyObj;
    public List<GameObject> moneyQueue;

    int count = 0;
    int amount = 0;
    int oldCount = 0;
    int totalDollars { get { return MemoryBetUI.instance.totalDollars; } }

    private void Start()
    {
        BetMoneySetter();
    }

    public void BetMoneySetter()
    {
        
        int val = 0;
        if (totalDollars < 10)
        {
            val = 10;
            GameEssentials.instance.sd.SetTotalMoney(10);
        }
        else if (totalDollars > 1000)
        {
            val = 1000;
        }
        else
            val = totalDollars;

        maxMoney = val;

        minAmountTxt.text = Extensions.ScoreShow(minMoney);
        maxAmountTxt.text = Extensions.ScoreShow(maxMoney);
    }

    void LateUpdate()
    {
        count = (int)Extensions.Remap(slider.value, 0, 1, 1, maxMoneyCount);
        amount = (int)Extensions.Remap(count, 1, maxMoneyCount, minMoney, maxMoney);
        amountTxt.text = Extensions.ScoreShow(amount);
        MemoryBet.betAmount = amount;

        currentCount = count;

        if (count == oldCount || count == 1)
            return;
        
        var val = 0f;
        if (oldCount < count)
        {
            
            for (int i = 0; i < count; i++)
            {
                moneyQueue[i].SetActive(true);
                DOTween.Kill(moneyQueue[i].transform);
                moneyQueue[i].transform.eulerAngles = new Vector3(0, 90, 0);
                moneyQueue[i].transform.localPosition = new Vector3(0, val, 0);
                moneyQueue[i].transform.localScale = new Vector3(0.7f, 0.5f, 0.7f);

                if (val < 0.03f * moneyQueue.Count)
                    val += 0.03f;

                if (i == oldCount)
                {
                    moneyQueue[i].transform.DOScaleZ(0.85f, 0.05f).SetEase(Ease.InOutQuad).SetLoops(2, LoopType.Yoyo);
                }
               
            }
        }
        else 
        {
            float y = 0f;
            float x = 0;
           
            for (int i = oldCount; i > count; i--)
            {
                moneyQueue[i].SetActive(true);
                DOTween.Kill(moneyQueue[i].transform);
                if (i % 2 == 0)
                {
                    y = -50f;
                    x = 3f;
                }
                else 
                {
                    x = -3f;
                    y = 50f;
                }
              
                moneyQueue[i].transform.DORotate(new Vector3(0, y, 0), 0.1f).SetEase(Ease.InOutQuad);
                moneyQueue[i].transform.DOMoveX(x, 0.1f).SetEase(Ease.InOutCirc).OnComplete(()=> 
                {
                    moneyQueue[i].transform.eulerAngles = new Vector3(0, 90, 0);
                    moneyQueue[i].transform.localPosition = new Vector3(0, val, 0);
                    moneyQueue[i].transform.localScale = new Vector3(0.7f, 0.5f, 0.7f);
                });
            }
        }

        oldCount = count;
    }

    public void OnSliderValueChange()
    {
//// slider value change
    }

    public void BetBtnPressed()
    {
        GameEssentials.instance.shm.PlayBetBtnPress();
		MemoryBetGameEvents.InvokeOnBetButtonPressed();
        GameEssentials.instance.sd.SetMBTotalDollars(totalDollars - MemoryBet.betAmount);
        MemoryBetUI.instance.totalDollarsTxt.text = Extensions.ScoreShow(totalDollars);
        MemoryBet.instance.ActivateGameplay();
        MemoryBetUI.instance.ActivateGameplayUI();
		
        Vibration.Vibrate(30);
		
    }
}
