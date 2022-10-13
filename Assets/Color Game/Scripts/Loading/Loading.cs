using TMPro;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class Loading : MonoBehaviour
{
    public Image title, blackImg, fillImg;
    public TMP_Text loadingTxt;

    public int sceneIndex;
    public string sceneName;

    int baseIndex;
    float progression;
    GameEssentials gameEssentials;

    //float timeToLoad;
    //bool now;
    private void Awake()
    {
       
        gameEssentials = GameEssentials.instance;

        /*DOTweenTMPAnimator animator = new DOTweenTMPAnimator(loadingTxt);
        Tween tween = animator.DOFadeChar(7, 0, 0.5f).SetEase(Ease.OutBack).SetLoops(-1,LoopType.Yoyo);
        Tween tween1 = animator.DOFadeChar(8, 0, 0.7f).SetEase(Ease.OutBack).SetLoops(-1, LoopType.Yoyo);
        Tween tween2 = animator.DOFadeChar(9, 0, 0.9f).SetEase(Ease.OutBack).SetLoops(-1, LoopType.Yoyo);
        tween.Play();
        tween1.Play();
        tween2.Play();*/

        blackImg.DOFade(0, 0.1f).SetEase(Ease.Flash);
    }

    private void Start()
    {
        fillImg.DOFillAmount(0.2f, 0.1f);
        Invoke(nameof(BasedPlayerLevelType),1f);
    }
    void BasedPlayerLevelType()
    {
        // 1 - Color Game, 7 - Color Run, 8 - Memory Bet, 9 - Balloon Pop Race, 10 - Draw One Line, 11 - Stack swipe, 12 - Doge bonk, 13 - Emoji Game

        (int, int) val = GameEssentials.sceneVal switch
        {
            1 => (2, 0),            
            7 => (2, gameEssentials.sd.GetCRLevelNumber() > 5 ? Random.Range(1,6): gameEssentials.sd.GetCRLevelNumber()),
            8 => (7, gameEssentials.sd.GetMBLevelNumber()),
            9 => (8, gameEssentials.sd.GetBPRLevelNumber()),
            10 => (19, gameEssentials.sd.GetDOLLevelNumber()),
            11 => (69, gameEssentials.sd.GetSSLevelNumber()),
            12 => (71, 0),
            13 => (72, 0),
            _ => (2, 0)
        };

        baseIndex = val.Item1;
        sceneIndex = val.Item2;

        sceneIndex += baseIndex;
        GameEssentials.sceneVal = -1;

       // blackImg.DOFade(255f, 0.1f).SetEase(Ease.Flash);

        LoadScene();
    }

    void LoadScene()
    {
        if (string.IsNullOrEmpty(sceneName)) 
        {
            //gameEssentials.sl.LoadSceneAsynByIndex(sceneIndex, out progression);
            StartCoroutine(ILoadAsyncSceneByIndex(sceneIndex));
        }
        else 
        {
            //gameEssentials.sl.LoadSceneAsynByName(sceneName,out progression);
        }
    }

    public IEnumerator ILoadAsyncSceneByIndex(int SceneIndex)
    {
        yield return null;
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(SceneIndex);

        asyncOperation.allowSceneActivation = false;

        while (!asyncOperation.isDone)
        {
            fillImg.DOFillAmount(asyncOperation.progress, 0.1f); 
            if (asyncOperation.progress >= 0.9f)
            {
                fillImg.fillAmount = 1f;
                asyncOperation.allowSceneActivation = true;
            }

            yield return null;
        }
    }

}
