using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class EnemyEnd : MonoBehaviour
{
    public Transform player;
    // Start is called before the first frame update
    void Start()
    {
        transform.DOMove(player.position, 2f).SetEase(Ease.Linear);
    }

    // Update is called once per frame
    void Update()
    {
      //  transform.LookAt(player.position);
    }


}
