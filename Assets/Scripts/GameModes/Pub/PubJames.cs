using DG.Tweening;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public sealed class PubJames : PubNpc
{
	public GameObject idCard;
	[SerializeField] private Transform rigTarget, handoverCardTransform;

	[SerializeField] private float endYPos, fallDuration;
	private Rig _rig;

	private static readonly int Pushed = Animator.StringToHash("pushed");
	private static readonly int InPostPub = Animator.StringToHash("inPostPub");

	protected override void Start()
	{
		base.Start();
		_rig = GetComponent<RigBuilder>().layers[0].rig;
	}
	protected override void OnEnable()
	{
		base.OnEnable();
		PubEvents.DoneWithInput += OnDoneWithInput;
		
		GameEvents.GameWin += OnGameWin;

		PubEvents.StartPostPub += OnStartPostPub;
	}

	protected override void OnDisable()
	{
		base.OnDisable();
		PubEvents.DoneWithInput -= OnDoneWithInput;
		
		GameEvents.GameWin -= OnGameWin;
		
		PubEvents.StartPostPub -= OnStartPostPub;
	}

	public override void TryEntry()
	{
		PubHelper.GetBouncer.Talk();
		DOVirtual.DelayedCall(2.5f, () =>
		{
			PubEvents.InvokeStartInput();
			HoldCardUp();
		});
	}

	public void GetPushed() => Anim.SetTrigger(Pushed);

	private void HoldCardUp() => DOTween.To(() => _rig.weight, value => _rig.weight = value,
		1f, 1f);

	private void HeldUpToHandoverCard()
	{
		rigTarget.DOMove(handoverCardTransform.position, 0.5f)
			.SetDelay(0.5f)
			.OnStart(() =>
				rigTarget.DORotateQuaternion(handoverCardTransform.rotation, 0.5f)
					.OnComplete(() =>
					{
						PubHelper.GetBouncer.HandoverCard(this);
						DOTween.To(() => _rig.weight, value => _rig.weight = value, 
								0f, 1f)
							.SetDelay(1f);
					}));
	}

	public void StartFallingOnAnimation()
	{
		var endPos = transform.position;
		endPos.y = endYPos;
		endPos += transform.forward * 1f;
		
		transform.DOMove(endPos, fallDuration);
	}

	private void OnDoneWithInput() => HeldUpToHandoverCard();

	private void OnGameWin()
	{
		StartFollowing();
		if(AudioManager.instance)
			AudioManager.instance.Play("Laugh");
		
		Vibration.Vibrate(50);
	}


	private void OnStartPostPub()
	{
		StopFollowing();
		DisableSplineFollower();
		Anim.SetBool(InPostPub, true);
	}
}