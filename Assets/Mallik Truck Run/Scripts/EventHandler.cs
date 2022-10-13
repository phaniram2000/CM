using UnityEngine;
using System;

public class EventHandler : MonoBehaviour
{
    public static EventHandler instance;
    public event Action GameOverEvent;
    public event Action FinishReachEvent;

    public void Awake()
    {
        instance = this;
    }
    public void GameOver()
    {
        if(GameOverEvent != null)
        {
            GameOverEvent();
        }
    }

    public void FinishReached()
    {
        if (FinishReachEvent != null)
        {
            FinishReachEvent();
        }
    }
}
