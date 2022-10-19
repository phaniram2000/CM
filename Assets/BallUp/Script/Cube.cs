using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Cube : MonoBehaviour
{
    public bool emptyState;
    public Transform MainCamera;

    public float CameraSpeed;
    
    // Start is called before the first frame update
    void Start()
    {
        emptyState = true;
       
    }

    private void Update()
    {
       
        if (transform.childCount == 0)
            emptyState = true;
        else
        {
            emptyState = false;
        }
    }

    

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Deathknife"))
        {
           // print("move");
            var position = MainCamera.position;
            position = new Vector3(position.x, position.y + 0.55f, position.z);
            MainCamera.position = position;
            MainCamera.transform.DOMove(new Vector3(position.x, position.y + 0.55f,position.z), 1f)
               .SetEase(Ease.OutQuint);
        }
        // MainCamera.position= Vector3.Lerp(MainCamera.position,)
    }
}
