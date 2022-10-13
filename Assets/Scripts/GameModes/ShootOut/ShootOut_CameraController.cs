using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class ShootOut_CameraController : SingletonInstance<ShootOut_CameraController>
{
    public CinemachineVirtualCamera followCam;

    public void Follow(Transform target) => followCam.m_Follow = target;
}
