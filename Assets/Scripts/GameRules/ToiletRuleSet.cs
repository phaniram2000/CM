using UnityEngine;

public class ToiletRuleSet : MonoBehaviour, IRuleSet
{
	[HideInInspector] public ToiletHelper helper;
	
	private void Awake() => helper = GetComponent<ToiletHelper>();

	//////// Interface implementations
	public HelperBase GetHelperBase => helper;

	public bool CanResetInput => false;

	public int CheckGameResult(int input)
	{
		if (DoesMeetUnderflowCondition(input)) return -1;
		if (DoesMeetOverflowCondition(input)) return 1;

		return 0;
	}
	public bool DoesMeetUnderflowCondition(int input) => !ToiletHelper.GetAreSignsSwapped;
	public bool DoesMeetOverflowCondition(int input) => false;
	public bool TryResetInput()
	{
		// if they arent swapped, it is already reset
		if (!ToiletHelper.GetAreSignsSwapped) return true;
		
		//if they have been swapped, no functionality is planned to be put here
		return false;
	}
}