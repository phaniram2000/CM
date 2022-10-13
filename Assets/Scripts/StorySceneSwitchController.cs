using UnityEngine;

public class StorySceneSwitchController : MonoBehaviour
{
    [SerializeField] private GameObject interviewSceneGameobject, storySceneGameObject;

    private static readonly string InterviewSceneShowedId = "interviewSceneShowed";

    private void Awake()
    {
        int interviewSceneShowed = PlayerPrefs.GetInt(InterviewSceneShowedId);
        
        
        if (interviewSceneShowed == 0)
        {
            interviewSceneGameobject.SetActive(true);
            storySceneGameObject.SetActive(false);
            
            /*PlayerPrefs.SetInt(InterviewSceneShowedId,1);*/
        }
        else if (interviewSceneShowed == 1)
        {
            interviewSceneGameobject.SetActive(false);
            storySceneGameObject.SetActive(true);
        }
        
    }
}
