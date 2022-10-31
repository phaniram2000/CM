using UnityEngine;

public class WalkEnemy : MonoBehaviour
{

    public float speed;
    public PlayerControl NoControle;
    public Rigidbody WalkFall;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            NoControle.enabled = false;
            NoControle.PlayerGo.enabled = false;
            WalkFall.AddForce(Vector3.forward * .2f, ForceMode.Impulse);
        }
    }

}
