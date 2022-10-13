using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Traps : MonoBehaviour
{
    [SerializeField] bool isSawTrap,isHammer;
    Transform SawBlade,HammerRotpoint;
    // Start is called before the first frame update
    void Start()
    {
        if (isSawTrap)
        {
            SawBlade = transform.GetChild(0).gameObject.transform;
            SawBlade.DORotate(new Vector3(-180, -90, 0), 5f).SetEase(Ease.Flash).SetLoops(-1, LoopType.Restart);
            //SawBlade.DOMove(new Vector3(0,SawBlade.position.y,1),5f);
            //SawBlade.DORotate(new Vector3(Time.deltaTime * 50f, 0, SawBlade.rotation.z), Time.deltaTime * 50, RotateMode.FastBeyond360);
        }
        else if (isHammer)
        {
            HammerRotpoint = transform.GetChild(1).gameObject.transform;
            HammerRotpoint.DORotate(new Vector3(-180, 0, 0), 2.5f).SetEase(Ease.Flash).SetLoops(-1, LoopType.Yoyo);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
