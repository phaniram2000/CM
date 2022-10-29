using StateMachine;
using UnityEngine;

public class TFTGameController : MonoBehaviour
{
    [SerializeField] private float waitDurationBeforeSceneTransition;
    [SerializeField] private GameObject scene1, scene2;

    public float WaitDurationBeforeSceneTransition => waitDurationBeforeSceneTransition;

    //public float GetWaitDurationBeforeSceneTransition => _get.waitDurationBeforeSceneTransition;
    
    //private static TFTGameController _get;

    private void OnEnable()
    {
        GameEvents.TapToPlay += OnTapToPlay;
        TFTGameEvents.ActivateNextScene += OnActivateNextScene;
    }

    private void OnDisable()
    {
        GameEvents.TapToPlay -= OnTapToPlay;
        TFTGameEvents.ActivateNextScene -= OnActivateNextScene;
    }

    

    private void Awake()
    {
        /*if (!_get) _get = this;
        else Destroy(gameObject);*/
    }


    private void Start()
    {
       ActivateScene1();
       
       Vibration.Init();
        
    }

    private void ActivateScene1()
    {
        scene1.SetActive(true);
        scene2.SetActive(false);
    }

    private void ActivateScene2()
    {
        scene2.SetActive(true);
        scene1.SetActive(false);
    }
    
    
    private void OnActivateNextScene()
    {
        ActivateScene2();
    }
    
    
    private void OnTapToPlay()
    {
        AInputHandler.AssignNewState(InputState.Disabled);
    }
    
    
    
}
