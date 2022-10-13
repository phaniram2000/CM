using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;


namespace ShuffleCups
{
	public class GirlPlayerController : MonoBehaviour
    {
    	[Header("On head hit with pie"), SerializeField] private Transform leftHandDest;
    	[SerializeField] private Transform rightHandDest;
    	[SerializeField] private ParticleSystem creamSplash;
    
    	private PlayerController _playa;
    	private Animator _anim;
    	private bool _hasHadHeadHit;
    
    	
    	private static readonly int HeadHit = Animator.StringToHash("headHit");
    
    	private void Start()
    	{
    		_playa = GetComponent<PlayerController>();
    		_anim = GetComponent<Animator>();
    	}
    
    	private void OnTriggerEnter(Collider other)
    	{
    		if (_hasHadHeadHit) return;
    		if (!other.CompareTag("Missile")) return;
    
    		_hasHadHeadHit = true;
    
    		creamSplash.Play(true);
    		
    		other.transform.parent.parent = null;
    		other.isTrigger = false;
    		other.transform.parent.gameObject.AddComponent<Rigidbody>();
    
    		StartCoroutine(Waiter());
    		PlayHandSeq();
    		_anim.SetTrigger(HeadHit);
    
    		Vibration.Vibrate(30);
    		AudioManager.instance.Play("splat");
    	}
    
    	private IEnumerator Waiter()
    	{
    		yield return new WaitForSeconds(1f);
    		_hasHadHeadHit = false;
    	}
    
    	private void PlayHandSeq()
    	{
    		var seq = DOTween.Sequence();
    
    		var initPosL = _playa.leftHand.position;
    		var initPosR = _playa.rightHand.position;
    		var initRotL = _playa.leftHand.rotation;
    		var initRotR = _playa.rightHand.rotation;
    
    		var duration = 0.5f;
    		
    		seq.Append(_playa.leftHand.DOMove(leftHandDest.position, duration));
    		seq.Insert(0f, _playa.leftHand.DORotateQuaternion(leftHandDest.rotation, duration));
    		
    		seq.Insert(0f, _playa.rightHand.DOMove(rightHandDest.position, duration));
    		seq.Insert(0f, _playa.rightHand.DORotateQuaternion(rightHandDest.rotation, duration));
    
    		seq.Append(_playa.leftHand.DOMove(initPosL, duration));
    		seq.Insert(duration, _playa.leftHand.DORotateQuaternion(initRotL, duration));
    		
    		seq.Insert(duration, _playa.rightHand.DOMove(initPosR, duration));
    		seq.Insert(duration, _playa.rightHand.DORotateQuaternion(initRotR, duration));
    	}
    }
}

