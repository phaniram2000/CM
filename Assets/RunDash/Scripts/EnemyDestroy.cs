using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
//using Cinemachine;

public class EnemyDestroy : MonoBehaviour
{
    public int RagdollKickForce;
    public Animator EnemyAnim;
    private string currentAnimaton;
    public List< Rigidbody> EnemyRb;
    //public TimeManager timeManager;
    public static EnemyDestroy instance;
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
            
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
           // m_collider.enabled = false;
           // transform.GetComponent<CapsuleCollider>().isTrigger = true;
            EnemyAnim.enabled = false;
            for(int i =0;i<EnemyRb.Count;i++)
            {
             EnemyRb[i].AddForce(Vector3.forward * RagdollKickForce, ForceMode.Force);
            // transform.GetComponent<CapsuleCollider>().isTrigger = true;
            }
            Time.timeScale = slowdownFactor * Time.deltaTime;
            ParticleEffect.SetActive(true);
            StartCoroutine(DisableEnemy());
        } 
        //if (collision.gameObject.CompareTag("Enemy"))
        //{
        //    GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        //    //GetComponent<Rigidbody>().useGravity = false;
        //   // m_collider.enabled = false;
        //   // transform.GetComponent<CapsuleCollider>().isTrigger = true;
        //   // EnemyAnim.enabled = false;
        //    for(int i =0;i<EnemyRb.Count;i++)
        //    {
        //     EnemyRb[i].AddForce(Vector3.forward * 100, ForceMode.Force);
        //    // transform.GetComponent<CapsuleCollider>().isTrigger = true;
        //    }
        //    Time.timeScale = slowdownFactor * Time.deltaTime;
        //    ParticleEffect.SetActive(true);
        //    print("EnemyHit");
        //}
        IEnumerator DisableEnemy()
        {
            yield return new WaitForSeconds(2f);
            Enemy.SetActive(false);
        }
    }

    
}





