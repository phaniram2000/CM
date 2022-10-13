using UnityEngine;

public class CheatingRuleSet : MonoBehaviour, IRuleSet
{
	public bool CanResetInput => true;
	public int CheckGameResult(int input)
	{
		return DoesMeetOverflowCondition(input) ? 1 : 0;
	}

	public bool DoesMeetUnderflowCondition(int input)
	{
		return true;
	}

	public bool DoesMeetOverflowCondition(int input)
	{
		//if found while cheating, return 1 else 0
		return true;
	}

	public bool TryResetInput()
	{
		return true;
	}
}
