using DG.Tweening;
using Dreamteck.Splines;
using UnityEngine;

public class PubNpc : MonoBehaviour
{
	[SerializeField] protected bool isMale;
	private SplineFollower _spliner;
	protected Animator Anim { get; private set; }

	private static readonly int IsMirrored = Animator.StringToHash("isMirrored");
	private static readonly int IsWalking = Animator.StringToHash("isWalking");
	private static readonly int GenderBlender = Animator.StringToHash("genderBlender");

	protected virtual void OnEnable() => GameEvents.TapToPlay += OnTapToPlay;

	protected virtual void OnDisable() => GameEvents.TapToPlay -= OnTapToPlay;

	protected virtual void Start() => InitialiseVariables();

	private void InitialiseVariables()
	{
		_spliner = GetComponent<SplineFollower>();
		Anim = GetComponent<Animator>();

		Anim.SetBool(IsMirrored, Random.value > 0.5f);
		Anim.SetFloat(GenderBlender, isMale ? 0f : 1f);
	}

	public void QueueStopMoving() => StopFollowing();

	public void StartFollowing()
	{
		_spliner.follow = true;
		Anim.SetBool(IsWalking, true);
	}

	protected void StopFollowing()
	{
		_spliner.follow = false;
		Anim.SetBool(IsWalking, false);
	}

	protected void DisableSplineFollower()
	{
		_spliner.enabled = false;
	}
	
	private void OnTapToPlay() => StartFollowing();

	public virtual void TryEntry()
	{
		PubHelper.GetBouncer.SideStep();
		DOVirtual.DelayedCall(1f, StartFollowing);
	}
}