using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MBGamePlayManager : MonoBehaviour
{
    public static MBGamePlayManager instance;

    public int tilesCount = 4;
    public int maxBombCounts = 1;
    public GameObject[] blockTiles;
    public GameObject currentBlockTiles;

    Transform currentBlockTransfrom;
    int val = 0;
    private void Awake()
    {
        instance = this;

        ActivateGameplay();
    }

    public void ActivateGameplay()
    {
        GettingBlockTiles();
        currentBlockTransfrom = currentBlockTiles.transform;
        ActivateBombsOnBlockTiles(currentBlockTiles, maxBombCounts);
    }

    void GettingBlockTiles()
    {
        switch (tilesCount)
        {
            case 4:
				Camera.main.DOFieldOfView(60f, 0.1f).SetEase(Ease.Flash);
                ActivatingBlockTilesBasedOnCount(0);
                maxBombCounts = 1;
				MemoryBetGameEvents.InvokeOnIncreasePlatformSize(4);
                break;
            case 6:
				Camera.main.DOFieldOfView(60f, 0.1f).SetEase(Ease.Flash);
                ActivatingBlockTilesBasedOnCount(1);
                maxBombCounts = Random.Range(2,3);
				MemoryBetGameEvents.InvokeOnIncreasePlatformSize(6);
               // maxBombCounts = 1;
                break;
            case 9:
				Camera.main.DOFieldOfView(60f, 0.1f).SetEase(Ease.Flash);
                ActivatingBlockTilesBasedOnCount(2);
                maxBombCounts = Random.Range(2, 4);
				MemoryBetGameEvents.InvokeOnIncreasePlatformSize(9);
                break;
            case 12:
				Camera.main.DOFieldOfView(65f, 0.1f).SetEase(Ease.Flash);
                ActivatingBlockTilesBasedOnCount(3);
                maxBombCounts = Random.Range(3, 5);
				MemoryBetGameEvents.InvokeOnIncreasePlatformSize(12);
				// maxBombCounts = 1;
                break;
            case 16:
                Camera.main.DOFieldOfView(70f, 0.1f).SetEase(Ease.Flash);
                ActivatingBlockTilesBasedOnCount(4);
                maxBombCounts = Random.Range(4, 6);
				MemoryBetGameEvents.InvokeOnIncreasePlatformSize(16);
                break;
        }
    }

    void ActivatingBlockTilesBasedOnCount(int count)
    {
        for (int i = 0; i < blockTiles.Length; i++)
        {
            if (i == count)
            {
                blockTiles[i].SetActive(true);
                currentBlockTiles = blockTiles[i];
            }
            else
            {
                blockTiles[i].SetActive(false);
            }
        }
    }

    public void RevealTheTiles()
    {
        for (int i = 0; i < currentBlockTransfrom.childCount; i++)
        {
            var temp = currentBlockTransfrom.GetChild(i).GetComponent<Tile>();

            temp.Reveal(0);
        }
    }

    void ActivateBombsOnBlockTiles(GameObject GO, int BombsCount)
    {
        var childCount = GO.transform.childCount;
        var bombCountList = ReturnBombsList(BombsCount, childCount);
        for (int i = 0; i < childCount; i++)
        {
            var temp = GO.transform.GetChild(i).GetComponent<Tile>();
            temp.TurnBalloonOn();
        }

        for (int j = 0; j < bombCountList.Count; j++)
        {
            var temp = GO.transform.GetChild(bombCountList[j]).GetComponent<Tile>();
            temp.TurnBombOn();
        }
    }

    List<int> ReturnBombsList(int ListLength, int NumbersLimit )
    {
        List<int> temp = new List<int>();

        for (int i = 0; i < ListLength; i++)
        {
            int val = Random.Range(0, NumbersLimit);
            while (!temp.Contains(val)) 
            {
                temp.Add(val);
            }
        }

        return temp;
    }

    public void ResetAllTiles()
    {
        val = 0;
        var childCount = currentBlockTransfrom.childCount;

        for (int i = 0; i < childCount; i++)
        {
           currentBlockTransfrom.GetChild(i).GetComponent<Tile>().ResetThisTile();
        }
    }

    public void DeactivateAllTiles()
    {
        var childCount = currentBlockTransfrom.childCount;

        for (int i = 0; i < childCount; i++)
        {
            var temp = currentBlockTransfrom.GetChild(i).GetComponent<Tile>();
            if (temp.isBomb)
            {
                temp.GetComponent<Collider>().enabled = false;
            }
        }
    }

    public void CheckForSemiComplete()
    {
        if (!AllTilesCleared() || val == 1)
            return;

        if (val == 0)
            val = 1;

        DeactivateAllTiles();
        MemoryBetUI.instance.gameplayUI.Invoke("ActivateSemiCompletePanel", 0.5f);
    }

    bool AllTilesCleared()
    {
        var childCount = currentBlockTransfrom.childCount;

        for (int i = 0; i < childCount; i++)
        {
            var temp = currentBlockTransfrom.GetChild(i).GetComponent<Tile>();
            if (!temp.isTapped && !temp.isBomb)
                return false;
        }

        return true;
    }
}
