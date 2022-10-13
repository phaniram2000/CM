using Cinemachine;
using DG.Tweening;
using Dreamteck.Splines;
using UnityEngine;

public class ToiletMale : ToiletNpc
{
	public bool getsKickedOut;
	[SerializeField] private GameObject fallSmoke;

	private CinemachineImpulseSource _impulse;

	private static readonly int GetKicked = Animator.StringToHash("getKicked");
	private static readonly int StopRunning = Animator.StringToHash("stopRunning");
	private static bool _hasPassedOnce;

	private void Start()
	{
		InitialiseAbstractVariables();
		_impulse = GetComponent<CinemachineImpulseSource>();
	}

	public void GetKickedOut()
	{
		Anim.SetTrigger(GetKicked);
	}

	public override void EnterToilets(in float delay, SplineComputer spline)
	{
		DOVirtual.DelayedCall(delay, () =>
		{
			Spliner.SplineFollower.spline = spline;
			Spliner.SplineFollower.follow = true;
		});
	}

	public void RunFromWomensToilet(in float delay, SplineComputer spline)
	{
		Anim.SetTrigger(Run);

		DOVirtual.DelayedCall(delay, () =>
		{
			Spliner.SplineFollower.followSpeed = 1.5f;
			Spliner.SplineFollower.spline =
				getsKickedOut ? Spliner.Helper.GetMenExitSplineKicked() : spline;
			Spliner.SplineFollower.autoStartPosition = true;
			Spliner.SplineFollower.Restart();
		});
	}

	public void RunTowardsWomen(in float delay, SplineComputer spline)
	{
		Anim.SetTrigger(Run);

		DOVirtual.DelayedCall(delay, () =>
		{
			Spliner.SplineFollower.followSpeed = 1f;
			Spliner.SplineFollower.spline = spline;
			Spliner.SplineFollower.autoStartPosition = true;
			Spliner.SplineFollower.Restart();
		});
	}

	public void ReachDoor()
	{
		_hasPassedOnce = true;
		DOVirtual.DelayedCall(_hasPassedOnce ? 2f : .5f, () =>
		{
			Anim.SetTrigger(StopRunning);
			Spliner.SplineFollower.follow = false;

			var newPos = transform.position + transform.right * 0.5f * (Random.value > 0.5f ? 1f : -1f);
			
			transform.DOMove(newPos, Random.value / 3).SetEase(Ease.OutSine);
		});
	}

	public void FallDownOnAnimation()
	{
		fallSmoke.SetActive(true);
		fallSmoke.transform.parent = null;
		_impulse.GenerateImpulse();
	}
}