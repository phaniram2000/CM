using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;

public enum ShootOut_PlayerState
{
    None, Aim, Shoot, Run, Die, Finish
};
public class ShootOut_Player : SingletonInstance<ShootOut_Player>,ShootOut_IFallDown
{
    public bool isDead;
    public Rigidbody rb;
    public GameObject gun;
    public Collider myCol;
    public LineRenderer laser;
    public Animator myAnimator;
    public LayerMask layerMask;
    public Transform spawnPoint;
    public ParticleSystem muzzleFlash;
    public ShootOut_PlayerState myState;
    public Rigidbody[] bodyParts;
    
    private RaycastHit _hit;
    [SerializeField]private GameObject opponent;
    private static readonly int Run = Animator.StringToHash("Run");
    private static readonly int Shoot = Animator.StringToHash("Shoot");
    private static readonly int Victory = Animator.StringToHash("Victory");
    private static readonly int ShootPos = Animator.StringToHash("ShootPos");
    
    private static ShootOut_GameManager gm => ShootOut_GameManager.instance;

    protected override void Awake()
    {
        rb = GetComponent<Rigidbody>();
        myCol = GetComponent<Collider>();
        laser = GetComponent<LineRenderer>();
        myAnimator = GetComponentInChildren<Animator>();

        BodyPartsStatus(true);
    }

    private void ShootNow()
    {
        muzzleFlash.Play();
        myAnimator.SetTrigger(Shoot);
        AudioManager.instance.Play("GunShoot");
    }

    private void BodyPartsStatus(bool status)
    {
        foreach (var t in bodyParts)
        {
            t.isKinematic = status;
            t.GetComponent<Collider>().enabled = !status;
        }
    }

    private void Update()
    {
        if(myState is not (ShootOut_PlayerState.Shoot or ShootOut_PlayerState.Aim) || !gm.startGame) return;
        
        PlayerRotation();
        DetectingObjects();
    }

    private void PlayerRotation()
    {
        if (Input.GetMouseButton(0))
        {
            var dir = new Vector3(0, Input.GetAxis("Mouse X"), 0);
            transform.Rotate(dir * (Time.deltaTime * 20));
            myState = ShootOut_PlayerState.Aim;
        }

        if (Input.GetMouseButtonUp(0))
        {
            myState = ShootOut_PlayerState.Shoot;
            ShootNow();
            if(opponent is null) return;
            gm.subLevels[gm.policeGroupIndex].ReflectHandler(false);
            gm.subLevels[gm.policeGroupIndex].PoliceShootNow();
            if( opponent.GetComponent<ShootOut_Police>()) opponent.GetComponent<ShootOut_Police>().FallDown();
            if( opponent.GetComponent<ShootOut_Bamboo>()) opponent.GetComponent<ShootOut_Bamboo>().ActivateSticks();
            
        }
    }

    private void DetectingObjects()
    {
        if(myState == ShootOut_PlayerState.Shoot) return;
        
        Debug.DrawRay(spawnPoint.position, spawnPoint.TransformDirection(Vector3.forward) * 100,Color.yellow);
        
        if (!Physics.Raycast(spawnPoint.position, spawnPoint.TransformDirection(Vector3.forward), out _hit, Mathf.Infinity, layerMask))
        {
            opponent = null;
            UpdateLineRenderer(spawnPoint.position, spawnPoint.TransformDirection(Vector3.forward) * 1000, laser); 
            gm.subLevels[gm.policeGroupIndex].ReflectHandler(false);
            return;     
        }
        
        if(_hit.collider is null) return;

        UpdateLineRenderer(spawnPoint.position, _hit.point, laser);
        
        
        if (_hit.collider.CompareTag("Reflect"))
        {
            opponent = null;
            gm.subLevels[gm.policeGroupIndex].ReflectHandler(true);
            var reflect = Vector3.Reflect(spawnPoint.TransformDirection(Vector3.forward), _hit.normal);
            Raycast(new Ray(_hit.point, reflect),_hit.collider.GetComponent<LineRenderer>());
        } else if (_hit.collider.CompareTag("Bamboo"))
        {
            opponent = _hit.collider.gameObject;
            gm.subLevels[gm.policeGroupIndex].ReflectHandler(false);

        } else if (_hit.collider.CompareTag("Bomb"))
        {
            opponent = null;
            gm.subLevels[gm.policeGroupIndex].ReflectHandler(false);
        }
        else if (_hit.collider.CompareTag("Enemy"))
        {
            opponent = _hit.collider.gameObject;
            gm.subLevels[gm.policeGroupIndex].ReflectHandler(false);
        }
        
    }

