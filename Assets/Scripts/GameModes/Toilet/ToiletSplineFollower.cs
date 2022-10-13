using DG.Tweening;
using Dreamteck.Splines;
using UnityEngine;

public class ToiletSplineFollower : MonoBehaviour
{
	public ToiletHelper Helper { get; private set; }
	public SplineFollower SplineFollower { get; private set; }
	
	private void Start()
	{
		SplineFollower = GetComponent<SplineFollower>();
		Helper = (ToiletHelper) GameRules.GetRuleSet.GetHelperBase;

		transform.position = Helper.GetHiddenLocation();
	}

	public ToiletNpc GetNpc(out bool isFemale)
	{
		if (TryGetComponent(out ToiletMale male))
		{
			isFemale = false;
			return male;
		}

		TryGetComponent(out ToiletFemale female);

		isFemale = true;
		return female;
	}
	
	public void MoveAfterKick()
	{
		transform.DOMove(transform.position + transform.forward * 10f, 5f)
			.SetEase(Ease.Linear);

		if (TryGetComponent(out ToiletFemale female))
			female.StartChasing();
	}

	public void StopFollowing() => SplineFollower.follow = false;
}