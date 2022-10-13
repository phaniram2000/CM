using DG.Tweening;
using UnityEngine;
using UnityEngine.Animations.Rigging;

namespace ShuffleCups
{
	public class EnemyController : MonoBehaviour
{
	[SerializeField] private Rigidbody reward;
	[SerializeField] private Rig giftRig, threatRig, aimRig;

	[Header("Hit with pie"), SerializeField] private bool isHittingWithPie;
	[SerializeField] private Transform pieHandTarget, pieHandDest;
	[SerializeField] private AnimationCurve pieHitCurve;

	[SerializeField] private GameObject muzzleFlash, bulletPrefab;
	[SerializeField] private Transform muzzle;
	[SerializeField] private float bulletForce;
	
	private Animator _anim;
	
	private static readonly int Shoot = Animator.StringToHash("shoot");
	private static readonly int PlayerWin = Animator.StringToHash("playerWin");
	private static readonly int Win = Animator.StringToHash("win");

	private void OnEnable()
	{
		GameEvents.Singleton.GameResult += OnGameResult;
	}

	private void OnDisable()
	{
		GameEvents.Singleton.GameResult -= OnGameResult;
	}

	private void Start()
	{
		_anim = GetComponent<Animator>();
	}

	private void Update()
	{
		if(Input.GetKeyDown(KeyCode.Space)) HitWithPie();
	}

	private void OnGameResult(bool didWin)
	{
		if (!didWin)
		{
			reward.isKinematic = false;
			reward.transform.parent = null;
			
			if (isHittingWithPie)
			{
				HitWithPie();
				return;
			}
			
			_anim.SetTrigger(Shoot);
			DOTween.To(() => giftRig.weight, value => giftRig.weight = value, 0f, 0.5f);
			DOTween.To(() => threatRig.weight, value => threatRig.weight = value, 0f, 0.5f);

			return;
		}
		
		//RIGS
		DOTween.To(() => giftRig.weight, value => giftRig.weight = value, 0f, 0.5f);
		DOTween.To(() => threatRig.weight, value => threatRig.weight = value, 0f, 0.5f);
		DOTween.To(() => aimRig.weight, value => aimRig.weight = value, .0f, 0.5f);

		_anim.SetTrigger(PlayerWin);

		//Suitcase to winSpot
		reward.transform.parent = null;
		var briefcaseSeq = DOTween.Sequence();
		var suitPos = LevelFlowController.only.winSuitcasePos.position;

		briefcaseSeq.Append(reward.transform.DORotate(Vector3.zero, 1f));
		briefcaseSeq.Insert(0f, reward.transform.DOMoveY(reward.position.y + 2f, 1f));
		briefcaseSeq.AppendInterval(3f);
		briefcaseSeq.Insert(3f, reward.transform.DOMoveX(suitPos.x, 1f));
		//briefcaseSeq.Append(reward.transform.DOMoveX(suitPos.x, 1f));
		briefcaseSeq.Insert(3f, reward.transform.DOMoveZ(suitPos.z, 2f));
		briefcaseSeq.Insert(3f, reward.transform.DORotateQuaternion(LevelFlowController.only.winSuitcasePos.rotation, 2f));
		briefcaseSeq.Insert(3f, reward.transform.DOMoveY(reward.transform.position.y + 7f, 1f));
		briefcaseSeq.Insert(4f, reward.transform.DOMoveY(suitPos.y, 1f));
		briefcaseSeq.Insert(5f, reward.transform.GetChild(1).DOLocalRotateQuaternion(Quaternion.identity, 1f));

		briefcaseSeq.Play();
	}

	private void HitWithPie()
	{
		var pieSequence = DOTween.Sequence();
		
		pieSequence.Append(pieHandTarget.DOMove(pieHandDest.position, 1f).SetEase(pieHitCurve));
		pieSequence.Insert(0.4f, pieHandTarget.DORotateQuaternion(pieHandDest.rotation, .5f));
		pieSequence.Insert(0.6f, DOTween.To(() => aimRig.weight, value => aimRig.weight = value, .5f, .5f));
		pieSequence.Insert(1f, DOTween.To(() => aimRig.weight, value => aimRig.weight = value, 0f, 0.5f));
		
		pieSequence.InsertCallback(1f, () =>
		{
			//RIGS
			DOTween.To(() => giftRig.weight, value => giftRig.weight = value, 0f, 0.5f);
			DOTween.To(() => threatRig.weight, value => threatRig.weight = value, 0f, 0.5f);
			DOTween.To(() => aimRig.weight, value => aimRig.weight = value, .0f, .5f);
		});

		pieSequence.Append(transform.DOMove(transform.position - transform.forward.normalized * 2f, 0.5f));

		pieSequence.AppendCallback(() =>
		{
			_anim.SetTrigger(Win);
			transform.DOMove(transform.position - transform.forward.normalized * 2f, 0.5f)
				.OnComplete(() => _anim.applyRootMotion = true);
		});
		//screen shake
	}

	public void EnableAimRig()
	{
		DOTween.To(() => aimRig.weight, value => aimRig.weight = value, .75f, 0.5f);
	}

	public void EnemyFireGunOnAnimation()
	{
		Destroy(Instantiate(muzzleFlash, muzzle.position, muzzle.rotation), 3f);
		
		var missile = Instantiate(bulletPrefab, muzzle.position, muzzle.rotation);
		
		var missileRb = missile.GetComponent<Rigidbody>();

		missileRb.AddForce((Vector3.up * 4f - missile.transform.position) * bulletForce, ForceMode.Impulse);
		if(!isHittingWithPie)
			Vibration.Vibrate(30);
	}

	public void PlayerDieOnAnimation()
	{
		DOTween.To(() => aimRig.weight, value => aimRig.weight = value, 0f, 2f);
	}
}
}


