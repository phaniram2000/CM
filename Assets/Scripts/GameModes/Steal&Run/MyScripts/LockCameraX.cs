using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class LockCameraX : CinemachineExtension
{
    public float m_XPosition = 10;
 
    protected override void PostPipelineStageCallback(
        CinemachineVirtualCameraBase vcam,
        CinemachineCore.Stage stage, ref CameraState state, float deltaTime)
    {
        if (stage != CinemachineCore.Stage.Body) return;
        
        var pos = state.RawPosition;
        pos.x = m_XPosition;
        state.RawPosition = pos;
    }
}
