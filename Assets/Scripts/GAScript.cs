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
        Dictionary<string, object> dict = new Dictionary<string, object>();
        var lvl = int.Parse(levelName);
        dict.Add("missionName", levelName );
        TTPGameProgression.FirebaseEvents.MissionStarted(lvl, dict);
    }

    public void LevelFail(string levelName)
    {
        print("LevelFail: " + MissionName + "::" + levelName);
        Dictionary<string, object> dict = new Dictionary<string, object>();
        dict.Add("missionName", levelName);
        TTPGameProgression.FirebaseEvents.MissionFailed(dict);
    }

    public void LevelCompleted(string levelName)
    {
        Dictionary<string, object> parameters = new Dictionary<string, object> ();
        parameters.Add ("missionType", "level");
        TTPGameProgression.FirebaseEvents.MissionComplete (parameters);
        LevelUp(levelName);
    }

    public void LevelUp(string levelName)
    {
        //var parameters = new Dictionary<string, object> { { "LevelUp", levelName } };
        Dictionary<string, object> parameters = new Dictionary<string, object> ();
        parameters.Add ("LevelUp", levelName);
        int lvl = int.Parse(levelName);
        TTPGameProgression.FirebaseEvents.LevelUp(lvl+1, parameters);
    }
}