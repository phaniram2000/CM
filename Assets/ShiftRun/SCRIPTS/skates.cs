using UnityEngine;
using Dreamteck.Splines;

public class skates : MonoBehaviour
{
    private PLAYER pp;
    private RAGDOL rg;
    //private UIMANAGER UIM;
    public bool faster=false;
    public float timer=3.5f;
    public bool skatejump=false;
    bool highjump, normaljump;
    private ShiftRun_GameManager GM;
    private Animator ppanim;
    public bool land=false;
   public bool springjump = false;
    public bool playerkilled = false;
    // Start is called before the first frame update
    void Start()
    {
        GM = ShiftRun_GameManager.instance;
        //UIM = FindObjectOfType<UIMANAGER>();
        pp = PLAYER.instance;
        rg = pp.GetComponent<RAGDOL>();
        ppanim = pp.GetComponent<Animator>();
    }
    private void FixedUpdate()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        if (faster)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                ppanim.SetLayerWeight(2, 0.3f);
                ppanim.SetBool("FASTER", false);
                pp.transform.parent.GetComponent<SplineFollower>().followSpeed = GM.playerspeed;
                faster = false;
            }
        }
    }
    int i = 0;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "OBSTACLES"&& i==0)
        {
            this.GetComponent<MeshCollider>().enabled = false;
            //  Invoke("playerkill", 0.3f);
            playerkilled= true;
            ppanim.enabled = false;
            rg.activeragdol();
            // calling LevelFail
            GameCanvas.game.MakeGameResult(1,1);
            //UIM.LF();
            i++;
        }
        if (other.tag == "SPEED")
        {
            timer = 3.5f;
            faster = true;
            other.transform.GetChild(0).GetComponent<Renderer>().material.color = Color.blue;
            ppanim.SetLayerWeight(2, 0f);
            ppanim.SetBool("FASTER",true);
            pp.transform.parent.GetComponent<SplineFollower>().followSpeed = GM.playerspeed*2;
        }
        if (other.tag == "BASE")
        {
            land = true;
        }
        if (other.tag == "JUMPER")
        {
            pp.jump = true;
            springjump = true;
            other.transform.position = Vector3.Lerp(other.transform.position, new Vector3(other.transform.position.x, other.transform.position.y + 0.4f, other.transform.position.z), 5 * Time.deltaTime);
          //  ppanim.SetLayerWeight(2, 0.8f);
          //  ppanim.SetTrigger("JUMP");
        }
    }
    public bool obstaclepass = false;
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "OBS")
        {
            obstaclepass = true;
            other.GetComponent<MeshRenderer>().enabled = true;
        }
        if (other.tag == "SPEED")
        {
          //  StartCoroutine("oncomplete");
        }
        if (other.tag == "skatejump") {
            pp.jump = true;
            skatejump = true;
        }
        if (other.tag == "BASE")
        {
            land = false;
        }
    }
    public void playerkill()
    {
        pp.transform.parent.GetComponent<SplineFollower>().followSpeed = 0;
     
    }

}
