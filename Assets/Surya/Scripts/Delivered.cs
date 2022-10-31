using System.Collections;
using UnityEngine;

public class Delivered : MonoBehaviour
{
    public Animator WayPoint;
    //public List<GameObject> DeliveredCum;
    public Animator CustomerAnim;
    public int SubBoxes;
   
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    int Counter = 0;
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            WayPoint.SetTrigger("Win");
            StartCoroutine(delayBox());
            CustomerAnim.SetTrigger("Delivired");
        }
    }
    IEnumerator delayBox()
    {
        for (int i = 0; i < 3; i++)
        {
           // DeliveredCum[Counter++].SetActive(true);
            yield return new WaitForSeconds(0.1f);
        }
    }
}
