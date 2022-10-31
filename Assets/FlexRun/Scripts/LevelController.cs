using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelController : MonoBehaviour
{
    int _levelNo = 0;
    void Start()
    {
        _levelNo = PlayerPrefs.GetInt("levelno", 1);
        _levelNo = _levelNo >= SceneManager.sceneCountInBuildSettings - 1
            ? Random.Range(1, SceneManager.sceneCountInBuildSettings - 1)
            : _levelNo;
        SceneManager.LoadScene(_levelNo);
    }

    
}
