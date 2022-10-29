using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChasingCar : MonoBehaviour
{
    public List<GameObject> allParts;
    bool isCrashed;

    public float minForce;
    public float maxForce;
    public float radius;

    EnemyCarChasingAI enemyCarChasingAI;
    private void Start()
    {
        enemyCarChasingAI = GetComponent<EnemyCarChasingAI>();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "wood")
        {
            StartCoroutine(SlowMo());
            isCrashed = true;
            StartCoroutine(DecreaseSpeed());
            StartCoroutine(Expode());
            //collision.transform.tag = "Untagged";
            transform.GetComponent<Collider>().enabled = false;
            Vibration.Vibrate(17);
            //collision.gameObject.SetActive(false);
        }
        if (collision.transform.tag == "Player")
        {
            //StartCoroutine(DecreaseSpeed());
        }

        if (collision.transform.tag == "Player")
        {
            enemyCarChasingAI.speed = GameManagerTruck.instance.truckSpeed - 5;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("truckPart"))
        { 
            StartCoroutine(SlowMo());
            isCrashed = true;
            StartCoroutine(DecreaseSpeed());
            StartCoroutine(Expode());
            transform.GetComponent<Collider>().enabled = false;
            Vibration.Vibrate(17);
        }
    }

    IEnumerator DecreaseSpeed()
    {
        yield return new WaitForSeconds(2);
        enemyCarChasingAI.speed = enemyCarChasingAI.speed - 1;
        yield return new WaitForSeconds(3);
        enemyCarChasingAI.speed = 0;
    }
    IEnumerator Expode()
    {
        for (int i = 0; i < allParts.Count; i++)
        {
            Rigidbody rb = allParts[i].GetComponent<Rigidbody>();
            rb.isKinematic = false;
            allParts[i].GetComponent<Collider>().enabled = true;
            if (rb != null)
            {
                rb.AddExplosionForce(Random.Range(minForce, maxForce), transform.position, radius);
            }
            allParts[i].transform.parent = null;
        }

        GameObject explodeEffect = Instantiate(EffectsManager.instance.enemyCarBlast).gameObject;
        explodeEffect.transform.position = transform.position;
        StartCoroutine(DeactivateEffect(explodeEffect));
        yield return new WaitForSeconds(1.5f);
        StartCoroutine(StopSloMo());
        this.gameObject.SetActive(false);
    }
    IEnumerator DeactivateEffect(GameObject effect)
    {
        yield return new WaitForSeconds(2);
        effect.SetActive(false);
    }
    IEnumerator SlowMo()
    {
        Time.timeScale = 0.4f;
        yield return new WaitForSeconds(0.1f);
        Time.timeScale = 1;
    }

    IEnumerator StopSloMo()
    {
        yield return new WaitForSeconds(0);
        Time.timeScale = 1;
    }
}
