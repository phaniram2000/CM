using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shark : MonoBehaviour
{
    public GameObject Player;
    public GameObject MainPlayer;
    public GameObject DummyPlayer;
    public GameObject Camera;
    public ParticleSystem ObstacleParticle;
    public List<GameObject> WarningOn;
    public GameObject EnemyBot;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            Time.timeScale = 30 * Time.deltaTime;
            StartCoroutine(DelayCatch());
        }
        

        IEnumerator DelayCatch()
        {
            yield return new WaitForSeconds(0.05f);
            MainPlayer.SetActive(false);
            DummyPlayer.SetActive(false);
            Camera.SetActive(true);
            Player.SetActive(true);
            uimanagr.instance.lost_panel();
            ObstacleParticle.Play(true);
            for(int i = 0; i < WarningOn.Count;i++)
            {
            WarningOn[i].SetActive(false);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("WarningGate"))
        {
            for (int i = 0; i < WarningOn.Count; i++)
            {
                WarningOn[i].SetActive(true);
            }
        }
        if (other.gameObject.CompareTag("Enemy"))
        {
            StartCoroutine(EnemyCatch());
        }

        IEnumerator EnemyCatch()
        {
            yield return new WaitForSeconds(0f);
            EnemyBot.SetActive(false);
        }
    }
}
