using UnityEngine;

public class ShiftRun_GameManager : MonoBehaviour
{
    public static ShiftRun_GameManager instance;
    public float playerspeed=5;
    public float myrot;
    public GameObject PE1, PE2;
    private PLAYER pp;
    //private UIMANAGER UIM;
    private void Awake()
    {
        instance = this;
    }

    
    // Start is called before the first frame update
    void Start()
    {
        pp = PLAYER.instance;
        //UIM = FindObjectOfType<UIMANAGER>();
        PE1.SetActive(false);
        PE2.SetActive(false);
        RenderSettings.fog = true;
        RenderSettings.fogMode = FogMode.Linear;
        RenderSettings.fogColor = Color.white;
        RenderSettings.fogStartDistance = 15f;
        RenderSettings.fogEndDistance = 60f;
       
        

    }
    int i = 0;
    // Update is called once per frame
    void Update()
    {if(pp.transform.position.y<-200 && i == 0)
        {
            //Calling Level Fail 
            //UIM.LF();
            GameCanvas.game.MakeGameResult(1,1);
            i++;
        }
        
    }
   
}
