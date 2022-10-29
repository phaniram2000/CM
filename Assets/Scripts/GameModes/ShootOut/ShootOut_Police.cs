using UnityEngine;

public class ShootOut_Police : MonoBehaviour,ShootOut_IFallDown
{
    public bool isDead;
    public GameObject gun;
    public Collider myCol;
    public Animator myAnimator;
    public LineRenderer laser;
    public ParticleSystem muzzleFlash;
    public Rigidbody[] bodyParts;
    private RaycastHit _hit;
    public LayerMask layerMask;
    public GameObject opponent;
    public Transform spawnPoint;
    private static readonly int Shoot = Animator.StringToHash("Shoot");

    private ShootOut_Player _player;
    private static ShootOut_GameManager gm => ShootOut_GameManager.instance;
    private void Awake()
    {
        myCol = GetComponent<Collider>();
        laser = GetComponent<LineRenderer>();
        myAnimator = GetComponent<Animator>();

        BodyPartsStatus(true);
    }

    private void Start()
    {
        _player = FindObjectOfType<ShootOut_Player>();
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
        if(isDead || !gm.startGame) return;
        
        DetectingObjects();
        PlayerRotation();
    }
    
    private void PlayerRotation()
    {
        if (Input.GetMouseButton(0))
        {
            var dir = new Vector3(0, Input.GetAxis("Mouse X"), 0);
            transform.Rotate(dir * (Time.deltaTime * 20));
        }

        /*if (Input.GetMouseButtonUp(0))
        {
           ShootNow();
           if(opponent is null) return;
            
          if(opponent.GetComponent<ShootOut_Player>())  opponent.GetComponent<ShootOut_Player>().FallDown();
          if(opponent.GetComponent<ShootOut_Police>())  opponent.GetComponent<ShootOut_Police>().FallDown();
        }*/
    }
    
    private void DetectingObjects()
    {
        Debug.DrawRay(spawnPoint.position, spawnPoint.forward * 100,Color.yellow);
        
        if (!Physics.Raycast(spawnPoint.position, spawnPoint.forward, out _hit, Mathf.Infinity, layerMask))
        {
            opponent = null;
            UpdateLineRenderer(spawnPoint.position, spawnPoint.position+ spawnPoint.forward * 100); 
            return;     
        }
        
        if(_hit.collider is null) return;

        UpdateLineRenderer(spawnPoint.position, _hit.point);
        
        
        if (_hit.collider.CompareTag("Player"))
        {
            opponent = _hit.collider.gameObject;
            
        } else if (_hit.collider.CompareTag("Reflect"))
        {
            opponent = null;
        } else if (_hit.collider.CompareTag("Bamboo"))
        {
            opponent = null;
        } else if (_hit.collider.CompareTag("Bomb"))
        {
            opponent = null;
        }
    }

    private void UpdateLineRenderer(Vector3 pos1, Vector3 pos2)
    {
        laser.positionCount = 2;
        laser.SetPosition(0,pos1);
        laser.SetPosition(1,pos2);
    }

    public void FallDown()
    {
        ShootNow();
        Finish();
        isDead = true;
        myAnimator.enabled = false;
        BodyPartsStatus(false);
        gm.policeIndex++;
        CheckForRunOrWin();
        gm.Vibrate(20);
    }
    
    private void Finish()
    {
        gun.SetActive(false);
        myCol.enabled = false;
        laser.enabled = false;
    }

    private void CheckForRunOrWin()
    {
        if (gm.CheckAllPoliceGroups())
        {
            _player.FinishNow();
        }
        else
        {
            if (gm.CheckThisGroupPolices() && _player.myState != ShootOut_PlayerState.Finish) _player.RunNow();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name.Contains("Bamboo"))
        {
            if(isDead) return;
            FallDown();
        }
    }
}
