using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class AeroPlaneMove : MonoBehaviour
{
    public float speed;

    // public ParticleSystem BlastParticle;
    // public AudioSource blast;
    //[Header("Info")]
    //private Vector3 _startPos;
    //private float _timer;
    //private Vector3 _randomPos;

    //[Header("Settings")]
    //[Range(0f, 2f)]
    //public float _time = 0.2f;
    //[Range(0f, 2f)]
    //public float _distance = 0.1f;
    //[Range(0f, 0.1f)]
    //public float _delayBetweenShakes = 0f;

    //private void Awake()
    //{
    //    _startPos = transform.position;

    //}

    //private void OnValidate()
    //{
    //    if (_delayBetweenShakes > _time)
    //        _delayBetweenShakes = _time;
    //}

    //public void Begin()
    //{
    //    StopAllCoroutines();
    //    StartCoroutine(Shake());
    //}

    //private IEnumerator Shake()
    //{
    //    _timer = 0f;

    //    while (_timer < _time)
    //    {
    //        _timer += Time.deltaTime;

    //        _randomPos = _startPos + (Random.insideUnitSphere * _distance);

    //        transform.position = _randomPos;

    //        if (_delayBetweenShakes > 0f)
    //        {
    //            yield return new WaitForSeconds(_delayBetweenShakes);
    //        }
    //        else
    //        {
    //            yield return null;
    //        }
    //    }

    //    transform.position = _startPos;
    //}
    private void Start()
    {
        transform.DORotate(new Vector3(transform.rotation.x, transform.rotation.y, -9f), 2f).SetEase(Ease.Linear).OnComplete(() =>
         {
             transform.DORotate(new Vector3(transform.rotation.x, transform.rotation.y, 9f), 2f).SetEase(Ease.Linear).SetLoops(-1,LoopType.Yoyo);
         });
    }
    void FixedUpdate()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
       // Begin();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Shake"))
        {
            AudioManager.instance.Play("Explosion");
           // BlastParticle.Play(true);
            // CameraEffects.ShakeOnce(0.5f, 10f);
        }
        if (other.gameObject.CompareTag("DummyWall"))
        {
            gameObject.GetComponent<Rigidbody>().useGravity = true;
            speed = 50;
        }
    }

}
