using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public int GetTotalScenesInBuildSettings()
    {
        return SceneManager.sceneCountInBuildSettings;
    }
    public int GetCurrentSceneByBuildIndex()
    {
        return SceneManager.GetActiveScene().buildIndex;
    }
    public string GetCurrentSceneByName()
    {
        return SceneManager.GetActiveScene().name;
    }
    public string GetNextSceneByName()
    {
        return (SceneManager.GetActiveScene().buildIndex + 1).ToString();
    }

    public int GetNextSceneByBuildIndex()
    {
        return SceneManager.GetActiveScene().buildIndex + 1;
    }

    public int GetRandomSceneByBuildIndex()
    {
        return Random.Range(1, GetTotalScenesInBuildSettings() - 1);
    }

    public void LoadScene(string SceneName)
    {
        SceneManager.LoadScene(SceneName);
    } 
    
    public void LoadSceneByInt(int SceneName)
    {
        SceneManager.LoadScene(SceneName);
    }

    public IEnumerator ILoadAsyncSceneByIndex(int SceneIndex)
    {
       
        yield return null;

        
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(SceneIndex);
     
        asyncOperation.allowSceneActivation = false;
        Debug.Log("Pro :" + asyncOperation.progress);
       
        while (!asyncOperation.isDone)
        {

            //  m_Text.text = "Loading progress: " + (asyncOperation.progress * 100) + "%";
            // Check if the load has finished
            if (asyncOperation.progress >= 0.9f)
            {
               asyncOperation.allowSceneActivation = true;
            }

            yield return null;
        }
    }

    public void LoadSceneAsynByIndex(int SceneIndex, out float val)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(SceneIndex);
        val = operation.progress;
    }

    public void LoadSceneAsynByName(string SceneName, out float val)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(SceneName);
        val = operation.progress;
    }
    

    public void LoadSameScene()
    {
        SceneManager.LoadScene(GetCurrentSceneByName());
    }
}
