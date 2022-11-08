using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TruckForce : MonoBehaviour
{
    public int truckfoceRight;
    public int truckfoceup;
   // public ParticleSystem truckparticle;
    //public Rigidbody truckrb;
    // Start is called before the first frame update
    void Start()
    {
      
    }

    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("MainTruck"))
        {
          //  truckparticle.Play(true);
            gameObject.GetComponent<Rigidbody>().AddForce(-Vector3.right * truckfoceRight); //1000
            gameObject.GetComponent<Rigidbody>().AddForce(Vector3.up * truckfoceup);//500
        }
    }
}
