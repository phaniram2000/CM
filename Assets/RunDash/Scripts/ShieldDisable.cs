using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldDisable : MonoBehaviour
{
    public GameObject _ShieldDisable;
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
            _ShieldDisable.SetActive(false);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            _ShieldDisable.SetActive(false);
        }
    }
}
