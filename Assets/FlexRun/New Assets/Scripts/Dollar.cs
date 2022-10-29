using UnityEngine;

public class Dollar : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if(AudioManager.instance)AudioManager.instance.Play("Button");
            gameObject.SetActive(false);
        }
    }
}
