using UnityEngine;

public class HelicopterChaseTrigger : MonoBehaviour
{
    public GameObject helicopter;
    HelicopterMovement helicopterMovement;
    float copterSpeed;
    public float xOffset;

    private void Start()
    {
        helicopterMovement = helicopter.GetComponent<HelicopterMovement>();
        copterSpeed = helicopterMovement.speed;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            helicopterMovement.speed = 0;
            int val = Random.Range(1,2);
            if(xOffset == -8.5f)
            {
                helicopter.transform.localEulerAngles = new Vector3(0, 15, 0);
            }
            if (xOffset == 8.5f)
            {
                helicopter.transform.localEulerAngles = new Vector3(0, -15, 0);
            }
            Vector3 copterPosition = new Vector3(xOffset, 2.35f, transform.position.z + 12);
            helicopter.transform.position = copterPosition;
            helicopter.SetActive(true);
            helicopterMovement.speed = copterSpeed;
        }
    }
}
