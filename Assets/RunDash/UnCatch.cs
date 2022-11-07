using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnCatch : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            StartCoroutine(DelayCollision());
        }
        IEnumerator DelayCollision()
        {
            yield return new WaitForSeconds(3f);
            this.GetComponent<Collider>().isTrigger = true;
        }
    }
}
