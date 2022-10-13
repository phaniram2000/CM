using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCarChasingAI : MonoBehaviour
{
    public float speed;
    public float increasedSpeed;
    public float rotateSpeed;
    Rigidbody carRb;
    Transform targetPlayer;
    Vector3 targetPosition;

    float chaseAngle;
    public bool isComplex = true;
    private void Start()
    {
        carRb = GetComponent<Rigidbody>();
        chaseAngle = Random.Range(10, 30);
        StartCoroutine(SpeedControl());
        EventHandler.instance.FinishReachEvent += StopMovement;
        targetPlayer = GameManagerTruck.instance.player.transform;
    }
    void FixedUpdate()
    {
        if (!GameManagerTruck.instance.gameOver)
        {
            carRb.position += transform.forward * speed * Time.deltaTime;
            targetPosition = targetPlayer.transform.position;
        }
    }
    private void Update()
    {
        if (!GameManagerTruck.instance.gameOver)
        {
            if (Vector3.Angle(transform.forward, targetPosition - transform.position) > chaseAngle)
            {
                Rotate(ChaseAngle(transform.forward, targetPosition - transform.position, Vector3.up));
            }
            else // Otherwise, stop rotating
            {
                Rotate(0);
            }
        }
    }

    void StopMovement()
    {
        speed = 0;
        this.gameObject.SetActive(false);
    }
    IEnumerator SpeedControl()
    {
        if (!GameManagerTruck.instance.gameOver && speed > 0)
        {
            yield return new WaitForSeconds(Random.Range(0, 1.5f));
            float speedAdder = Random.Range(0, 5);
            speed = GameManagerTruck.instance.truckSpeed + 1.75f;

        }
        yield return new WaitForSeconds(Random.Range(0, 2.5f));
        StartCoroutine(SpeedControl());
    }
    public void Rotate(float rotateDirection)
    {
        if (rotateDirection != 0)
        {
            transform.localEulerAngles += Vector3.up * rotateDirection * rotateSpeed * Time.deltaTime;

            //transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z);
        }
    }
    public float ChaseAngle(Vector3 forward, Vector3 targetDirection, Vector3 up)
    {
        float approachAngle = Vector3.Dot(Vector3.Cross(up, forward), targetDirection);
        if (approachAngle > 0f)
        {
            return 1f;
        }
        else if (approachAngle < 0f)
        {
            return -1f;
        }
        else
        {
            return 0f;
        }
    }
}
