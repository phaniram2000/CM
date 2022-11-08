using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PathCreatorLine : MonoBehaviour
{
    private LineRenderer lineRenderer;
    public List<Vector3> points = new List<Vector3>();
    public Action<IEnumerable<Vector3>> OnNewPathCreated = delegate { };

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        if (Input.GetButtonDown("Fire1"))
            points.Clear();
        if (Input.GetButton("Fire1"))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;
            if (Physics.Raycast(ray, out hitInfo))
            {
                if (DistanceToLastPoint(hitInfo.point) > 1.5f)
                {
                    points.Add(hitInfo.point);
                    lineRenderer.positionCount = points.Count;
                    lineRenderer.SetPositions(points.ToArray());
                  //  GameObject Player = ObjectPool.instance.GetPooledObjects();
                    //if (Player != null)
                    //{
                    //    Player.transform.position = hitInfo.point;
                    //    Player.transform.rotation = Quaternion.identity;
                    //    Player.SetActive(true);
                    //}
                }
            }
        }
        else if (Input.GetButtonUp("Fire1"))
            OnNewPathCreated(points);
    }

    private float DistanceToLastPoint(Vector3 point)
    {
        if (!points.Any())
        return Mathf.Infinity;
        return Vector3.Distance(points.Last(), point);
    }
}
