using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using UnityEngine.SceneManagement;
using DG.Tweening;
using TMPro;
using DitzeGames.Effects;
using SWS;
using Cinemachine;

public class EnemyRD : MonoBehaviour
{
    private Collider MonsterCollider;
    private Collider[] RagdollColliders;
    public Animator anim;
    private float range;
    private Rigidbody EnemyRb;
    Collider m_collider;
    public float upForce;
    public float forwardForce;
    public static EnemyRD instance;
    //  public GameObject Explosion;
    // public TimeManager timeManager;
    private void Awake()
    {
        MonsterCollider = GetComponent<Collider>();
        RagdollColliders = GetComponentsInChildren<Collider>();
        anim = GetComponentInChildren<Animator>();
        ActivateRagdoll(false);
        instance = this;
    }
    private void Start()
    {
        m_collider = GetComponent<Collider>();
        m_collider.GetComponent<CapsuleCollider>().enabled = true;
    }

    private void ActivateRagdoll(bool Status)
    {
        foreach (Collider col in RagdollColliders)
        {
            col.enabled = Status;
        }
        //MonsterCollider.enabled = !Status;
        //anim.enabled = !Status;
        //GetComponent<Rigidbody>().useGravity = !Status;
        //GetComponent<Rigidbody>().isKinematic = false;
      //   GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        // GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionY;
    }
    public void KillEnemy(Vector3 ExplosionPosition)
    {
        ActivateRagdoll(true);
        //   EnemyManager.instance.Enemies.Remove(this.gameObject);
        foreach (Collider col in RagdollColliders)
        {
            // GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionY;
            //  col.GetComponent<Rigidbody>().AddExplosionForce(40f, ExplosionPosition, 3f, 3f, ForceMode.VelocityChange);
            //Explosion.SetActive(true);
            //  col.GetComponent<Rigidbody>().AddForce(new Vector3(-4, 0, 1) *10, ForceMode.Impulse);
            //  col.GetComponent<Rigidbody>().isKinematic = false;
            //col.GetComponent<Rigidbody>().AddForce(transform.forward * 10000, ForceMode.Force);
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            anim.enabled = false;
            col.GetComponent<Rigidbody>().AddForce(Vector3.forward * forwardForce, ForceMode.Force); // 20000 - level1
           // col.GetComponent<Rigidbody>().AddForce(Vector3.down * 5000, ForceMode.Force); // 20000 - level1
            col.GetComponent<Rigidbody>().AddForce(Vector3.up * upForce, ForceMode.Force);      // 40000 - level1
            AudioManager.instance.Play("FinalBoss");
            StartCoroutine(Disable());
            //col.GetComponent<Rigidbody>().AddForce(col.transform.up * 25000, ForceMode.Force);
            //  m_collider.enabled = false;
            // timeManager.DoSlowMotion();
            IEnumerator Disable()
            {
                yield return new WaitForSeconds(2f);
                this.gameObject.SetActive(false);
            }
        }
  
    }
}
