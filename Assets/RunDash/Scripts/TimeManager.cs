using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public float slowdownFactor;
  //  public float slowdownLength = 2f;

    public void DoSlowMotion()
    {
        Time.timeScale = slowdownFactor * Time.fixedDeltaTime;
    }

    public void ResetSlowMotion()
    {
        Time.timeScale = 1f;
    }
}
