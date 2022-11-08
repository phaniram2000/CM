using UnityEngine;
using DitzeGames.Effects;
public class ExplosiveBarrel : MonoBehaviour
{
    public GameObject Barrel,Explosion;
  //  public float slowdownFactor;
    // private AudioSource source;

    [SerializeField]
    private float range;
    Collider m_collider;
    public static ExplosiveBarrel instance;
    private void Awake()
    {
        Barrel.SetActive(true);
        instance = this;
      //  Explosion.SetActive(false);

        // source = GetComponent<AudioSource>();
    }
    private void Start()
    {
        m_collider = GetComponent<Collider>();
    }
    public void Explode()
    {
        Barrel.SetActive(false);
        Explosion.SetActive(true);
        // source.Play();

        Collider[] enemies = Physics.OverlapSphere(transform.position, range);

        foreach (Collider enemy in enemies)
        {
            if (enemy.GetComponent<EnemyRD>() != null)
            {
                enemy.GetComponent<EnemyRD>().KillEnemy(transform.position);

            }
        }
        this.enabled = false;
    }

    private void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Explode();
            CameraEffects.ShakeOnce(1f,10f);
            //  Time.timeScale = slowdownFactor * Time.deltaTime;
            AudioManager.instance.Play("FinalBossHit");
            Vibration.VibratePop();
            Player.instance.speed = 0;
            Player.instance.jumpForce = 0;
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Explode();
            CameraEffects.ShakeOnce(1f, 10f);
            //  Time.timeScale = slowdownFactor * Time.deltaTime;
            AudioManager.instance.Play("FinalBossHit");
            Vibration.VibratePop();
            //   Player.instance.speed = 0;
            //  Player.instance.jumpForce = 0;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, range);
    }

}
