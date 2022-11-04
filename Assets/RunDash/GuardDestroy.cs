using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GuardDestroy : MonoBehaviour
{
    public int RagdollKickForce;
    public Animator EnemyAnim;
    private string currentAnimaton;
    public Rigidbody EnemyRb;
    //public TimeManager timeManager;
    public static GuardDestroy instance;
    Collider m_collider;
    public float slowdownFactor;
    public GameObject ParticleEffect;
    public GameObject Enemy;
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
    }

    void Update()
    {

    }
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            m_collider.enabled = false;
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            EnemyAnim.enabled = false;
            EnemyRb.AddForce(Vector3.forward * 150000, ForceMode.Force);
            // transform.GetComponent<CapsuleCollider>().isTrigger = true;
            Time.timeScale = slowdownFactor * Time.deltaTime;
            ParticleEffect.SetActive(true);
            StartCoroutine(DisableEnemy());
        }
       
        IEnumerator DisableEnemy()
        {
            yield return new WaitForSeconds(2f);
            Enemy.SetActive(false);
        }
    }

}
