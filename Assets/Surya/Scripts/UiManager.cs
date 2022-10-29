using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class UiManager : MonoBehaviour
{
    public TextMeshProUGUI level;
    public GameObject intro;
    private void Start()
    {
     
      int currentLevel = PlayerPrefs.GetInt("levelnumber", 1);
        level.text = "level " + currentLevel.ToString();


        //if (GAScript.Instance)
        //{
        //    GAScript.Instance.LevelStart("levelnumber");
        //}
    }

    public void NextlevelButton()
    { 
        //NEXT BUTTON CALL
        if (PlayerPrefs.GetInt("level", 1) >= SceneManager.sceneCountInBuildSettings - 1)
        {
            SceneManager.LoadScene(Random.Range(0, SceneManager.sceneCountInBuildSettings - 1));
            PlayerPrefs.SetInt("level", (PlayerPrefs.GetInt("level", 1) + 1));
        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            PlayerPrefs.SetInt("level", (PlayerPrefs.GetInt("level", 1) + 1));
        }
        PlayerPrefs.SetInt("levelnumber", PlayerPrefs.GetInt("levelnumber", 1) + 1);
    }

    public void RetryButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        //if (ISManager.instance)
        //{
        //    ISManager.instance.ShowInterstitialAds();
        //}
        //if (GAScript.Instance)
        //{
        //    GAScript.Instance.LevelFail("levelnumber");
        //}
    }
    private void Update()
    {
        if(Input.GetMouseButtonUp(0))
        {
            if(intro!=null)
            intro.SetActive(false);
        }
    }

}
