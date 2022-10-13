using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TruckMovement : MonoBehaviour
{
    public static TruckMovement instance;
    [HideInInspector] public Rigidbody truckRb;
    float xInput;

    public float speed;
    public float turnSpeed;
    public Transform groundCheckPoint;

    [HideInInspector] public bool truckControlEnabled;

    public List<GameObject> truckParts;
    public GameObject speedParticle;
    [HideInInspector] public bool canControl;
    public GameObject barMeter;
    public Transform jumpPos;

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        truckRb = GetComponent<Rigidbody>();
        truckControlEnabled = true;
        speed = 0;
    }

    private void Update()
    {
#if UNITY_EDITOR
        xInput = Input.GetMouseButton(0) ? Input.GetAxis("Mouse X") * turnSpeed * Mathf.Deg2Rad : 0;
#elif UNITY_ANDROID
        if(Input.touchCount> 0 && Input.GetTouch(0).phase == TouchPhase.Moved)
		  {
			Vector2 touchDeltaPosition = Input.GetTouch(0).deltaPosition;
			xInput = touchDeltaPosition.x*8*Mathf.Deg2Rad;
          }
#endif
        //xInput = Input.GetMouseButton(0) ? Input.GetAxis("Mouse X") * turnSpeed * Mathf.Deg2Rad : 0;
        if (!GameManagerTruck.instance.gameOver)
        {
            if (Input.GetMouseButton(0) && canControl)
            {
                transform.Rotate(Vector3.up, xInput); ;

            }
            RaycastHit hit;
            Debug.DrawRay(groundCheckPoint.position, -transform.up * 10, Color.black);
            if (Physics.Raycast(groundCheckPoint.position, -transform.up, out hit, 10))
            {
                if (hit.collider.gameObject.layer != 0)
                {
                    GameManagerTruck.instance.StartCoroutine(GameManagerTruck.instance.LevelFailed(2));
                }
            }
            else
                GameManagerTruck.instance.StartCoroutine(GameManagerTruck.instance.LevelFailed(2));
        }
    }
    private void FixedUpdate()
    {
        //truckRb.velocity = -transform.forward * speed;
        if (!GameManagerTruck.instance.gameOver)
            truckRb.position -= transform.forward * speed * Time.deltaTime;
    }

    public void ExplodeTruck()
    {
        speed = 0;
        List<GameObject> allWoods = DroppablesController.instance.droppables;
        for (int i = 0; i < allWoods.Count; i++)
        {
            allWoods[i].GetComponent<Collider>().enabled = true;
            allWoods[i].GetComponent<Rigidbody>().isKinematic = false;
            allWoods[i].transform.tag = "Untagged";
            allWoods[i].transform.parent = null;
            allWoods[i].GetComponent<Rigidbody>().AddExplosionForce(Random.Range(2000, 6000), transform.position, 5);
        }
        for (int i = 0; i < truckParts.Count; i++)
        {
            if (truckParts[i].GetComponent<Collider>())
            {
                truckParts[i].GetComponent<Collider>().enabled = false;
                truckParts[i].AddComponent<Rigidbody>();
                truckParts[i].AddComponent<BoxCollider>();
            }
            else
            {
                truckParts[i].AddComponent<Rigidbody>();
                truckParts[i].AddComponent<BoxCollider>();
                truckParts[i].transform.parent = null;
                truckParts[i].GetComponent<Rigidbody>().AddExplosionForce(Random.Range(1000, 2000), transform.position, 5);
            }
        }
        AudioManager.instance.Play("TruckCrash");
        Vibration.Vibrate(50);
        /*GameObject explodeEffect = Instantiate(EffectsManager.instance.enemyCarBlast).gameObject;
        explodeEffect.transform.position = transform.position;
        Destroy(explodeEffect, 2);*/
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Finish"))
        {
            speed = 0;
            transform.position = new Vector3(-0.4f, transform.position.y, transform.position.z);
            //transform.Rotate(180, transform.localEulerAngles.y, transform.localEulerAngles.z, Space.Self);

            truckRb.isKinematic = true;
            VirtualCameraManager.instance.SwitchToFinishCamera();
            other.tag = "Untagged";
            StartCoroutine(WoodsCollection(other.gameObject));
            speedParticle.SetActive(false);
            //GameManager.instance.StartCoroutine(GameManager.instance.LevelComplete());
        }
        if (other.gameObject.CompareTag("finishGate"))
        {
            speedParticle.SetActive(false);
            AudioManager.instance.Play("Confetti");
            //truckRb.isKinematic = true;
            //speed = 0;
            //StartCoroutine(LevelWin());
            EventHandler.instance.FinishReached();
            for (int i = 0; i < other.transform.childCount; i++)
            {
                other.transform.GetChild(i).GetComponent<ParticleSystem>().Play();
            }
            Vibration.Vibrate(17);
            
        }
    }

    IEnumerator LevelWin()
    {
        VirtualCameraManager.instance.SwitchToFinishCamera();
        EventHandler.instance.GameOver();
        //SoundsManager.instance.BGMusicSource.enabled = false;
        AudioManager.instance.Play("Confetti");
        //transform.DORotate(new Vector3(transform.localEulerAngles.x, 0, transform.localEulerAngles.z), 0.35f);
        yield return new WaitForSeconds(2);
        GameEvents.InvokeGameWin();
    }
    float incremental = 0;
    IEnumerator WoodsCollection(GameObject finishLine)
    {

        List<GameObject> allWoods = GetComponent<DroppablesController>().droppables;
        GameObject tubeThroat = finishLine.transform.GetChild(0).gameObject;
        Vector3 movePos = tubeThroat.transform.position;

        List<GameObject> activeWoods = new List<GameObject>();
        Transform slider = finishLine.transform.GetChild(1);

        for (int i = allWoods.Count - 1; i >= 0; i--)
        {
            if (allWoods[i].activeInHierarchy)
            {
                activeWoods.Add(allWoods[i]);
            }
        }

        yield return new WaitForSeconds(2);
        tubeThroat.GetComponent<Animator>().enabled = true;
        finishLine.transform.GetChild(2).transform.GetComponent<ParticleSystem>().Play();
        for (int i = 0; i < activeWoods.Count; i++)
        {
            yield return new WaitForSeconds(0.15f);
            activeWoods[i].transform.DOMove(movePos, 0.2f);
            activeWoods[i].transform.DOScale(Vector3.zero, 0.2f);
            incremental += 0.05f;
            slider.localScale = new Vector3(1, 1, incremental);
            AudioManager.instance.Play("CollectionPop");
            Vibration.Vibrate(10);
        }
        tubeThroat.GetComponent<Animator>().enabled = false;
        finishLine.transform.GetChild(2).transform.GetComponent<ParticleSystem>().Stop();
        incremental = (float)System.Math.Round(incremental, 2);
        
        yield return new WaitForSeconds(1);
        StartCoroutine(CoinCollectionsEffect());
    }
    IEnumerator CoinCollectionsEffect()
    {
        /*Vector3 movePos = Camera.main.WorldToScreenPoint(transform.position);
        GameObject coinsParticle = EffectsManager.instance.coinsCollectionParticles.gameObject;
        RectTransform coinIconRect = GameManagerTruck.instance.coin.GetComponent<RectTransform>();
        coinsParticle.GetComponent<ParticleControlScript>().PlayControlledParticles(coinIconRect, coinsParticle.transform.position);*/
        //GameManagerTruck.instance.StartCoroutine(GameManagerTruck.instance.UpdateCoins(incremental * 100));
        yield return new WaitForSeconds(1f);
        AudioManager.instance.Play("CoinCollection");

        yield return new WaitForSeconds(0.5f);
        GameEvents.InvokeGameWin();
    }
    

    public GameObject glassParent;
    public void TruckAttacked()
    {
        BreakScreen();
        barMeter.transform.DOScale(Vector3.zero, 0.5f).OnComplete(() =>
        {
            barMeter.SetActive(false);
            truckRb.isKinematic = false;
        });
    }

    void BreakScreen()
    {
        glassParent.SetActive(true);
        Vector3 explosionPos = glassParent.transform.position;
        Vibration.Vibrate(27);
        /*Collider[] colliders = Physics.OverlapSphere(explosionPos, 3);
        foreach (Collider hit in colliders)
        {
            Rigidbody rb = hit.GetComponent<Rigidbody>();

            if (rb != null)
                rb.AddExplosionForce(250, explosionPos, 3, 3.0F);
        }*/
        AudioManager.instance.Play("GlassBreak");
        DOVirtual.DelayedCall(5, () => { glassParent.SetActive(false); });
        canControl = true;
    }
}
