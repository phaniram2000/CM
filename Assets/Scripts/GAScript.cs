using System.Collections.Generic;
using UnityEngine;
//using GameAnalyticsSDK;
using Tabtale.TTPlugins;

public class GAScript : MonoBehaviour
{
    // Start is called before the first frame update
    public static GAScript Instance;
    public static string MissionName = "StoryScene";

    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            DestroyImmediate(gameObject);
        }


        TTPCore.Setup();
        //   GameAnalytics.Initialize();
    }

    private void Start()
    {
        GameObject.Find("EventSystem").SetActive(false);
    }

    public void LevelStart(string levelName)
    {
        print("LevelStart: " + MissionName + "::" + levelName + "::" + int.Parse(levelName));
        var lvl = int.Parse(levelName);
        var parameters = new Dictionary<string, object> { { "missionName", levelName } };
        TTPGameProgression.FirebaseEvents.MissionStarted(lvl, parameters);
        //FaceBookScript.instance.LevelStarted(levelName);
        //   GameAnalytics.NewProgressionEvent(GAProgressionStatus.Start, levelName);
    }

    public void LevelFail(string levelName)
    {
        print("LevelFail: " + MissionName + "::" + levelName);
        var parameters = new Dictionary<string, object> { { "missionName", levelName } };
        TTPGameProgression.FirebaseEvents.MissionFailed(parameters);
        //GameAnalytics.NewProgressionEvent(GAProgressionStatus.Fail, levelName);
        // FaceBookScript.instance.LevelFailed(levelName);
    }

    public void LevelCompleted(string levelName)
    {
        print("LevelComplete: " + MissionName + "::" + levelName);
        var parameters = new Dictionary<string, object> { { "missionName", levelName } };
        TTPGameProgression.FirebaseEvents.MissionComplete(parameters);
        LevelUp((int.Parse(levelName) + 1.ToString()));
        //GameAnalytics.NewProgressionEvent(GAProgressionStatus.Complete, levelName);
        // FaceBookScript.instance.LevelCompleted(levelName);
    }

    public void LevelUp(string levelName)
    {
        var parameters = new Dictionary<string, object> { { "LevelUp", levelName } };
        var lvl = int.Parse(levelName);
        TTPGameProgression.FirebaseEvents.LevelUp(lvl, parameters);
    }
}