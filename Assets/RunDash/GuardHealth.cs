using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GuardHealth : MonoBehaviour
{
    public int RagdollKickForce;
    public Animator EnemyAnim;
    private string currentAnimaton;
    public List<Rigidbody> EnemyRb;
    //public TimeManager timeManager;
    public static GuardHealth instance;
    Collider m_collider;
    public float slowdownFactor;
    public GameObject ParticleEffect;
    public GameObject Enemy;
    public int maxHealth;
    public int enemyhealth;
    public TextMeshProUGUI HealthText;
  //  public ParticleSystem dummyPlayerParticle;
    public GameObject disableCanvas;
    //  public Rigidbody[] EnemySpine;

    //public GameObject particleEffect;

    //-------------Animation States---------------------//
    const string ENEMY_BOXING = "Boxing";
    private void Awake()
    {
        instance = this;
    }
    void ChangeAnimationStateEnemy(string newAnimation)
    {
        if (currentAnimaton == newAnimation) return;

        EnemyAnim.Play(newAnimation);
        currentAnimaton = newAnimation;
    }
    void Start()
    {
        m_collider = GetComponent<Collider>();
        enemyhealth = maxHealth;
    }

    void Update()
    {

    }
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            takedamage(1);
            enemyhealth--;
            HealthText.text = enemyhealth.ToString();
            Time.timeScale = slowdownFactor * Time.deltaTime;
            ParticleEffect.SetActive(true);

            // CrowdController.instance.dummyplayers_();
        }
    }

    IEnumerator DisableEnemy()
    {
        yield return new WaitForSeconds(2f);
        Enemy.SetActive(false);
    }
    public void takedamage(int Damage_amount)
    {
        enemyhealth -= Damage_amount;
        // Debug.Log(enemyhealth);
        if (enemyhealth <= 0)
        {
          //  dummyPlayerParticle.Play(true);
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            m_collider.enabled = false;
            //transform.GetComponent<CapsuleCollider>().isTrigger = true;
            for (int i = 0; i < EnemyRb.Count; i++)
            {
                EnemyRb[i].AddForce(Vector3.forward * RagdollKickForce, ForceMode.Force);
                // transform.GetComponent<CapsuleCollider>().isTrigger = true;
            }
            EnemyAnim.enabled = false;
            disableCanvas.SetActive(false);
            Time.timeScale = 100 * Time.deltaTime;
            StartCoroutine(DisableEnemy());
        }
    }
}
