using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallDown : MonoBehaviour
{
    public List<Rigidbody> rb;
    public List<Collider> ragdoll;
    public Animator PlayerAnim;
    public GameObject Player;
  //  public GameObject footWare;
  
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < rb.Count; i++)
        {
            rb[i].isKinematic = true;
            rb[i].constraints = RigidbodyConstraints.FreezeAll;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Enemy"))
        {
            collision.gameObject.GetComponent<Collider>().enabled = false;
            PlayerAnim.enabled = false;
           // AudioManager.instance.Play("Crash");
            for(int i = 0; i< rb.Count; i++) 
            { 
            Player.transform.parent = null;
            rb[i].GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionX;
            rb[i].GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotationX; 
            rb[i].isKinematic = false;
            rb[i].AddForce(Vector3.forward * .2f, ForceMode.Impulse);
          //  footWare.SetActive(false);
            gameObject.GetComponent<PlayerControl>().MakePackagesFall();
            StartCoroutine(delayLose());
            StartCoroutine(removeRagdoll());
            }
        }
    }
    IEnumerator  delayLose()
    {
        yield return new WaitForSeconds(1.8f);
       // GameManger.instance.LosePanel.SetActive(true);
      GameEvents.InvokeGameLose(-1);
        //AudioManager.instance.Pause("Crash");
    }
    IEnumerator removeRagdoll()
    {
        for (int i = 0; i < ragdoll.Count; i++)
        {
            yield return new WaitForSeconds(0.28f);
            ragdoll[i].enabled = false;
        }
    }
}
