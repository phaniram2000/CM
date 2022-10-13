using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Duster : MonoBehaviour
{
    private bool isOver;
    private bool up;
    private Vector3 startPosition;
    public GameObject item;
    public Camera cam;

    void Awake()
    {
        startPosition = item.transform.position;
    }

    private void Update()
    {
        RaycastHit hitinfo;
        if (Input.GetMouseButtonDown(0))
        {
            if (AudioManager.instance)
            {
                AudioManager.instance.Play("Drag");
            }
        }
        if (Input.GetMouseButton(0))
        {
            var ray = cam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hitinfo) && hitinfo.transform.tag == "EraseTarget")
            {
                Debug.Log("board");
                Vector3 pos = ray.origin + (ray.direction * 2);
                item.transform.position = pos;
               
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            if (AudioManager.instance)
            {
                AudioManager.instance.Pause("Drag");
            }
        }
    }

    void OnMouseEnter()
    {
        isOver = true;
    }

    private void OnMouseDrag()
    {
    }

    void OnMouseExit()
    {
        isOver = false;
    }
}