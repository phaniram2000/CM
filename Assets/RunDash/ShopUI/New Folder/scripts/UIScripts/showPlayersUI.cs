using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class showPlayersUI : MonoBehaviour
{
    [Header("Player Models")]
    Transform commonModels;

    public static showPlayersUI instance;

    public SkinnedMeshRenderer demoPlayerSkinnedMeshRend;
    public Material greenText, orangeTex, pinkTex, redTex, yellowTex;
    public List<GameObject> demoPlayerItems;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        ChangeSkin(PlayerPrefs.GetInt("CharacterNo"));
        //SelectedCharacter(PlayerPrefs.GetString("selectionType","common"),PlayerPrefs.GetInt("savedCharacter"));
    }
    public void SelectedCharacter(string selectionType, int selected)
    {
        //PlayerPrefs.SetString("selectionType", selectionType);
        //PlayerPrefs.SetInt("savedCharacter", selected);
        //LoadCharacter(PlayerPrefs.GetString("selectionType"), PlayerPrefs.GetInt("savedCharacter"));
        //PlayerPrefs.SetInt("unlockedCharacters", selected);
        //print(PlayerPrefs.GetInt("unlockedCharacters"));
        ChangeSkin(selected);
    }

    public void LoadCharacter(string selectionType, int number)
    {
        DeactiveAll();
        if (selectionType == "common")
        {
            commonModels.GetChild(number).gameObject.SetActive(true);
        }
        else if (selectionType == "rare")
        {

        }
        else if (selectionType == "epic")
        {

        }

    }
    public void ChangeSkin(int buttonIdex)
    {
        //PlayerSkinChange.instance.ChangeSkin(buttonIdex);
        DeactiveAll();
        
        switch (buttonIdex)
        {
            case 0:
                demoPlayerSkinnedMeshRend.material = pinkTex;
                demoPlayerItems[1].SetActive(true);
                break;
            case 1:
                demoPlayerSkinnedMeshRend.material = orangeTex;
                demoPlayerItems[2].SetActive(true);
                break;
            case 2:
                demoPlayerSkinnedMeshRend.material = greenText;
                demoPlayerItems[2].SetActive(true);
                demoPlayerItems[1].SetActive(true);
                break;
            case 3:
                demoPlayerSkinnedMeshRend.material = yellowTex;
                demoPlayerItems[8].SetActive(true);
                break;
            case 4:
                demoPlayerSkinnedMeshRend.material = pinkTex;
                demoPlayerItems[9].SetActive(true);
                break;
            case 5:
                demoPlayerSkinnedMeshRend.material = orangeTex;
                demoPlayerItems[6].SetActive(true);
                demoPlayerItems[1].SetActive(true);
                break;
        }
    }

    public void DeactiveAll()
    {
        for (int i = 0; i < demoPlayerItems.Count; i++)
        {
            demoPlayerItems[i].SetActive(false);
        }
    }
}
