using UnityEngine;

public class HitBox : MonoBehaviour
{
    public GameObject OrginalWall,Text,ParticleEffect;
    public bool isDummy = false;

    private FlexRun_GameManager flexRunGM;
    // Start is called before the first frame update
    void Start()
    {
        Text.SetActive(true);
        OrginalWall.GetComponent<Collider>().enabled = true;
        gameObject.GetComponent<MeshRenderer>().enabled = true;
        flexRunGM = FlexRun_GameManager.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Breakwall()
    {
        Text.SetActive(false);
        gameObject.GetComponent<MeshRenderer>().enabled = false;
        gameObject.GetComponent<Collider>().enabled = false;
        OrginalWall.GetComponent<Collider>().enabled = false;
        OrginalWall.GetComponent<MeshRenderer>().enabled = false;
        Instantiate(ParticleEffect, flexRunGM.hiteffect.transform.position, flexRunGM.hiteffect.transform.rotation);
        if (isDummy == false)
        {
            flexRunGM.ExpValue += 1;
            Vibration.Vibrate(20);
            print(FlexRun_GameManager.Instance.ExpValue);
        }
        if (AudioManager.instance != null)
        {
            AudioManager.instance.Play("Powerup");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            print("PowerUp");
            Breakwall();
        }
    }
}
