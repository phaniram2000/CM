using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossFight : MonoBehaviour
{
    public Animator enemyAnim;
    public Rigidbody rb;
    const string PLAYER_PUNCH = "Punch";
    public static BossFight instance;
    public Collider m_collider;
    public int RagdollKickForce;
    private string currentAnimaton;
    public List<Rigidbody> EnemyRb;
    //public TimeManager timeManager;
    public float slowdownFactor;
    public GameObject ParticleEffect;
    public GameObject Enemy;
    public Animator playerAnim;
    public int Maxbosshealth = 200;
    public int bosshealth;
    public BossSlider healthBar;
    public GameObject HealthBar;
    const string PLAYER_HIPHOPDANCE = "HipHopDancing";
    public GameObject PaticleDisable;
    private void Awake()
    {
        instance = this;
    }
    void ChangeAnimationState(string newAnimation)
    {
        if (currentAnimaton == newAnimation) return;

        playerAnim.Play(newAnimation);
        currentAnimaton = newAnimation;
    }
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        //bosshealth = 200;
        bosshealth = Maxbosshealth;
        healthBar.setmaxhealth(Maxbosshealth);
    }
    void Update()
    {

    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            enemyAnim.Play("Punch");
            Player.instance.takedamageplayer(50);
            //damagePlayer();
        }
    }

    public void takedamage(int Damage_amount)
    {
        bosshealth -= Damage_amount;
        healthBar.sethealth(bosshealth);
      //  Debug.Log(bosshealth);
        if (bosshealth <= 0)
        {
            bosshealth = 0;
            // m_collider.GetComponent<CapsuleCollider>().enabled = false;
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            m_collider.enabled = false;
           //   transform.GetComponent<CapsuleCollider>().isTrigger = true;
            for (int i = 0; i < EnemyRb.Count; i++)
            {
                EnemyRb[i].AddForce(Vector3.forward * RagdollKickForce, ForceMode.Force);
                // transform.GetComponent<CapsuleCollider>().isTrigger = true;
            }
            Player.instance.isControl = false;
            enemyAnim.enabled = false;
            HealthBar.SetActive(false);
            PaticleDisable.SetActive(false);
            ParticleEffect.SetActive(true);
            StartCoroutine(winPanelDelay());
            
        }
    }

    IEnumerator winPanelDelay()
    {
        yield return new WaitForSeconds(2f);
        playerAnim.Play("HipHopDancing");
        uimanagr.instance.win_panel();
        Player.instance.disable();
    }
}

