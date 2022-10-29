using DG.Tweening;
using UnityEngine;

public class Prankster : MonoBehaviour
{
	public static bool IsPranking;
	
	private Animator _animator;
	
	private static readonly int StartPrankHash = Animator.StringToHash("StartPrank");
	private static readonly int EndPrankHash = Animator.StringToHash("EndPrank");
	private static readonly int GotFoundHash = Animator.StringToHash("GotFound");
	private static readonly int DonePrankingHash = Animator.StringToHash("DonePranking");
	private static readonly int RunHash = Animator.StringToHash("Run");

	public PrankCanvas prankCanvas;
	[SerializeField] private Transform finalRunPosTransform;
	[SerializeField] private Transform jumpPosTransform;
	[SerializeField] private GameObject bubbleStream;
	[SerializeField] private GameObject liquidStream;

	private void OnEnable()
	{
		ShowerPrankEvents.StartPrank += OnStartPrank;
		ShowerPrankEvents.EndPrank += OnEndPrank;
		ShowerPrankEvents.GotFoundPranking += OnGotFound;
		ShowerPrankEvents.DonePranking += OnDonePranking;
	}

	private void OnDisable()
	{
		ShowerPrankEvents.StartPrank -= OnStartPrank;
		ShowerPrankEvents.EndPrank -= OnEndPrank;
		ShowerPrankEvents.GotFoundPranking -= OnGotFound;
		ShowerPrankEvents.DonePranking -= OnDonePranking;
	}

	private void Start()
	{
		_animator = GetComponent<Animator>();
	}

	private void OnStartPrank()
	{
		_animator.SetTrigger(StartPrankHash);
		StartShampoo();
		IsPranking = true;
	}

	private void OnEndPrank()
	{
		_animator.SetTrigger(EndPrankHash);
		StopShampoo();
		IsPranking = false;
	}

	private void StartShampoo()
	{
		bubbleStream.SetActive(true);
		liquidStream.SetActive(true);
	}

	private void StopShampoo()
	{
		bubbleStream.SetActive(false);
		liquidStream.SetActive(false);
	}
	
	private void OnGotFound()
	{
		var x = new Vector3(jumpPosTransform.position.x,
			transform.position.y,
			jumpPosTransform.position.z);

		transform.DOLookAt(x, 0.25f).SetEase(Ease.Linear).OnComplete(()=>
			{
				_animator.SetTrigger(GotFoundHash);
				transform.DOJump(jumpPosTransform.position, 1, 1, 0.25f).SetEase(Ease.Linear).OnComplete(() =>
				{
					transform.DOMove(finalRunPosTransform.position, 3f).SetEase(Ease.Linear);
				});
			});
	}

	private void OnDonePranking()
	{
		_animator.SetTrigger(DonePrankingHash);
	}
	
}
