using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DitzeGames.Effects;

public class MeteriodsSpeed : MonoBehaviour
{
    public float speed;
    public ParticleSystem Metrioties;
    // Start is called before the first frame update
    void Start()
    {
        Metrioties.Play();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.Translate(Vector3.down * speed * Time.deltaTime);
        transform.Translate(Vector3.back * 325 * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("WaterRun"))
        {
            CameraEffects.ShakeOnce(1f, 5f);
       
        }
    }
}
