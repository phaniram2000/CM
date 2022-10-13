using UnityEngine;

public class CheatingStudentController : MonoBehaviour
{
	[SerializeField] private GameObject alphabetsParticleSystem;
	[SerializeField] private Animator animator;
	public static bool IsCheating;
	private static readonly int ToCopyHash = Animator.StringToHash("ToCopy");
	private static readonly int VictoryHash = Animator.StringToHash("Victory");
	private static readonly int CaughtHash = Animator.StringToHash("Caught");

	public CheatingCanvas canvas;

	private void OnEnable()
	{
		CheatingEvents.CheaterFound += OnGotFound;
		CheatingEvents.DoneCheating += OnCompleteCheating;
	}

	private void OnDisable()
	{
		CheatingEvents.CheaterFound -= OnGotFound;
		CheatingEvents.DoneCheating -= OnCompleteCheating;
	}
	
	public void PlayCopyAnim()
	{
		animator.SetBool(ToCopyHash, true);
		IsCheating = true;
		alphabetsParticleSystem.SetActive(true);
	}

	public void PlayCoveringAnim()
	{
		animator.SetBool(ToCopyHash, false);
		IsCheating = false;
		alphabetsParticleSystem.SetActive(false);
	}

	public void PlayVictoryAnim()
	{
		print("Victory");
		animator.SetTrigger(VictoryHash);
		alphabetsParticleSystem.SetActive(false);
	}

	public void PlayCaughtAnim()
	{
		IsCheating = false;
		animator.SetTrigger(CaughtHash);
	}

	private void OnGotFound()
	{
		PlayCaughtAnim();
	}

	private void OnCompleteCheating()
	{
		PlayVictoryAnim();
	}
}
