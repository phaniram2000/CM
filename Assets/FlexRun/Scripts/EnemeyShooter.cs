using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class EnemeyShooter : MonoBehaviour
{
    public Rigidbody[] Ragdool;
    public Rig rig;
    public Transform StartTarget;
    public Collider EneCollider;
    public GameObject BulletPrefab;
    public LineRenderer BulletPathLine;
    public Transform BulletStart,BulletEnd,BulletSpawnPos;
    public Vector3 LastFrameBulletEnd;
    GameObject TempObject;
    Animator Enemey;
    Vector3 direction;
    bool isPlayerinaim,isaim,isshooted;
    bool testaim, shoot,iscanshoot;
    public float Timer,BulletSpeed,MovementSpeed,afterShootRunSpeed;
    public float FTimer,FBulletSpeed,FMovementSpeed,FafterShootRunSpeed;
    float oldtimer, oldbulletspeed, oldmovespeed, oldaftershootrun;
    float v = 1;
    RigBuilder rb;
    bullet BB;
    public Transform StartPos, EndPos,AfterLoosePos;
    private FlexRun_GameManager flexRunGM;
    // Start is called before the first frame update
    void Start()
    {
        isshooted = false;
        EneCollider.enabled = false;
        rb = GetComponent<RigBuilder>();
        rb.enabled = false;
        isaim = false;
        testaim = false;
        Enemey = gameObject.GetComponent<Animator>();
        Enemey.SetBool("isaim", false);
        BulletPathLine.enabled = false;
        TempObject = new GameObject();
        isPlayerinaim = false;
        oldtimer = Timer;
        oldmovespeed = MovementSpeed;
        oldbulletspeed = BulletSpeed;
        oldaftershootrun = afterShootRunSpeed;

        for(int i = 0; i < Ragdool.Length; i++)
        {
            Ragdool[i].GetComponent<Collider>().enabled = false;
            Ragdool[i].GetComponent<Collider>().isTrigger = true;
            Ragdool[i].useGravity = true;
            Ragdool[i].isKinematic =true;
        }
        flexRunGM = FlexRun_GameManager.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        //basic aiming animations and line render controls
        if (EneCollider.GetComponent<EnemeyRigid>().isEnemeyTouched == true)
        {
            KillEne();
        }
        if (gameObject.transform.position != EndPos.position && EneCollider.GetComponent<EnemeyRigid>().isEnemeyTouched == false)
        {
            Move();
        }
        else 
        {
            Aim();
        }
        if (flexRunGM.isFeverBarFull == true)
        {
            Timer = FTimer;
            MovementSpeed = FMovementSpeed;
            BulletSpeed = FBulletSpeed;
            afterShootRunSpeed = FafterShootRunSpeed;
        }
        else if(flexRunGM.isFeverBarFull==false)
        {
            Timer = oldtimer;
            MovementSpeed = oldmovespeed;
            BulletSpeed = oldbulletspeed;
            afterShootRunSpeed = oldaftershootrun;
        }
        
        //raycast projection
        if (isaim == true)
        {
            var ray = new Ray(BulletStart.position, transform.forward);
            RaycastHit hit;
            Vector3 fromPosition = BulletStart.transform.position;
            Vector3 toPosition = BulletEnd.transform.position;
            direction = toPosition - fromPosition;


            if (Physics.Raycast(BulletStart.transform.position, direction, out hit))
            {
                if (hit.transform.gameObject.CompareTag("Player"))
                {
                    print("HitPlayer");
                    TempObject.transform.position = new Vector3(hit.point.x, hit.point.y,hit.point.z);
                    isPlayerinaim = true;
                }
                else if (hit.transform.CompareTag("Wall")|| hit.transform.CompareTag("Untagged"))
                {
                    print("PlayerWentaWay");
                    print(hit.transform.gameObject.name);
                    isPlayerinaim = false;
                }
            }
        }

        if (isPlayerinaim==true)
        {
            rig.weight = 1;
            BulletPathLine.SetPosition(0, BulletStart.position);
            BulletPathLine.SetPosition(1, TempObject.transform.position);
        }
        else
        {
            rig.weight = 1;
            BulletPathLine.SetPosition(0, BulletStart.position);
            BulletPathLine.SetPosition(1, BulletEnd.position);
        }
        if (shoot)
        {
            Shoot();
            shoot = false;
        }
        else
        {
            Enemey.SetBool("isshoot", false);
        }
        if (v == 1&&iscanshoot&&flexRunGM.isplayerDead==false)
        {
            StartCoroutine(WaitShoot());
            v += 1;
        }
        if (isshooted==true)
        {
            if(BB.isplayerescaped == true)
            {
                print("Move the Enemey");
                AfterShoot();
            }
        }
        if (flexRunGM.isplayerDead)
        {
            isaim = false;
            testaim = false;
            iscanshoot = false;
            rb.enabled = false;
            BulletPathLine.enabled = false;
        }
    }

    public void Aim()
    {
        rb.enabled = true;
        iscanshoot = true;
        rig.weight = 1;
        isaim = true;
        testaim = true;
        Enemey.SetBool("isaim", true);
        Enemey.SetBool("isshoot", false);
        BulletPathLine.enabled = true;
        BulletPathLine.SetPosition(0, BulletStart.position);
        BulletPathLine.SetPosition(1, BulletEnd.position);
    }


    public void KillEne()
    {
        
        Enemey.enabled = false;
        for (int i = 0; i < Ragdool.Length; i++)
        {
            Ragdool[i].GetComponent<Collider>().enabled = true;
            Ragdool[i].GetComponent<Collider>().isTrigger = false;
            Ragdool[i].useGravity = true;
            Ragdool[i].isKinematic = false;
            Ragdool[i].AddExplosionForce(300f, Vector3.right, 50f);
        }
    }
    public void Move()
    {
        iscanshoot = false;
        rb.enabled = false;
        rig.weight = 0;
        Enemey.SetBool("isaim", false);
        Enemey.SetBool("isshoot", false);
        gameObject.transform.position = Vector3.MoveTowards(StartPos.position, EndPos.position, MovementSpeed*Time.deltaTime);
       
    }
    public void Shoot()
    {
        rig.weight = 0;
        BulletPathLine.SetPosition(0, BulletStart.position);
        Enemey.SetBool("isaim", true);
        Enemey.SetBool("isshoot", true);
        LastFrameBulletEnd = BulletEnd.position;
        GameObject Bullet = Instantiate(BulletPrefab, BulletSpawnPos.position, BulletSpawnPos.rotation);
        Bullet.GetComponent<Rigidbody>().velocity = direction* BulletSpeed * Time.deltaTime;
        BB = Bullet.GetComponent<bullet>();
        if (AudioManager.instance != null)
        {
            AudioManager.instance.Play("Shoot");
        }
        isshooted = true;

    }

    public void AfterShoot()
    {
        rb.enabled = false;
        BulletPathLine.enabled = false;
        EneCollider.enabled = true;
        Enemey.SetBool("isaim", false);
        Enemey.SetBool("isshoot", false);
        if(EneCollider.GetComponent<EnemeyRigid>().isEnemeyTouched == false)
        {
            gameObject.transform.position = Vector3.MoveTowards(transform.position, AfterLoosePos.position, afterShootRunSpeed * Time.deltaTime);
        }
    }
    IEnumerator WaitShoot()
    {
        yield return new WaitForSeconds(Timer);
        shoot = true;
    }
}
