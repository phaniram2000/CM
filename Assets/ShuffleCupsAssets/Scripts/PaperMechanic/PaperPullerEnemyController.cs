using DG.Tweening;
using UnityEngine;
using UnityEngine.Animations.Rigging;

namespace ShuffleCups
{
	public class PaperPullerEnemyController : MonoBehaviour
{
	[SerializeField] private Rigidbody reward;
	[SerializeField] private Rig giftRig, threatRig, aimRig;
	
	[Header("Hit with pie"), SerializeField] private bool isThrowingPie;
	[SerializeField] private Rigidbody pie;

	[Header("Bullet"), SerializeField] private Transform muzzle;
	[SerializeField] private GameObject muzzleFlash, bulletPrefab;
	[SerializeField] private float bulletForce;

	private Animator _anim;

	private bool _didPlayerLose;
	
	private static readonly int Shoot = Animator.StringToHash("shoot");
	private static readonly int Throw = Animator.StringToHash("throw");

	private void OnEnable()
	{
		PaperGameEvents.Singleton.tearPaper += OnPlayerLose;
		PaperGameEvents.Singleton.aiCrossFinishLine += OnPlayerLose;
		PaperGameEvents.Singleton.playerCrossFinishLine += OnPlayerWin;
	}

	private void OnDisable()
	{
		PaperGameEvents.Singleton.tearPaper -= OnPlayerLose;
		PaperGameEvents.Singleton.aiCrossFinishLine -= OnPlayerLose;
		PaperGameEvents.Singleton.playerCrossFinishLine -= OnPlayerWin;
	}

	private void Start()
	{
		_anim = GetComponent<Animator>();
	}

	private void BriefCaseSequence()
	{
		reward.transform.parent = null;
		var briefcaseSeq = DOTween.Sequence();
		var suitPos = PaperLevelFlowController.only.suitcaseWinPos.position;

		briefcaseSeq.AppendInterval(1f);
		briefcaseSeq.Append(reward.transform.DORotate(Vector3.zero, 1f));
		briefcaseSeq.Insert(1f, reward.transform.DOMoveY(reward.position.y + 2f, 1f));
		briefcaseSeq.AppendInterval(2.5f);
		briefcaseSeq.Insert(3.5f, reward.transform.DOMoveX(suitPos.x, 1f));
		briefcaseSeq.Insert(3.5f, reward.transform.DOMoveZ(suitPos.z, 2f));
		briefcaseSeq.Insert(3.5f, reward.transform.DORotateQuaternion(PaperLevelFlowController.only.suitcaseWinPos.rotation, 2f));
		briefcaseSeq.Insert(3.5f, reward.transform.DOMoveY(reward.transform.position.y + 7f, 1f));
		briefcaseSeq.Insert(4.5f, reward.transform.DOMoveY(suitPos.y, 1f));
		
		if(reward.transform.childCount > 0)
			briefcaseSeq.Insert(5.5f, reward.transform.GetChild(1).DOLocalRotateQuaternion(Quaternion.identity, 1f));

		briefcaseSeq.Play();
	}
	
	private void Projectile()
	{
		var target = _didPlayerLose
			? GameObject.FindGameObjectWithTag("Player")
			: GameObject.FindGameObjectWithTag("Enemy");
		
		if(target)
		{
			var targetPos = target.transform.position;
			targetPos.y = transform.position.y;

			var direction = targetPos - transform.position;
			transform.DORotateQuaternion(Quaternion.LookRotation(direction), 0.5f);
		}
		
		reward.transform.parent = null;
		if(_didPlayerLose)
			reward.isKinematic = false;

		var seq = DOTween.Sequence();
		
		if(!isThrowingPie)
			seq.AppendInterval(3f);
		
		if(target)
			seq.AppendCallback(() => _anim.SetTrigger(isThrowingPie ? Throw : Shoot));
		
		seq.Append(DOTween.To(() => giftRig.weight, value => giftRig.weight = value, 0f, 0.5f));
		seq.Insert(2f, DOTween.To(() => threatRig.weight, value => threatRig.weight = value, 0f, 0.5f));
		
		if(aimRig)
			seq.Insert(2f,DOTween.To(() => aimRig.weight, value => aimRig.weight = value, .0f, 0.5f));
		
		if(!_didPlayerLose) //Suitcase to winSpot
			BriefCaseSequence();
	}
	
	private void OnPlayerLose()
	{
		_didPlayerLose = true;
		
		Projectile();
	}
	
	private void OnPlayerWin()
	{
		_didPlayerLose = false;

		Projectile();
	}
	
	public void CakeLaunch()
	{
		pie.isKinematic = false;
		pie.transform.parent = null;
		var targetPos = (_didPlayerLose
			? GameObject.FindGameObjectWithTag("PlayerHead")
			: GameObject.FindGameObjectWithTag("EnemyHead")).transform.position;
		targetPos.y += 2f;
		
		var direction = targetPos - pie.position;
		
		pie.rotation = Quaternion.LookRotation(direction);

		pie.AddForce(direction * bulletForce, ForceMode.Impulse);
		Vibration.Vibrate(30);
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

		var targetPos = (_didPlayerLose
			? GameObject.FindGameObjectWithTag("PlayerHead").transform.position
			: GameObject.FindGameObjectWithTag("EnemyHead").transform.position);
		targetPos.y += 1f;
		
		var direction = targetPos - missile.transform.position;
		missileRb.AddForce(direction * bulletForce, ForceMode.Impulse);
		Vibration.Vibrate(30);
	}

	public void PlayerDieOnAnimation()
	{
		DOTween.To(() => aimRig.weight, value => aimRig.weight = value, 0f, 2f);
	}
}
}



