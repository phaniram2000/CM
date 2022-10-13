using Dreamteck.Splines;
using UnityEngine;

public abstract class ToiletNpc : MonoBehaviour
{
	protected ToiletSplineFollower Spliner;
	protected Animator Anim;

	protected static readonly int Run = Animator.StringToHash("run");
	private static readonly int IsMirrored = Animator.StringToHash("isMirrored");

	protected void InitialiseAbstractVariables()
	{
		Spliner = GetComponent<ToiletSplineFollower>();
		Anim = GetComponent<Animator>();
		Anim.SetBool(IsMirrored, Random.value > 0.5f);
	}

	public abstract void EnterToilets(in float delay, SplineComputer spline);
}