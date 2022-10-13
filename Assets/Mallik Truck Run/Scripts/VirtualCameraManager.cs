using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class VirtualCameraManager : MonoBehaviour
{
    public static VirtualCameraManager instance;

    public CinemachineVirtualCamera vCamFollower;
    public CinemachineVirtualCamera vCamFinish;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        vCamFollower.Follow = GameManagerTruck.instance.player.transform;
    }

    public void SwitchToFinishCamera()
    {
        //vCamFollower.gameObject.SetActive(false);
        vCamFinish.gameObject.SetActive(true);
    }
}

