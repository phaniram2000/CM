using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class RankPanel : MonoBehaviour
{
	[SerializeField] private List<TextMeshProUGUI> nameTexts, rankTexts;
	
	[SerializeField] private List<string> names = new()
	{
		"Elon Musk", "Tim Cook", "Zuckerberg", "Vladmir Putin", "Joe Biden", "Donald Trump",
		"Zelensky", "Matthew", "George", "Joshua", "Jeffrey", "Jacob", "Eric", "Larry",
		"Brandon", "Elon", "Dennis", "Tyler", "Aaron", "Lisha"
	};
	
	[SerializeField] private RectTransform rowParent, playerRankRow, winStart, loseStart;

	private void Start() => SetNames();

	public void WinRanking(int playerRank)
	{
		var rowCount = rowParent.childCount;
		var rowTargetIndex = rowCount / 2 - 1;
		var targetRow = rowParent.GetChild(rowTargetIndex) as RectTransform;
		var destAnchoredPos = targetRow.anchoredPosition;
		
		SetRanks(rowTargetIndex, playerRank, true);
		
		for (var i = rowCount - 2; i >= rowTargetIndex; i--)
		{
			var rect = rowParent.GetChild(i) as RectTransform;
			rect.DOAnchorPosY(rect.anchoredPosition.y - (playerRankRow.sizeDelta.y + 30f), 2f);
		}
		rowParent.GetChild(rowCount - 1).gameObject.SetActive(false);
		
		playerRankRow.anchoredPosition = winStart.anchoredPosition;
		playerRankRow.DOAnchorPosY(destAnchoredPos.y, 2f)
			.OnStart(() => playerRankRow.DOScale(1.15f, 0.25f))
			.OnComplete(() => playerRankRow.DOScale(1f, 0.25f));
	}

	public void LoseRanking(int playerRank)
	{
		var rowCount = rowParent.childCount;
		var rowTargetIndex = rowCount / 2 + 1;
		var targetRow = rowParent.GetChild(rowTargetIndex) as RectTransform;
		var destAnchoredPos = targetRow.anchoredPosition;

		var child = rowParent.GetChild(rowCount - 1) as RectTransform;
		
		SetRanks(rowTargetIndex, playerRank, false);
		
		for (var i = 0; i <= rowTargetIndex; i++) 
		{
			var rect = rowParent.GetChild(i) as RectTransform;
			rect.DOAnchorPosY(rect.anchoredPosition.y + (playerRankRow.sizeDelta.y + 30f), 1f);
		}
		
		var sibling = rowParent.GetChild(rowCount - 2) as RectTransform;
		child.sizeDelta = sibling.sizeDelta; 

		playerRankRow.anchoredPosition = loseStart.anchoredPosition;
		playerRankRow.DOAnchorPosY(destAnchoredPos.y, 2f)
			.OnStart(() => playerRankRow.DOScale(1.15f, 0.25f))
			.OnComplete(() => playerRankRow.DOScale(1f, 0.25f));
	}

	private void SetNames()
	{
		var tempList = names;
		foreach (var nameText in nameTexts)
		{
			var randomIndex = Random.Range(0, tempList.Count);
			nameText.text = tempList[randomIndex];
			tempList.RemoveAt(randomIndex);
		}
	}

	private void SetRanks(int playerIndex, int playerRank, bool didWin)
	{
		for (var i = rankTexts.Count - 1; i >= 0; i--)
		{
			var rank = 0;

			switch (didWin)
			{
				case true:
					rank = playerRank - playerIndex + i;
					if (i >= playerIndex) rank++;
					break;
				case false:
					rank = playerRank - playerIndex + i - 1;
					if (i > playerIndex) rank++;
					break;
			}
			
			rankTexts[i].text = "#" + rank;
		}
	}
}