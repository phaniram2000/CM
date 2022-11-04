using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class BossFall : MonoBehaviour
{
    public Animator enemyAnim;
    public Rigidbody rb;
    const string PLAYER_PUNCH = "Punch";
    public static BossFall instance;
    public Collider m_collider;
    public int RagdollKickForce;
    private string currentAnimaton;
    public List<Rigidbody> EnemyRb;
    //public TimeManager timeManager;
    public float slowdownFactor;
    public GameObject ParticleEffect;
    public GameObject Enemy;
    public GameObject PunchText;
    public Animator playerAnim;
    public int Maxbosshealth = 200;
    public int bosshealth;
    public BossSlider healthBar;
    public GameObject HealthBar;
    const string PLAYER_HIPHOPDANCE = "HipHopDancing";
    const string PLAYER_KICK = "Flying_Kick";
  //  public GameObject PaticleDisable;
    void ChangeAnimationState(string newAnimation)
    {
        if (currentAnimaton == newAnimation) return;

        playerAnim.Play(newAnimation);
        currentAnimaton = newAnimation;
    }
    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        //bosshealth = 200;
        bosshealth = Maxbosshealth;
        healthBar.setmaxhealth(Maxbosshealth);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //transform.Translate(Vector3.up * 300 * Time.deltaTime);
       // transform.Translate(Vector3.forward * 500 * Time.deltaTime);
    }

    public void takedamage(int Damage_amount)
    {
        bosshealth -= Damage_amount;
        healthBar.sethealth(bosshealth);
        if (bosshealth <= 0)
        {
            bosshealth = 0;
            ChangeAnimationState(PLAYER_KICK);
            Player.instance.isControlFall = false;
            StartCoroutine(BossDeath());
            StartCoroutine(Win());
        }
        print(bosshealth);
        IEnumerator BossDeath()
        {
            yield return new WaitForSeconds(0.5f);
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            AudioManager.instance.Play("Hit");
            //  m_collider.enabled = false;
            for (int i = 0; i < EnemyRb.Count; i++)
            {
                EnemyRb[i].AddForce(Vector3.forward * RagdollKickForce, ForceMode.Force);
            }
            Player.instance.isControl = false;
            enemyAnim.enabled = false;
            HealthBar.SetActive(false);
            ParticleEffect.SetActive(true);
            PunchText.SetActive(false);
        }

        IEnumerator Win()
        {
            yield return new WaitForSeconds(2f);
            uimanagr.instance.win_panel();
        }
    }
}
