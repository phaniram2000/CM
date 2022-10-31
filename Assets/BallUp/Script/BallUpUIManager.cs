using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using Random = UnityEngine.Random;

public class BallUpUIManager : MonoBehaviour
{
    public static BallUpUIManager instance;

    public TextMeshProUGUI level;

    public GameObject intro;
    public GameObject WinPanel, LosePanel;
    public GameObject levelNum;
    public float AiKnifeOffsetx,AiKnifeOffsetz;

    private void Awake()
    {
        instance = this;
    }


    private void Start()
    {
        int currentLevel = PlayerPrefs.GetInt("levelnumber", 1);
        level.SetText("Level " + currentLevel);
        Vibration.Init();
        // Application.targetFrameRate = 60;
        // QualitySettings.vSyncCount = 0;
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
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (intro)
            {
                intro.SetActive(false);
            }
        }

        Time.timeScale = Input.GetKey(KeyCode.LeftAlt) ? 5 : 1;
    }
}