    private void Raycast(Ray ray, LineRenderer lr)
    {
        if (!Physics.Raycast(ray, out _hit, Mathf.Infinity, layerMask))
        {
            opponent = null;
            UpdateLineRenderer(ray.origin, ray.direction * 1000,lr); 
            return;     
        }
        
        if(_hit.collider is null) return;

        UpdateLineRenderer(ray.origin, _hit.point,lr);
        
        if (_hit.collider.CompareTag("Reflect"))
        {
            opponent = null;
        } else if (_hit.collider.CompareTag("Bamboo"))
        {
            opponent = _hit.collider.gameObject;
        } else if (_hit.collider.CompareTag("Bomb"))
        {
            opponent = null;
        }
        else if (_hit.collider.CompareTag("Enemy"))
        {
            opponent = _hit.collider.gameObject;
        }
        
    }

    private void UpdateLineRenderer(Vector3 pos1, Vector3 pos2, LineRenderer lr)
    {
        lr.positionCount = 2;
        lr.SetPosition(0,pos1);
        lr.SetPosition(1,pos2);
    }

    public void FallDown()
    {
        laser.enabled = false;
        myCol.enabled = false;
        BodyPartsStatus(false);
        myAnimator.enabled = false;
        myState = ShootOut_PlayerState.Die;
        gm.Vibrate(20);
        GameCanvas.game.MakeGameResult(1,1);
    }

    public void RunNow()
    {
        if(myState != ShootOut_PlayerState.Shoot) return;
        gm.policeGroupIndex++;
        gun.SetActive(false);
        laser.enabled = false;
        myState = ShootOut_PlayerState.Run;
        gm.subLevels[gm.policeGroupIndex].ReflectHandler(false);
        DOVirtual.DelayedCall(0.5f, () =>
        {
            myAnimator.SetTrigger(Run);
            gm.currencyStack.SetActive(true);
            ShootOut_CameraController.instance.Follow(transform);
            transform.rotation = Quaternion.identity;
            var z = gm.transform.GetChild(gm.policeGroupIndex-1).position.z;
            transform.DOMoveZ(z, 6f).SetEase(Ease.Flash).OnComplete(()=>
            {
                var position = transform.position;
                position = new Vector3(position.x, position.y, z);
                transform.position = position;
                gm.subLevels[gm.policeGroupIndex].gameObject.SetActive(true);
                Invoke(nameof(GetReadyToShoot),0.2f);
                gm.policeIndex = 0;
            });
        });
       
    }

    private void GetReadyToShoot()
    {
        gun.SetActive(true);
        laser.enabled = true;
        DOTween.Kill(transform);
        myAnimator.SetTrigger(ShootPos);
        myState = ShootOut_PlayerState.Aim;
        ShootOut_CameraController.instance.Follow(null);
    }

    public void FinishNow()
    {
        print("LC");
        gm.Vibrate(20);
        gun.SetActive(false);
        laser.enabled = false;
        myAnimator.SetTrigger(Victory);
        myState = ShootOut_PlayerState.Finish;
        gm.confetti.transform.position = transform.position + new Vector3(0,3f,2f);
        gm.confetti.Play();
        ShootOut_CameraController.instance.Follow(null);
        GameCanvas.game.MakeGameResult(0,0);
        transform.DORotate(Vector3.up * 180f, 0.2f);
    }

    
}
