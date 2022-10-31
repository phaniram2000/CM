using System.Collections;
using UnityEngine;

public class WoodDeactivator : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.tag == "wood")
        {
            StartCoroutine(WoodUntagger(collision.gameObject));
            Time.timeScale = 1;
        }
    }

   IEnumerator WoodUntagger(GameObject w)
    {
        yield return new WaitForSeconds(2);
        w.tag = "Untagged";
    }
}
