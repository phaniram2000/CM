using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameEssentials : Singleton<GameEssentials>
{
    public static string remoteConfigVal = "0";

    public SavedData sd;
    public SceneLoader sl;
    public SoundHapticManager shm;

    public static int sceneVal = -1;
    public static int retryVal = 0;
    public static int gameModeVal = 0;

    void Start()
    {
        Vibration.Init();
        //GameObject.Find("EventSystem").SetActive(false);
    }

}

    


