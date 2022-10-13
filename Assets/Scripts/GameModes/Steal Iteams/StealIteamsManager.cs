using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class StealIteamsManager : MonoBehaviour
{
    private RaycastHit hitinfo;
    public Animator shopkeeper;
    public List<GameObject> TableIteams;
    public List<GameObject> Bagiteams;
    public Transform BagPosition;
    public GameObject downposition; 
    private StealIteamsScanner scann;
    public GameObject cone;
    public bool _start;
    public bool _winning;
    public bool _loosing;
    private void OnEnable()
    {
        GameEvents.TapToPlay += Taptoplay;
    }

    private void OnDisable()
    {
        GameEvents.TapToPlay -= Taptoplay;

    }

    public void Taptoplay()
    {
        DOVirtual.DelayedCall(.2f,() => _start = true);
       
        shopkeeper.enabled = true;
    }
    void Start()
    {
        shopkeeper = GetComponent<Animator>();
        scann = StealIteamsScanner.instance;
    }

    public void seque()
    {
        var seq = DOTween.Sequence();
        seq.AppendCallback(() =>
        {
            GameEvents.InvokeGameWin();
        });
    }

    public void looseseq()
    {
        var seq = DOTween.Sequence();
        seq.AppendCallback(() =>
        {
            print("Loose");
            GameEvents.InvokeGameLose(-1);
        });
    }
    void Update()
    {
        if (!_winning)
        {
            if (TableIteams.Count == 0)
            {
               seque();
            }
        }
        if (Input.GetMouseButtonDown(0) && _start)
        {
            var Ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(Ray, out hitinfo))
            {
                if (!Bagiteams.Contains(hitinfo.collider.gameObject) && hitinfo.transform.CompareTag("pickups"))
                {
                    if (scann.UnderScanner.Contains(hitinfo.transform.gameObject))
                    {
                        if (!_loosing)
                        {
                            looseseq();
                            _start = false;
                        }
                    }
                    else
                    {
                        
                        Selectedobject(hitinfo.transform.gameObject);
                        TableIteams.Remove(hitinfo.transform.gameObject);
                    }
                   
                }
            }
        }
    }

    public void Selectedobject(GameObject selectedobj)
    {
        if (AudioManager.instance)
        {
            AudioManager.instance.Play("Button");
        }
        var coll = selectedobj.GetComponent<Collider>();
        selectedobj.tag = "Untagged";
        Bagiteams.Add(selectedobj);
        coll.isTrigger = false;
        selectedobj.transform.DOMoveY( selectedobj.transform.position.y + 0.4f, 0.1f).SetEase(Ease.Linear).OnComplete(
            () =>
            {
                selectedobj.transform.DOMove(BagPosition.position, 0.2f).SetEase(Ease.Linear).OnComplete(() =>
                {
                    selectedobj.transform.DOScale(new Vector3(0.7f, 0.7f, 0.7f), 0.6f);
                    selectedobj.transform.DOMove(downposition.transform.position, 0.6f);
                });
            });
    }


    void MeshOff()
    {
        cone.GetComponent<MeshCollider>().enabled = false;
        scann.UnderScanner.Clear();

    }
    
    void MeshOn()
    {
        cone.GetComponent<MeshCollider>().enabled = true;
    }
}