using Cinemachine;

public class StealNRun_CameraController : SingletonInstance<StealNRun_CameraController>
{
    public CinemachineVirtualCamera stealingCam, followCam, endCam;
    
    public void OnTapToPlay()
    {
        stealingCam.Priority -= 1;
        followCam.Priority += 1;
    }

    public void OnEnd()
    {
        followCam.Priority -= 1;
        endCam.Priority += 1;
    }
}
