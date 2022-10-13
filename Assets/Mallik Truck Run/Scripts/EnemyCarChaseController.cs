using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCarChaseController : MonoBehaviour
{
    public Transform frontRayPoint;
    public Transform closerDetectorPoint;
    public Transform rightFrontRayPoint;
    public Transform rightBackRayPoint;
    public Transform leftFrontRayPoint;
    public Transform leftBackRayPoint;

    ChasingCar chasingScript;
    Transform playerTruck;
    private void Start()
    {
        playerTruck = GameObject.FindGameObjectWithTag("Player").transform;
        chasingScript = GetComponent<ChasingCar>();
        //StartCoroutine(IsAnyOneRightSide());
        //StartCoroutine(IsAnyOneLeftSide());
        //StartCoroutine(IsAnyOneFront());
        //StartCoroutine(CloserDetection(20));
    }
    private void Update()
    {
        IsAnyOneRightSide();
    }
    public IEnumerator IsAnyOneFront()
    {
        yield return new WaitForSeconds(0.8f);
        RaycastHit hit;
        Debug.DrawRay(frontRayPoint.position, transform.forward * 3, Color.red);
        if (Physics.Raycast(frontRayPoint.position, transform.forward, out hit, 3))
        {
            if (hit.collider.tag == "enemyObj" && IsRightClear())
            {
                //chasingScript.MoveSide(1.5f);
            }
            else if (hit.collider.tag == "enemyObj" && IsLeftClear())
            {
                //chasingScript.MoveSide(-1.5f);
            }
            else if (hit.collider.tag == "enemyObj" && (IsLeftClear() && IsRightClear()));
            {
                //chasingScript.MoveSide(1.5f);
            }
        }
        else
        {
            //StartCoroutine(chasingScript.IncreaseSpeed(4, 1));
        }
        yield return new WaitForSeconds(0.05f);
        StartCoroutine(IsAnyOneFront());
    }
    public void IsAnyOneRightSide()
    {
        bool presentOnRightFront = false, presentOnRightBack = false;

        RaycastHit hit;
        Debug.DrawRay(rightFrontRayPoint.position, transform.right * 3, Color.yellow);
        if (Physics.Raycast(rightFrontRayPoint.position, transform.right, out hit, 3))
        {
            presentOnRightFront = true;
        }
        Debug.DrawRay(rightBackRayPoint.position, transform.right * 3, Color.yellow);
        if (Physics.Raycast(rightBackRayPoint.position, transform.right, out hit, 3))
        {
            presentOnRightBack = true;
        }

        if (presentOnRightFront && presentOnRightBack)
        {
            transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x - 0.2f, transform.position.y, transform.position.z), 40f * Time.deltaTime);
        }
    }

    public IEnumerator IsAnyOneLeftSide()
    {
        bool presentOnLeftFront = false, presentOnLeftBack = false;

        yield return new WaitForSeconds(0.8f);
        RaycastHit hit;
        Debug.DrawRay(leftFrontRayPoint.position, -transform.right * 6, Color.green);
        if (Physics.Raycast(leftFrontRayPoint.position, -transform.right, out hit, 6))
        {
            presentOnLeftFront = true;
            if (hit.collider.tag == "Player")
            {
                //chasingScript.MoveSide(-0.5f);
                //StartCoroutine(chasingScript.DecreaseSpeed());
            }
        }
        Debug.DrawRay(leftBackRayPoint.position, -transform.right * 6, Color.green);
        if (Physics.Raycast(leftBackRayPoint.position, -transform.right, out hit, 6))
        {
            presentOnLeftBack = true;
        }

        if (!presentOnLeftFront && !presentOnLeftBack)
        {
            //chasingScript.MoveSide(-2.5f);
        }

        yield return new WaitForSeconds(0.05f);
        StartCoroutine(IsAnyOneLeftSide());
    }

    IEnumerator CloserDetection(float callTime)
    {
        bool isClose = false;
        yield return new WaitForSeconds(callTime);
        RaycastHit hit;
        Debug.DrawRay(closerDetectorPoint.position, transform.forward * 12, Color.green);
        if (!Physics.Raycast(closerDetectorPoint.position, transform.forward, out hit, 12))
        {
           // StartCoroutine(chasingScript.IncreaseSpeed(7, 4));
        }
        yield return new WaitForSeconds(0.05f);
        //StartCoroutine(CloserDetection(0.8f));
    }
    bool IsRightClear()
    {
        bool presentOnRightFront = false, presentOnRightBack = false, isRightClear = false;

        RaycastHit hit;
        Debug.DrawRay(rightFrontRayPoint.position, transform.right * 6, Color.yellow);
        if (Physics.Raycast(rightFrontRayPoint.position, transform.right, out hit, 6))
        {
            presentOnRightFront = true;
        }
        Debug.DrawRay(rightBackRayPoint.position, transform.right * 6, Color.yellow);
        if (Physics.Raycast(rightBackRayPoint.position, transform.right, out hit, 6))
        {
            presentOnRightBack = true;
        }

        if (!presentOnRightFront && !presentOnRightBack)
        {
            isRightClear = true;
        }

        return isRightClear;
    }
    bool IsLeftClear()
    {
        bool presentOnLeftFront = false, presentOnLeftBack = false, isLeftClear = false;

        RaycastHit hit;
        Debug.DrawRay(rightFrontRayPoint.position, transform.right * 6, Color.yellow);
        if (Physics.Raycast(rightFrontRayPoint.position, transform.right, out hit, 6))
        {
            presentOnLeftFront = true;
        }
        Debug.DrawRay(rightBackRayPoint.position, transform.right * 6, Color.yellow);
        if (Physics.Raycast(rightBackRayPoint.position, transform.right, out hit, 6))
        {
            presentOnLeftBack = true;
        }

        if (!presentOnLeftFront && !presentOnLeftBack)
        {
            isLeftClear = true;
        }

        return isLeftClear;
    }
}
