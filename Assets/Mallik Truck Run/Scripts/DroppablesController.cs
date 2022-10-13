using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DroppablesController : MonoBehaviour
{
    public static DroppablesController instance;
    public List<GameObject> droppables;
    public GameObject woodToDrop;
    int inc;

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        inc = 0;
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("collectable"))
        {
            if (inc < droppables.Count)
            {
                //print("to collec index= " + inc);
                GameObject collectedItem = col.gameObject;
                collectedItem.tag = "Untagged";
                collectedItem.SetActive(false);
                droppables[inc].SetActive(true);
                droppables[inc].transform.DOScale(droppables[inc].transform.localScale / 4, 0.2f).From();
                inc++;
            }
            if (col.gameObject.GetComponent<PlaySoundBool>())
            {
                AudioManager.instance.Play("WoodCollect");
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "bumper")
        {
            if (inc > 0)
            {
                //print("to drop index= " + (inc - 1));
                collision.gameObject.tag = "Untagged";
                GameObject droppableItem = droppables[inc - 1];

                GameObject droppedItem = Instantiate(woodToDrop, droppableItem.transform.position, droppableItem.transform.rotation);
                float forceAmount = droppableItem.transform.position.y - droppables[0].transform.position.y;
                StartCoroutine(DropNear(forceAmount, droppedItem.GetComponent<Rigidbody>()));
                droppableItem.SetActive(false);
                inc--;
                //print("decremented = " + inc);
                //Time.timeScale = 0.3f;
            }
        }
    }

    IEnumerator SlowMo()
    {
        Time.timeScale = 0.2f;
        yield return new WaitForSeconds(0.6f);
        Time.timeScale = 1;
    }
    IEnumerator DropNear(float forceAmount, Rigidbody rb)
    {
        yield return new WaitForSeconds(0.2f);
        //rb.AddForce(Vector3.forward * forceAmount * 10, ForceMode.Impulse);
        rb.AddForce(-Vector3.up * forceAmount * 5, ForceMode.Impulse);
    }
}
