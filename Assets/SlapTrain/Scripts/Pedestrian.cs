using UnityEngine;

public class Pedestrian : MonoBehaviour
{
	[SerializeField] private ParticleSystem slapParticle;
	[SerializeField] private ParticleSystem pastryParticle;
	
	private Animator _animator;
	private static readonly int GotHitHash = Animator.StringToHash("Hit");
	private static readonly int AnimNumber = Animator.StringToHash("AnimationNumber");

	[SerializeField] private bool toHoldingPhone;
	[SerializeField] private bool toHoldInRightHand;
	
	[SerializeField] private GameObject rightHandPhone;
	[SerializeField] private GameObject leftHandPhone;

	[SerializeField] private GameObject faceCake;

	private GameObject _activePhone;
	private bool _isPlayerHoldingCake;

	private void OnEnable()
	{
		GameEventsTrain.IsHoldingCake += IfHoldingCake;
	}

	private void OnDisable()
	{
		GameEventsTrain.IsHoldingCake -= IfHoldingCake;
	}

	private void Start()
	{
		_animator = GetComponent<Animator>();

		if (!toHoldingPhone)
		{
			var x = Random.Range(1, 5);
			_animator.SetInteger(AnimNumber, x);
			print(x);
			return;
		}

		if (toHoldInRightHand)
		{
			rightHandPhone.SetActive(true);
			_activePhone = rightHandPhone;
		}
		else
		{
			leftHandPhone.SetActive(true);
			_activePhone = leftHandPhone;
		}
		faceCake.SetActive(false);
	}

	private void OnTriggerEnter(Collider other)
	{
		if (!other.CompareTag("Player")) return;
		
		_animator.SetTrigger(GotHitHash);
		
		if(_isPlayerHoldingCake) pastryParticle.Play(); 
		else slapParticle.Play();

		GameManagerTrain.Instance.totalSlappedPedestrians++;
		GameManagerTrain.Instance.FillTheSlapMeter();
		
		if(_isPlayerHoldingCake) faceCake.SetActive(true);
	}

	public GameObject GetActivePhone()
	{
		return _activePhone;
	}

	private void IfHoldingCake()
	{
		_isPlayerHoldingCake = true;
	}
}
