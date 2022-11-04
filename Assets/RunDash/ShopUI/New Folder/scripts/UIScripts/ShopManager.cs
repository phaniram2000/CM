using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopManager : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject common;
    public GameObject rare;
    public GameObject epic;
    public GameObject shop;

    public GameObject commonButton;
    public GameObject rareButton;
    public GameObject epicButton;


    public Color commonColor;
    public Color rareColor;
    public Color epicColor;


    public TextMeshProUGUI coinsText;

    public int totalCoins;

    private CommonShopePanel _CommonShopePanel;
    private RareShopePanel _rareShopePanel;

    public void Awake()
    {
        _CommonShopePanel = GetComponent<CommonShopePanel>();
        _rareShopePanel = GetComponent<RareShopePanel>();
        totalCoins = PlayerPrefs.GetInt("totalCoins");
        coinsText.text = totalCoins.ToString();

    }

    void Start()
    {
        OnCommonButtonPress();
       
    }
    private void LateUpdate()
    {
        totalCoins = PlayerPrefs.GetInt("totalCoins");
        coinsText.text = totalCoins.ToString();

        _CommonShopePanel.totalCoins = totalCoins;
        _rareShopePanel.totalCoins = totalCoins;
       
    }

    public void OnCommonButtonPress()
    {
        #region colorChangeOfButtons
        commonButton.GetComponent<Image>().color = Color.white;
        commonButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = commonColor;

        rareButton.GetComponent<Image>().color = rareColor;
        rareButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = Color.white;

        epicButton.GetComponent<Image>().color = epicColor;
        epicButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = Color.white;

        DeactiveAllShopePanels();
        common.SetActive(true);
        #endregion

    }
    public void OnRareButtonPress()
    {

        #region colorChangeOfButtons
        rareButton.GetComponent<Image>().color = Color.white;
        rareButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = rareColor;

        commonButton.GetComponent<Image>().color = commonColor;
        commonButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = Color.white;

        epicButton.GetComponent<Image>().color = epicColor;
        epicButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = Color.white;

        DeactiveAllShopePanels();
        rare.SetActive(true);
        #endregion

    }
    public void OnEpicButtonPress()
    {
        #region colorChangeOfButtons
        epicButton.GetComponent<Image>().color = Color.white;
        epicButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = epicColor;

        commonButton.GetComponent<Image>().color = commonColor;
        commonButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = Color.white;

        rareButton.GetComponent<Image>().color = rareColor;
        rareButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = Color.white;

        DeactiveAllShopePanels();
        epic.SetActive(true);
        #endregion

    }

    public void DeactiveAllShopePanels()
    {
        common.SetActive(false);
        rare.SetActive(false);
        epic.SetActive(false);
    }
}
