using DG.Tweening;
using Dreamteck.Splines;
using UnityEngine;

public class ToiletFemale : ToiletNpc
{
	[SerializeField] private bool trips;

	private bool _hasTripped, _startedMoving;
	
	private static readonly int Chase = Animator.StringToHash("chase");
	private static readonly int Trip = Animator.StringToHash("trip");

	private void Start() => InitialiseAbstractVariables();
	
	public void StartChasing() => Anim.SetTrigger(Chase);

	public override void EnterToilets(in float delay, SplineComputer spline)
	{
		DOVirtual.DelayedCall(delay, () =>
		{
			Spliner.SplineFollower.spline = spline;
			Spliner.SplineFollower.follow = true;
		});
	}

	public void ChaseAwayMen(in float delay, SplineComputer spline)
	{
		Anim.SetTrigger(Chase);

		DOVirtual.DelayedCall(delay, () =>
		{
			_startedMoving = true;
			Spliner.SplineFollower.followSpeed = 1.5f;
			Spliner.SplineFollower.spline = spline;
			Spliner.SplineFollower.autoStartPosition = true;
			Spliner.SplineFollower.Restart();
		});
	}

	public void RunAwayFromMen(in float delay, SplineComputer spline)
	{
		Anim.SetTrigger(Run);

		DOVirtual.DelayedCall(delay, () =>
		{
			_startedMoving = true;
			Spliner.SplineFollower.followSpeed = 1.5f;
			Spliner.SplineFollower.spline = spline;
			Spliner.SplineFollower.autoStartPosition = true;
			Spliner.SplineFollower.Restart();
		});
	}

	private void OnCollisionEnter(Collision collision)
	{
		if (!trips) return;
		if(_hasTripped) return;
		if(!_startedMoving) return;
		
		if(!collision.collider.CompareTag("NPC")) return;
		if(!collision.collider.TryGetComponent(out ToiletMale male)) return;
		if(!male.getsKickedOut) return;
		
		_hasTripped = true;
		Anim.SetTrigger(Trip);
		//StopWaving(Anim);

		transform.DOMove(transform.position + transform.forward * 1.75f, 0.75f)
			.SetEase(Ease.InQuad);
		Spliner.SplineFollower.follow = false;
		GetComponent<Rigidbody>().isKinematic = true;
	}
	
	private void StopWaving(Animator animator) => TweenLayerWeightTo(animator, 0f);
	
	private void TweenLayerWeightTo(Animator anim, in float value)
	{
		Debug.LogWarning("Refactor");
		DOTween.Kill(anim);
		DOTween.To(
				() => anim.GetLayerWeight(1),
				value => anim.SetLayerWeight(1, value), 
				value,
				0.5f)
			.SetTarget(anim);
	}
}